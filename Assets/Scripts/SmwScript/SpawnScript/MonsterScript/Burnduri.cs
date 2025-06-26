//���帮
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnduri : EnemyBase
{

    [Header("�÷��̾� ����Ʈ")]
    public List<Transform> players = new List<Transform>();//�÷��̾� ���⿡ �����

    Vector3 Target; //���� ���� ��ǥ ��ġ�� ������ ����
    Vector3 myPos; //�ڽ��� ���� ��ġ�� ������ ����

    [Header("�̵� ����")]
    [SerializeField] float MoveSpeed = 1f;// �⺻ �̵��ӵ�
    [SerializeField] float chargeSpeed = 10f; // ��¡�Ҷ� �̵��ӵ� 
    [SerializeField] float chargeDistance = 3f;// �ش� �����ȿ� ������ �������� �ٲ�� �Ÿ�

    [Header("�Ÿ� ����")]
    [SerializeField] float triggerDistance = 10f;// ���� �Ҷ� �̵��ϴ°Ÿ� 
    [SerializeField] float fixedY = 0f;//�߰����� ���� ���� �ʿ��Ҷ� 


    bool isCharging = false; //���� ��¡ �������� �ƴ��� üũ 


    Terrain terrain; //Terrain �� ������ �޾ƿ� Terrain���� 

    private Coroutine updateRoutine;// �ڷ�ƾ�� ����� ���� (�ڷ�ƾ�� ����� �Ǵ� ��Ȳ�� �ֱ� ������)


    Transform nearestPlayer; //�÷��̾��߿��� ���� �����ָ� ���� Transform����  

    Animator animator; //�ڱ��ڽ��� ���� ������ �ִϸ����� ����
    Collider myCollider;// �ڱ��ڽ� �浹ü�� ������ �浹ü ���� 

    [SerializeField] GameObject CrashBurnduri; //���帮�� ������ ���帮���� ���� ��ƼŬ

    [SerializeField] float FreezeTime = 3; // ���帮�� �ص��Ǵµ� �ɸ��� �ð� 

    [SerializeField] GameObject IsFreeze;// ���帮�� ������� ų ���� ������

    string AppearAni = "anim_01_MON001_Bduri_Appearance"; //���帮 ó�� ���� �ֳ����̼� �̸� 
    string state3 = "anim_03_MON001_Bduri_Encounter"; //��¡�� �ϱ��� ��¡�غ� ���ϸ��̼� �̸� 
    string state5 = "anim_01_MON001_Bduri_Disappearance";// ���帮�� ������� �� �ִϸ��̼� �̸�



    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;//��ü�� ���� ���� �ƴϸ� �Լ����� 
        if (other.CompareTag("Player"))//�浹ü�� ������ �÷��̾��� ��ũ�� �ް�������  
        {
            if (ImFreeze == true)//�������� ���� ���°� ���������� 
            {
                ImFreeze = false;// ������ ���� �ƴ϶�� �Ҳ��� 
                StartCoroutine(FreezeCor());//���ڽ��� �ص��� 
            }
            else if (ImFreeze == false)// ���� ������ �ƴϸ� 
            {
                damage = 1;//�������� �ϴ� 1���� 
                Manager.Instance.observer.HitPlayer(damage);// �̱������� ���� �Ǿ� �ִ� HitPlayer �Լ��� �����Ͽ� �÷��̾�� �������� �ش�.

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ������ ���� ����(normal)�� ��(direction)���� ��� ȸ��(Quaternion) ����
                Instantiate(CrashBurnduri, hitPoint, rot);//���帮���� ��ƼŬ�� ������ 

                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);//���帮�� Ǯ�� ��ȯ
                }

            }
        }

        else if (other.CompareTag("Monster"))//���� ��ũ�� �޸� ������Ʈ �ϰ�� 
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
                GameObject inst = Instantiate(CrashBurnduri, hitPoint, rot);// 


                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }

            }
        }

        else if (other.CompareTag("Aube"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            Vector3 normal = (hitPoint - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(normal);

            GameObject inst = Instantiate(CrashBurnduri, hitPoint, rot);


            if (PhotonNetwork.IsMasterClient)
            {
                PoolManager.Instance.DespawnNetworked(gameObject);
            }

        }
    }


    public override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();

        myCollider.enabled = false;

        terrain = Terrain.activeTerrain;
        isCharging = false;

        StartCoroutine(GoBurnduri());
    }

    void FindPlayer()
    {
        if (players.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");
            foreach (var obj in objs)
            {
                players.Add(obj.transform);
            }
        }
    }

    IEnumerator GoBurnduri()
    {

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni))
            yield return null;

        myCollider.enabled = true;

        updateRoutine = StartCoroutine(UpdateDistance());
        StartCoroutine(MoveRoutine());
        yield return null;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        myCollider.enabled = false;
    }


    IEnumerator UpdateDistance()
    {
        while (true)
        {
            if (players != null && players.Count == 0)
            {
                FindPlayer();
                yield return null;
            }

            for (int i = players.Count - 1; i >= 0; i--)
            {
                Transform p = players[i];

                if (p == null || !p.gameObject.activeInHierarchy)
                {
                    players.RemoveAt(i);
                }
            }

            myPos = transform.position;
            float minDistance = Mathf.Infinity;// �ϴ� ���� ū�������� 


            foreach (var player in players)
            {

                // �Ÿ����
                float Distance = Vector3.Distance(myPos, player.position);

                //�������� ã�Ҵ� �ּ� �Ÿ����� ������ ����
                if (Distance < minDistance)
                {
                    minDistance = Distance;
                    nearestPlayer = player;
                }
            }

            if (nearestPlayer != null)
            {
                Target = nearestPlayer.position;

                transform.LookAt(Target);
            }


            yield return new WaitForSeconds(0.5f);
        }
    }


    IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (!isCharging)
            {
                float CheckNear = Vector3.Distance(myPos, Target);

                if (CheckNear < chargeDistance)
                {

                    if (updateRoutine != null)
                    {
                        StopCoroutine(updateRoutine);
                    }
                    isCharging = true;


                    StartCoroutine(ChargeRoutine());

                    yield break;
                }
                else
                {
                    myPos = transform.position;
                    float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;
                    transform.position = new Vector3(myPos.x, terrainY, myPos.z);
                    transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ChargeRoutine()
    {
        animator.SetTrigger("ChargeStart");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state3));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state3))
            yield return null;

        Vector3 startPos = transform.position;


        float yAngle = transform.eulerAngles.y;
        float zAngle = transform.eulerAngles.z;

        Quaternion Rot = Quaternion.Euler(0f, yAngle, zAngle);

        transform.rotation = Rot;

        while (Vector3.Distance(transform.position, startPos) < triggerDistance)
        {
            myPos = transform.position;
            float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY;
            transform.position = new Vector3(myPos.x, terrainY, myPos.z);
            transform.position += transform.forward * chargeSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        StartCoroutine(DieBurnduri());
    }

    IEnumerator DieBurnduri()
    {
        myCollider.enabled = false;
        animator.SetTrigger("disappear");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state5));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state5))
            yield return null;

        if (PhotonNetwork.IsMasterClient)
        {
            PoolManager.Instance.DespawnNetworked(gameObject);
        }

    }


    public override void Move()
    {
        StartCoroutine(FreezeCor());
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
            StopAllCoroutines();
            ImFreeze = isFreeze;
            IsFreeze.SetActive(true);
            animator.speed = 0f;
        }
        else if (isFreeze == false)
        {
            Debug.Log(isFreeze + " ������ ����");// �̰� �Ѿ� �����ϴ°ǰ�
            ImFreeze = isFreeze;
            StartCoroutine(FreezeCor());
        }
        else
        {
            Debug.Log("���帮 ������ ���峲");
        }
    }

    IEnumerator FreezeCor()
    {
        yield return new WaitForSeconds(FreezeTime);
        animator.speed = 1f;
        StartCoroutine(GoBurnduri());
        IsFreeze.SetActive(false);

    }


}
