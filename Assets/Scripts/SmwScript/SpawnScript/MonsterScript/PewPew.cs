using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PewPew : EnemyBase
{

    Vector3 Position;

    float Radius;
    float MoveSpeed = 10;
    float Angle;

    int rotateDirection;   //���ΰ��� �ð� �ݽð�
    float fixedY = 0f;// ���߿� �����ϰ� ���鲨

    Coroutine rotateCoroutine;

    Terrain terrain;

    [SerializeField] GameObject CrashPewPew;

    Collider myCollider;

    float MoveSpeedSave;

    private PewPewSp Spawner;

    Animator animator;

    [SerializeField] float FreezeTime = 3;

    [SerializeField] GameObject IsFreeze;

    Vector3 newPos;
    Vector3 moveDir;

    float terrainY;

    [SerializeField] float CenterNoSpawn = 5f;

    public override void OnEnable()
    {
        base.OnEnable();

        animator = GetComponent<Animator>();

        if (Spawner == null)
        {
            Spawner = FindObjectOfType<PewPewSp>();
        }

        myCollider = GetComponent<Collider>();

        myCollider.enabled = false;

        terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogWarning("Ʈ���� ���� Ʈ���ξ�����.");
        }
        else
        {
            Vector3 tPos = terrain.transform.position;
            Vector3 tSize = terrain.terrainData.size;

            float centerX = tPos.x + tSize.x * 0.5f;
            float centerZ = tPos.z + tSize.z * 0.5f;

            Position = new Vector3(centerX, 0, centerZ);//Ʈ���α��� �߽�
        }

        float RandomScale = Random.Range(1, 4) * 0.5f;
        transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);

        Angle = Random.Range(0f, Mathf.PI * 2f);
        Radius = Random.Range(CenterNoSpawn, 20f);
        rotateDirection = Random.value < 0.5f ? 1 : -1;
        rotateCoroutine = StartCoroutine(GoPewPew());//���� ���� �����Ѱ� �� �ʱ�ȭ ����

    }


    public override void OnDisable()
    {
        base.OnDisable();
        myCollider.enabled = false;
        ImFreeze = false;
    }


    IEnumerator GoPewPew()
    {
        myCollider = GetComponent<Collider>();

        float angularSpeed = MoveSpeed / Radius;
        Angle -= angularSpeed * Time.deltaTime * rotateDirection;


        float x = Position.x + Mathf.Cos(Angle) * Radius;
        float z = Position.z + Mathf.Sin(Angle) * Radius;

        terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;
        newPos = new Vector3(x, terrainY, z);

        moveDir = (newPos - transform.position).normalized;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
        transform.position = newPos;

        yield return new WaitForSeconds(2f);


        myCollider.enabled = true;

        while (true)
        {
            angularSpeed = MoveSpeed / Radius; //�� �ѷ��� ���� �ӵ�
            Angle -= angularSpeed * Time.deltaTime * rotateDirection;//������ ȸ�� ���⿡ ���� �ٲ���

            //��ġ ����ؼ� �̵�
            x = Position.x + Mathf.Cos(Angle) * Radius;
            z = Position.z + Mathf.Sin(Angle) * Radius;

            terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;
            newPos = new Vector3(x, terrainY, z);

            moveDir = (newPos - transform.position).normalized;

            if (moveDir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }

            transform.position = newPos;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Manager.Instance.observer.HitPlayer(damage);

            Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������

            Vector3 normal = (hitPoint - transform.position).normalized;// ������
            Quaternion rot = Quaternion.LookRotation(normal);// ������

            GameObject inst = Instantiate(CrashPewPew, hitPoint, rot);

            PoolManager.Instance.Despawn(gameObject);
            if (Spawner != null)
            {
                Spawner.SpawnOne();

            }
        }

        if (other.gameObject.tag == "Monster")
        {
            EnemyBase otherEnemy = other.GetComponent<EnemyBase>();
            if (otherEnemy == null)
                return; 


            if (otherEnemy.ImFreeze)
            {
                Manager.Instance.observer.HitPlayer(damage);

                Vector3 hitPoint = other.ClosestPoint(transform.position);

                Vector3 normal = (hitPoint - transform.position).normalized;
                Quaternion rot = Quaternion.LookRotation(normal);

                GameObject inst = Instantiate(CrashPewPew, hitPoint, rot);

                PoolManager.Instance.Despawn(gameObject);
                Spawner.SpawnOne();
            }
        }
        if (other.gameObject.tag == "Aube")
        {
            Manager.Instance.observer.HitPlayer(damage);

            Vector3 hitPoint = other.ClosestPoint(transform.position);

            Vector3 normal = (hitPoint - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(normal);

            GameObject inst = Instantiate(CrashPewPew, hitPoint, rot);

            PoolManager.Instance.Despawn(gameObject);
        }

    }



    public override void Move(Vector3 direction)
    {
        photonView.RPC("MoveRPC", RpcTarget.All, direction);
    }

    [PunRPC]
    public void MoveRPC(Vector3 direction)
    {

    }

    public override void CsvEnemyInfo()
    {
        throw new System.NotImplementedException();
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction, isFreeze);
    }

    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)
        {
            ImFreeze = isFreeze;
            IsFreeze.SetActive(true);
            MoveSpeedSave = MoveSpeed;
            MoveSpeed = 0;
            myCollider.enabled = false;
            animator.speed = 0f;
        }
        else if (isFreeze == false)
        {
            ImFreeze = isFreeze;
            StartCoroutine(FreezeCor());
        }
        else
        {
            Debug.Log("ǻǻ ������ ���峲");
        }
    }
    IEnumerator FreezeCor()
    {

        yield return new WaitForSeconds(FreezeTime);
        MoveSpeed = MoveSpeedSave;
        animator.speed = 1f;
        myCollider.enabled = true;
        IsFreeze.SetActive(false);
    }


}
