using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ShookShook : EnemyBase
{

    Vector3 Position;// ���� ��ġ�� �̵� �� ���� ��ġ�� ������ ����
    Vector3 Terrainpos;// Terrain�� ���� ��ġ�� ������ ����
    Vector3 Terrainsize;// Terrain �������� ũ�⸦ ������ ����

    Vector3 Target;// �̵� ��ǥ ������ ������ ����

    float MoveSpeed = 5f;// �̵� �ӵ�
    Terrain terrain;// ���� Ȱ��ȭ�� Terrain ������Ʈ�� �����մϴ�.

    float minX;// ������ �ּ� X ��ǥ
    float maxX;// ������ �ִ� X ��ǥ
    float minZ;// ������ �ּ� Z ��ǥ
    float maxZ;// ������ �ִ� Z ��ǥ
    public float fixedY;// Y ��ġ ������ ������

    Coroutine Coroutine;// �̵� �ڷ�ƾ�� ������ ����

    [SerializeField] GameObject CrashShookShook;// ������ ������ ���� ��ƼŬ
    Collider myCollider;// �浹ü �ڱ��ڽ� �����Ұ� 

    float MoveSpeedSave;// ���� ���� ���� �ӵ� ������ ���� ����

    Animator animator;//�ڱ��ڽ��� Animator ������Ʈ�� ���� animator ����

    [SerializeField] float FreezeTime = 3;// ���� ���� ���� �ð�
    [SerializeField] GameObject IsFreeze;// ���� ������

    public override void OnEnable()              
    {
        base.OnEnable();                     
        myCollider = GetComponent<Collider>();// �ڱ��ڽ��� Collider ������Ʈ�� ������ ����
        myCollider.enabled = false;// �浹ü ����

        terrain = Terrain.activeTerrain;// ������ Ȱ��ȭ�� Terrain ������Ʈ ��������
        Terrainsize = terrain.terrainData.size;// Terrain�� ũ�� ���� ����
        Terrainpos = terrain.transform.position;// Terrain�� ���� ��ġ ����

        Position = transform.position;// ���� ������Ʈ ��ġ�� Position�� ����

        minX = Terrainpos.x;// ������ �ּ� X
        maxX = Terrainpos.x + Terrainsize.x;// ������ �ִ� X
        minZ = Terrainpos.z;// ������ �ּ� Z
        maxZ = Terrainpos.z + Terrainsize.z;// ������ �ִ� Z

        // ������Ʈ�� �� �� �� �Ÿ� ���
        float Left = Mathf.Abs(Position.x - minX);
        float Right = Mathf.Abs(Position.x - maxX);
        float Bottom = Mathf.Abs(Position.z - minZ);
        float Top = Mathf.Abs(Position.z - maxZ);

        // �� �Ÿ� �� ���� ����� ������� �Ÿ�
        float NearPos = Mathf.Min(Left, Right, Bottom, Top);

        if (NearPos == Left)// ���� ���� ���� �����ٸ�
        {
            Position.x = minX;// ���� ��ġ�� ���� ���� ����
            Target = new Vector3(maxX, 0, Position.z); // ��ǥ�� ������  �𼭸��� ����
        }
        else if (NearPos == Right)// ������ ���� ���� �����ٸ�
        {
            Position.x = maxX;// ���� ��ġ�� ������ ���� ����
            Target = new Vector3(minX, 0, Position.z); // ��ǥ�� ���� �𼭸��� ����
        }
        else if (NearPos == Bottom)// �Ʒ� ���� ���� �����ٸ�
        {
            Position.z = minZ;// ���� ��ġ�� �Ʒ� �𼭸��� ����
            Target = new Vector3(Position.x, 0, maxZ); // ��ǥ�� ���� �𼭸��� ����
        }
        else if (NearPos == Top)// �� ���� ���� �����ٸ�
        {
            Position.z = maxZ;// ���� ��ġ�� �� ���� ����
            Target = new Vector3(Position.x, 0, minZ); // ��ǥ�� �Ʒ� �𼭸��� ����
        }
        else
        {
            Debug.Log("������ ���밪 ����ִ°� ������");
        }

        // Y ��ġ�� ���� ���̿� ���� ����
        float terrainY = terrain.SampleHeight(Position) + transform.localScale.y / 2f + fixedY;
        Position.y = terrainY;
        transform.position = Position;// ��������

        terrainY = terrain.SampleHeight(Target) + transform.localScale.y / 2f + fixedY;
        Target.y = terrainY;// ��ǥ ������ Y ��������

        transform.position = Position;//������������ ��ǥ����

        Coroutine = StartCoroutine(GoShookShook());//�̵� �ڷ�ƾ ����
    }

    public override void OnDisable() 
    {
        base.OnDisable(); 
        if (Coroutine != null)               
        {
            StopCoroutine(Coroutine);             
            Coroutine = null;                     
        }
    }

    IEnumerator GoShookShook() 
    {
        Vector3 pos = transform.position;
        myCollider.enabled = true;// �浹ü Ű��  
        Target.y = pos.y;// ��ǥ Y�� ���� Y ����             

        // ��ǥ�� ���� ������ ������ �ݺ�
        while (Vector3.Distance(transform.position, Target) > 0.5f)
        {
            transform.LookAt(Target); //Target�� �ٶ󺸰� �� 
            transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime; // ������ ���� �ڵ� 

            // ���� ���̿� �°� Y ����
            float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;
            pos = transform.position;// ���� ��ġ����
            pos.y = terrainY;//y �����ؾ��ؼ� 
            transform.position = pos;// �̵����� 

            yield return new WaitForFixedUpdate();// FixedUpdate �ֱ⸶�� ���
        }

        transform.position = Target;// ��Ȯ�� ��ǥ ���� �������ѹ���(���������ϱ�)
        if (PhotonNetwork.IsMasterClient)
        {
            PoolManager.Instance.DespawnNetworked(gameObject); //PoolManager���� ��ȯ
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;// ���� Ŭ���̾�Ʈ�� �ƴϸ� ����

        if (other.CompareTag("Player"))// �÷��̾�� �浹���� ��
        {
            if (ImFreeze == true)// �ڽ��� ���������
            {
                ImFreeze = false;// ���� ����
                StartCoroutine(FreezeCor());// ���� ���� �ڷ�ƾ ����
            }
            else if (ImFreeze == false)// ������� ������
            {
                damage = 1;// ������ ����
                Manager.Instance.observer.HitPlayer(damage);// �÷��̾ ������ ����

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;//���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(CrashShookShook, hitPoint, rot);  // ������ �״� ��ƼŬ ����

                if (PhotonNetwork.IsMasterClient)   
                {
                    PoolManager.Instance.DespawnNetworked(gameObject); // PoolManager�� ��ȯ
                }
            }
        }
        else if (other.CompareTag("Monster"))// �ٸ� ���Ϳ� �浹���� ��
        {
            EnemyBase otherEnemy = other.GetComponent<EnemyBase>(); // EnemyBase �������� �ٸ����͵� EnemyBase �־ 
            if (otherEnemy == null)// ������Ʈ ������ ���
            {
                Debug.Log("���� EnemyBase �� ����");
                return;
            }

            if (ImFreeze == false && otherEnemy.ImFreeze == true) // �ڽž����ƴϰ� ��밡 ���� ���¸�
            {
                otherEnemy.ImFreeze = false;// �浹�� ���� ���� ����
                otherEnemy.Move();// ��� Move() ȣ���ؼ� �ص��� Ŵ

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;//���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(CrashShookShook, hitPoint, rot); // ������ �״� ��ƼŬ ����

                if (PhotonNetwork.IsMasterClient)   // ������ Ŭ���̾�Ʈ�� ���� ����
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }
            }
        }
        else if (other.CompareTag("Aube"))        // Aube �±׿� �浹���� ��
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
            Vector3 normal = (hitPoint - transform.position).normalized;//���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
            Instantiate(CrashShookShook, hitPoint, rot); // ������ �״� ��ƼŬ ����

            if (PhotonNetwork.IsMasterClient)       // ������ Ŭ���̾�Ʈ�� ���� ����
            {
                PoolManager.Instance.DespawnNetworked(gameObject);
            }
        }
    }

    public override void Move()
    {
        StartCoroutine(FreezeCor());// ���� ���� �ڷ�ƾ ����
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction, isFreeze);
    }

    [PunRPC]                                   
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)// ���� ���� �϶�
        {
            IsFreeze.SetActive(true);// ���� ������ Ȱ��ȭ
            ImFreeze = isFreeze;// ���� ����
            MoveSpeedSave = MoveSpeed;// ���� �ӵ� ����
            MoveSpeed = 0;// �̵� �ӵ� 0���� ����
            animator.speed = 0f;// �ִϸ��̼� ��� ����
        }
        else if (isFreeze == false)// ���� ���� ���� �϶�
        {
            ImFreeze = isFreeze;// ���� �÷��� ����
            StartCoroutine(FreezeCor());// ���� �ð� �� ���� �ڷ�ƾ ����
        }
        else
        {
            Debug.Log("������ ������ ���峲");
        }
    }

    IEnumerator FreezeCor()// ���� ���� ���� �� �ڵ� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(FreezeTime); // ������ �ð���ŭ ���
        MoveSpeed = MoveSpeedSave;// ���� �̵� �ӵ� ����
        animator.speed = 1f;// �ִϸ����� �ٽ����
        IsFreeze.SetActive(false);// ���� ������ ��Ȱ��ȭ
    }

} 
