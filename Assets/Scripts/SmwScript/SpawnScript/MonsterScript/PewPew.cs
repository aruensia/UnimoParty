//ǻǻ
using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PewPew : EnemyBase
{

    Vector3 Position;//	Terrain �߾� ��ǥ�� ����. ���� �˵��� �׸� �߽������� ���

    float Radius; // Position�� �������� �̵��� �˵��� ������
    float MoveSpeed = 10; //�̵��ӵ� 
    float Angle; //���� ȸ�� ������ ���� 

    int rotateDirection;   //���ΰ��� �ð� �ݽð�
    float fixedY = 0f;// Terrain �������� ���̸� �����ִ� ����

    Terrain terrain; //Terrain �� �����Ͷ� �ش� Terrain�� ���̸� �޾ƿ�

    [SerializeField] GameObject CrashPewPew; // ǻǻ�� ������ �������� ��ƼŬ

    Collider myCollider;// ���� �浹 ���� 

    float MoveSpeedSave;// ������ �����϶� ���� �̵��ӵ��� ����ٺ��� 

    PewPewSp Spawner; //ǻǻ ���� �����ʶ� �������� ��

    Animator animator; //ǻǻ ���� �ִϸ��̼� 

    [SerializeField] float FreezeTime = 3; //���� �����϶� �ص��Ǹ� �ص��Ǵµ� �ɸ��½ð�

    [SerializeField] GameObject IsFreeze; //ǻǻ �������¸� ������ ������

    Vector3 newPos;//�� ������ ����� ���� �̵��� ��ġ ����

    Vector3 moveDir;//newPos - ������ġ�� ����ȸ�� �� ���

    float terrainY;//���� ǥ�� ����+���� ���� ����+fixedY ��갪

    [SerializeField] float CenterNoSpawn = 5f;// �߽� ��ó ���� ���� �ݰ�

    public override void OnEnable()
    {
        base.OnEnable();//������ ���� ���� �ʿ����Լ� 

        animator = GetComponent<Animator>();// �ڽ��� �ִϸ����͸� �����

        if (Spawner == null)//ǻǻ���� �����ʰ� ����� �ȵ� ������  
        {
            Spawner = FindObjectOfType<PewPewSp>();//Spawner�� PewPewSp ������Ʈ�� �ִ°� ��ƶ� ObjectOfType�� ������Ʈ�� 1���������� �ᵵ�� �׿ܴ� x
        }

        myCollider = GetComponent<Collider>();//myCollider�� �� �ڽ� �浹ü�� �־��ٲ��� 

        myCollider.enabled = false;//�ϴ� �浹ü������ 

        terrain = Terrain.activeTerrain;//terrain �� Terrain �� ������ �����

        if (terrain == null) //terrain �ȿ� ���� ������
        {
            Debug.LogWarning("Ʈ���� ���� Ʈ���ξ�����.");
        }
        else
        {
            Vector3 tPos = terrain.transform.position;//Terrain ��ǥ 
            Vector3 tSize = terrain.terrainData.size;//Terrain ũ�� 

            float centerX = tPos.x + tSize.x * 0.5f;//�̰Ŵ� Terrain �߽� ��ǥ�� ����� ���� x��
            float centerZ = tPos.z + tSize.z * 0.5f;//�̰Ŵ� Terrain �߽� ��ǥ�� ����� ���� z��

            Position = new Vector3(centerX, 0, centerZ);//Ʈ���α��� �߽�
        }

        float RandomScale = Random.Range(1, 4) * 0.5f;//�̰Ŵ� ǻǻ ũ�⸦ �����ִ� ���� 

        transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);// ǻǻ�� ũ�⸦ �����Ű�� ��

        Angle = Random.Range(0f, Mathf.PI * 2f);//
        Radius = Random.Range(CenterNoSpawn, 20f);//���⼭ ���� ũ�Ⱑ �ٶ��� ����ϸ� �������� ������ ����� ���궧���� CenterNoSpawn�� ���������� ������
        rotateDirection = Random.value < 0.5f ? 1 : -1;//�̰Ŵ� ǻǻ�� �ð�������� ������ �ݽð� �������� ������ �����ִ� �� (�߰����� Random.value�� 0.0 ~ 1.0�� �������� �̰� 1, -1�� �����ش� )
        StartCoroutine(GoPewPew());//GoPewPew() �ڷ�ƾ�� �����ض� 

    }


    public override void OnDisable()
    {
        base.OnDisable();
        myCollider.enabled = false;//�浹ü�� ����� 
        ImFreeze = false;// enemybase�� �ִ� �������������� �ƴ����� �������� �ƴ϶�� �ʱ�ȭ
    }




    IEnumerator GoPewPew()
    {
        float angularSpeed = MoveSpeed / Radius;//���� �̵��ӵ� / ���� ������  �ؼ� ���ӵ��� ���� 
        Angle -= angularSpeed * Time.deltaTime * rotateDirection;// ���ӵ� * ������ ��� �ð� * ����������� ������

        float x = Position.x + Mathf.Cos(Angle) * Radius;//cos(Angle)���� ���� �� ���� ���� ���, ������ ���� ���� �Ÿ��� Ȯ��
        float z = Position.z + Mathf.Sin(Angle) * Radius;//sin(Angle)���� ���� �� ���� ���� ���, ������ ���� ���� �Ÿ��� Ȯ��

        terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;//terrainY�� transform.position �ڱ���ġ��  terrain.SampleHeight(Ʈ���ο� ����)�� ǻǻ �ڽ��� ũ���� ���ݸ�ŭ + fixedY �� �ѳ��̷� ����
        newPos = new Vector3(x, terrainY, z);// ���� ������ ��ǥ ����

        moveDir = (newPos - transform.position).normalized; //normalized: ���� ����. LookRotation�� ���

        if (moveDir.sqrMagnitude > 0.001f) //�ʹ����ϰ� ���� �� �������� �ڵ�
        {
            transform.rotation = Quaternion.LookRotation(moveDir);//ǻǻ�� ȸ������ ȸ���ϴ� �������� �� ���� ���� �� 
        }
        transform.position = newPos;// ȸ�� �� ��ġ�� ��� ��ġ �̵�


        animator.speed = 0f;//�ִϸ��̼� ��� ������ (���� ������)
        yield return new WaitForSeconds(1.5f);// ���������� 1.5�ʱ�޸��� 
        animator.speed = 1f;//�ִϸ��̼� �����̰� 

        myCollider.enabled = true;//�浹ü��Ŵ 

        while (true)
        {
            angularSpeed = MoveSpeed / Radius; //�� �ѷ��� ���� �ӵ�
            Angle -= angularSpeed * Time.deltaTime * rotateDirection;//������ ȸ�� ���⿡ ���� �ٲ���

            //��ġ ����ؼ� �̵�
            x = Position.x + Mathf.Cos(Angle) * Radius;//cos(Angle)���� ���� �� ���� ���� ���, ������ ���� ���� �Ÿ��� Ȯ��
            z = Position.z + Mathf.Sin(Angle) * Radius;//Sin(Angle)���� ���� �� ���� ���� ���, ������ ���� ���� �Ÿ��� Ȯ��

            terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;// ���� ���� �������� ���: ������ ���� ������ ���� �߰���ŭ �ø� + fixedY
            newPos = new Vector3(x, terrainY, z);// ���� ������ ��ǥ ����

            moveDir = (newPos - transform.position).normalized;//normalized: ���� ����. LookRotation�� ���

            if (moveDir.sqrMagnitude > 0.001f) //�ʹ����ϰ� ���� �� �������� �ڵ�
            {
                transform.rotation = Quaternion.LookRotation(moveDir);//ǻǻ�� ȸ������ ȸ���ϴ� �������� �� ���� ���� �� 
            }

            transform.position = newPos;// ȸ�� �� ��ġ�� ��� ��ġ �̵�

            yield return new WaitForFixedUpdate();//0.02�� ���� �ݺ� 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;//��ü�� ���� ���� �ƴϸ� �Լ����� 

        if (other.CompareTag("Player")) //other �� �±װ� Player �� ��� 
        {
            if (ImFreeze == true)//�������� ��� ������
            {
                ImFreeze = false;//����ִ°� �����϶�
                StartCoroutine(FreezeCor());//���� ���� �ڷ�ƾ ����
            }
            else if (ImFreeze == false)// �������°� �ƴҰ�� 
            {
                damage = 1;// ������ �������� 1�̴�
                Manager.Instance.observer.HitPlayer(damage);// �̱������� ���� �Ǿ� �ִ� HitPlayer �Լ��� �����Ͽ� �÷��̾�� �������� �ش�.


                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ������ ���� ����(normal)�� ��(direction)���� ��� ȸ��(Quaternion) ����
                Instantiate(CrashPewPew, hitPoint, rot);// ���� 3���� �����ؼ� ǻǻ ��� ��ƼŬ ����

                if (!PhotonNetwork.IsMasterClient) return;//����ƴϸ� �Լ��� �����ض� 
                Spawner.SpawnOne();// Spawner�ȿ� �ִ� SpawnOne() ���� SpawnOne()�� ǻǻ�� ������ �ٽû����ϰ� �Ҳ���
                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }
            }
        }

        else if (other.CompareTag("Monster"))//�±װ� �����ϰ�� 
        {
            EnemyBase otherEnemy = other.GetComponent<EnemyBase>();//���ͳ��� �浹�ѰŶ� other �� EnemyBase �� ���� otherEnemy �����

            if (otherEnemy == null)//���ڵ�� ��ü������ otherEnemy �ȿ� ���� ������� ���ٰ� �˸�
            {
                Debug.Log("���� EnemyBase �� ����");
                return;
            }

            if (ImFreeze == false && otherEnemy.ImFreeze == true)//���� ���� ���°� �ƴѵ� ���� ������� �ϰ�� 
            {

                otherEnemy.ImFreeze = false;//���濡 ������ Ǯ���ش� 
                otherEnemy.Move();// ���⿡ FreezeCor() �� ����ִ� ���������� ����־  �����Ŵ 

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ������ ���� ����(normal)�� ��(direction)���� ��� ȸ��(Quaternion) ����
                Instantiate(CrashPewPew, hitPoint, rot);// ���� 3���� �����ؼ� ǻǻ ��� ��ƼŬ ����

                if (!PhotonNetwork.IsMasterClient) return;//���常 �����ϰ� 
                Spawner.SpawnOne();// Spawner�ȿ� �ִ� SpawnOne() ���� SpawnOne()�� ǻǻ�� ������ �ٽû����ϰ� �Ҳ���
                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }

            }
        }


        else if (other.CompareTag("Aube"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
            Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ������ ���� ����(normal)�� ��(direction)���� ��� ȸ��(Quaternion) ����
            Instantiate(CrashPewPew, hitPoint, rot);// ���� 3���� �����ؼ� ǻǻ ��� ��ƼŬ ����

            if (!PhotonNetwork.IsMasterClient) return;//���常 �����ϰ� 
            Spawner.SpawnOne();// Spawner�ȿ� �ִ� SpawnOne() ���� SpawnOne()�� ǻǻ�� ������ �ٽû����ϰ� �Ҳ���
            if (PhotonNetwork.IsMasterClient)
            {
                PoolManager.Instance.DespawnNetworked(gameObject);
            }
        }

    }



    public override void Move()
    {
        StartCoroutine(FreezeCor());//�ص� �����ִ� �ڷ�ƾ���� 
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction, isFreeze);// ��� ��󿡰� FreezeRPC �Լ��� ����
    }

    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)// ó�� ���� ���¸� ó������ �Լ� 
    {
        if (isFreeze == true)//����!! �ɸ��� 
        {
            ImFreeze = isFreeze;// ImFreeze�� isFreeze�� true �̴ϱ� true�� ����� 
            IsFreeze.SetActive(true);//�̰Ŵ� ���Ͱ� �����̶�°� ���������� ������ (���� Ȱ��ȭ)
            MoveSpeedSave = MoveSpeed;//���� ���� ������ �̵��� �����ؾ��ϴϱ� ���߿� �ٽ� �ӵ��� ���ƿö� ���� �̵��ӵ��� ������
            MoveSpeed = 0;// ���� �̵��� ������Ŵ 
            animator.speed = 0f;//���������̴� �ִϸ��̼ǵ� ��� ���� 
        }
        else if (isFreeze == false)
        {
            Debug.Log(isFreeze + " ������ ����");// �̰� �Ѿ� �����ϴ°ǰ�
            ImFreeze = isFreeze;//�̰� �Ѿ���� false��
            StartCoroutine(FreezeCor());//�ൿ ������
        }
        else
        {
            Debug.LogWarning("ǻǻ ������ ���峲");
        }
    }
    IEnumerator FreezeCor()
    {

        yield return new WaitForSeconds(FreezeTime);//�ص���Ű�µ� ��޸��½ð� 
        MoveSpeed = MoveSpeedSave;//�̵��ӵ��� �����ӵ��� �ٲ��ٲ���
        animator.speed = 1f;//�ִϸ��̼� �ٽ� �����̰� ��
        IsFreeze.SetActive(false);//���� ������ ��Ȱ��ȭ 
    }


}
