using System.Collections;
using UnityEngine;
using Photon.Pun;

public class ShookShook : EnemyBase
{
    Vector3 Position;
    Vector3 Terrainpos;
    Vector3 Terrainsize;

    Vector3 Target;

    float MoveSpeed = 5f;
    Terrain terrain;

    float minX;
    float maxX;
    float minZ;
    float maxZ;
    public float fixedY;

    Coroutine Coroutine;

    [SerializeField] GameObject CrashShookShook;
    Collider myCollider;

    int FreezeCount = 0;

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    myCollider = GetComponent<Collider>();
    //    myCollider.enabled = false;
    //    terrain = Terrain.activeTerrain;
    //    Terrainsize = terrain.terrainData.size;
    //    Terrainpos = terrain.transform.position;

    //    Position = transform.position;
    //    Target = transform.position;//��ǥ ���߱�� 

    //    minX = Terrainpos.x;
    //    maxX = Terrainpos.x + Terrainsize.x;
    //    minZ = Terrainpos.z;
    //    maxZ = Terrainpos.z + Terrainsize.z;


    //    float Left = Mathf.Abs(Position.x - minX);
    //    float Right = Mathf.Abs(Position.x - maxX);
    //    float Bottom = Mathf.Abs(Position.z - minZ);
    //    float Top = Mathf.Abs(Position.z - maxZ);

    //    float NearPos = Mathf.Min(Left, Right, Bottom, Top);

    //    if (NearPos == Left)
    //    {
    //        Position.x = minX;
    //        Target.x = maxX;

    //    }
    //    else if (NearPos == Right)
    //    {
    //        Position.x = maxX;
    //        Target.x = minX;

    //    }
    //    else if (NearPos == Bottom)
    //    {
    //        Position.z = minZ;
    //        Target.z = maxZ;

    //    }
    //    else if (NearPos == Top)
    //    {
    //        Position.z = maxZ;
    //        Target.z = minZ;

    //    }
    //    else
    //    {
    //        Debug.Log("�ʴ� �� ������?");
    //    }
    //    transform.position = Position;

    //    Coroutine = StartCoroutine(GoShookShook());
    //}

    public override void OnDisable()
    {
        base.OnDisable();
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
            FreezeCount = 0;
        }
    }

    IEnumerator GoShookShook()
    {
        Vector3 pos = transform.position;
        myCollider.enabled = true;
        Target.y = pos.y;
        while (Vector3.Distance(transform.position, Target) > 0.5f)
        {
            transform.LookAt(Target);
            transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;
            float terrainY = terrain.SampleHeight(transform.position);
            terrainY += transform.localScale.y / 2f + fixedY;
            pos = transform.position;
            pos.y = terrainY;
            transform.position = pos;
            yield return new WaitForFixedUpdate();
        }
        transform.position = Target;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            damage = 1;
            Manager.Instance.observer.HitPlayer(damage);
            Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);

            Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������

            Vector3 normal = (hitPoint - transform.position).normalized;// ������
            Quaternion rot = Quaternion.LookRotation(normal);// ������

            GameObject inst = Instantiate(CrashShookShook, hitPoint, rot);

            gameObject.SetActive(false);
        }



    }


    public override void Move(Vector3 direction)
    {
        photonView.RPC("Move1",RpcTarget.All, direction);
    }

    [PunRPC]
    public  void Move1(Vector3 direction)
    {
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
        terrain = Terrain.activeTerrain;
        Terrainsize = terrain.terrainData.size;
        Terrainpos = terrain.transform.position;

        Position = transform.position;
        Target = transform.position;//��ǥ ���߱�� 

        minX = Terrainpos.x;
        maxX = Terrainpos.x + Terrainsize.x;
        minZ = Terrainpos.z;
        maxZ = Terrainpos.z + Terrainsize.z;


        float Left = Mathf.Abs(Position.x - minX);
        float Right = Mathf.Abs(Position.x - maxX);
        float Bottom = Mathf.Abs(Position.z - minZ);
        float Top = Mathf.Abs(Position.z - maxZ);

        float NearPos = Mathf.Min(Left, Right, Bottom, Top);

        if (NearPos == Left)
        {
            Position.x = minX;
            Target.x = maxX;

        }
        else if (NearPos == Right)
        {
            Position.x = maxX;
            Target.x = minX;

        }
        else if (NearPos == Bottom)
        {
            Position.z = minZ;
            Target.z = maxZ;

        }
        else if (NearPos == Top)
        {
            Position.z = maxZ;
            Target.z = minZ;

        }
        else
        {
            Debug.Log("�ʴ� �� ������?");
        }


        if (FreezeCount == 0)
        {
            transform.position = Position;
            FreezeCount++;
        }
        else if (FreezeCount >= 1)
        {
            Debug.Log("�̰Ŵ� ���� ��ź������ �ߴ� ������ ī������ �̰� �Ⱦ��� �����̴� �����ڸ��� ���ϰ���� �𼭸��� �̵��ع���");
        }


        Coroutine = StartCoroutine(GoShookShook());
    }

    public override void CsvEnemyInfo()
    {
        throw new System.NotImplementedException();
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)
        {
            StopAllCoroutines();
        }
        else if (isFreeze == false)
        {
            //StartCoroutine();
        }
        else
        {
            Debug.Log("������ ������ �ڷ�ƾ���峲");
        }


    }


}
