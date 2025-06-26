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
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
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
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(CrashBurnduri, hitPoint, rot);// ���帮 ��� ��ƼŬ ���� 
                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);//���ڽ��� PoolManager�� ��ȯ
                }

            }
        }

        else if (other.CompareTag("Aube"))//���꿡 �浹�ɰ�� 
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
            Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
            Instantiate(CrashBurnduri, hitPoint, rot);// ���帮 ��� ��ƼŬ ���� 
            if (PhotonNetwork.IsMasterClient)
            {
                PoolManager.Instance.DespawnNetworked(gameObject);//���ڽ��� PoolManager�� ��ȯ
            }
        }
    }


    public override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();//�ڽ��� �ִϸ��̼��� �ִ´� animator��
        myCollider = GetComponent<Collider>();// ���ڽ��� �浹ü�� ���� myCollider��

        myCollider.enabled = false;//ó���� ���� ������ ��� �浹�� ���ش�

        terrain = Terrain.activeTerrain; //Terrain�� ������ ��´� 
        isCharging = false;//��¡ ���°� �ƴϹǷ� �ʱ⿣ false�����ش� 

        StartCoroutine(GoBurnduri());// GoBurnduri() �ڷ�ƾ�� ���� �Ѵ� 
    }

    void FindPlayer()// �ش� �Լ��� �÷��̾ ã�� �Լ��� 
    {
        if (players.Count == 0)// �÷��̾ ������ 
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");//���̷�Ű�� �÷��̾� �ױ� �޸��ֵ��� ã�´� 
            foreach (var obj in objs)
            {
                players.Add(obj.transform);//����Ʈ�� �÷��̾� �߰�
            }
        }
    }

    IEnumerator GoBurnduri()
    {

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni));// AppearAni �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni))// ���� �÷��� ���� �ִϸ��̼��� AppearAni ��� �̸��� ���� ��� �ݺ��϶�
            yield return null; // �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        myCollider.enabled = true;// ���帮 �浹ü�� �ٽ� ���� ��������� ������ ������ 

        updateRoutine = StartCoroutine(UpdateDistance()); // UpdateDistance() �ڷ�ƾ�� ������ �׳� ��ŸƮ �ڷ�ƾ�̶� �ٸ����� �̰Ŵ� �߰��� ��ž �ڷ�ƾ���� ����� �־   
        StartCoroutine(MoveRoutine());//  MoveRoutine() �� �����ض� 
    }


    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();//��� �ڷ�ƾ�� ���� �Ҷ�
        myCollider.enabled = false;//�浹ü�� ���ش�
    }


    IEnumerator UpdateDistance()//����� �÷��̾� �Ÿ� ����̶� ����� �÷��̾� ������ �Ĵٺ��°� ���ִ� �ڷ�ƾ 
    {
        while (true)
        {
            if (players != null && players.Count == 0)//�÷��̾� ����Ʈ�z �����ϳ� �÷��̾ ������ 
            {
                FindPlayer();// �÷��̾� ã���Լ� ����
                yield return null;
            }

            for (int i = players.Count - 1; i >= 0; i--)//�÷��̾� ����Ʈ �ȿ� �ֵ�˻� 
            {
                Transform p = players[i];// �ϴ� �÷��̾� ��ǥ

                if (p == null || !p.gameObject.activeInHierarchy)//�÷��̾� ��ǥ�� ���ٰ�߰ų� ���̎��� ������ 
                {
                    players.RemoveAt(i);// �� �÷��̾� ��ǥ�� ���� �Ѵ� 
                }
            }

            myPos = transform.position;
            float minDistance = Mathf.Infinity;// �ϴ� ���� ū�������� 


            foreach (var player in players)//�÷��̾�� ����ŭ�ݺ�
            {


                float Distance = Vector3.Distance(myPos, player.position);//�÷��̾�� ���帮�� �Ÿ��� Distance�� ����

                if (Distance < minDistance)//�������� ã�Ҵ� �ּ� �Ÿ����� ������ ����
                {
                    minDistance = Distance;
                    nearestPlayer = player;
                }
            }

            if (nearestPlayer != null)//nearestPlayer �� ���°� �ƴϸ�
            {
                Target = nearestPlayer.position;//Target�� nearestPlayer.position ���� ��´� ���� ����� �� �̱⶧��

                transform.LookAt(Target);// ���� �����ָ� �Ĵٺ��� �ϴ� LookAt �Լ� 
            }


            yield return new WaitForSeconds(0.5f);// �ʹ� ���ֹݺ��ϸ� ���ɸ��� 0.5�ʸ��� �ض�
        }
    }


    IEnumerator MoveRoutine()//���������� ������ �����ڷ�ƾ
    {
        while (true)
        {
            if (!isCharging)//��¡���°� �ƴ϶�� 
            {
                float CheckNear = Vector3.Distance(myPos, Target); // ���帵�� ����� �÷��̾��� �Ÿ��� CheckNear ������

                if (CheckNear < chargeDistance)// ���� �Ÿ� �̳��� ������ chargeDistance�� ��ũ�� �Ÿ��� �����ѰŶ� 
                {

                    if (updateRoutine != null)//Ȥ�þ��°� üũ 
                    {
                        StopCoroutine(updateRoutine);//���� ����ִ� �ڷ�ƾ ���� 
                    }
                    isCharging = true;//������ ������ �ȵǴϱ� ���Ƿ� �̵��� ����


                    StartCoroutine(ChargeRoutine());// ���������� ��¡�ϴ� �ڷ�ƾ ���� 

                    yield break;//���� �����ΰ��� �ڷ�ƾ�� ���� 
                }
                else
                {
                    myPos = transform.position;// �ٽ��ѹ� �ڱ���ġ ����
                    float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY; //���� ���帮�� �ִ� ��ġ�� Ʈ���� ���� ���� ���帮 yũ�� ���ݸ�ŭ ���̷� ���� + fixedY
                    transform.position = new Vector3(myPos.x, terrainY, myPos.z);// ���⿡�� ���̸� ����
                    transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;// ������ �̵��ӵ���ŭ �̵� (Time.fixedDeltaTime�� 0.02�ʸ��� �����Ѵ� )
                }
            }
            yield return new WaitForFixedUpdate();//0.05�ʸ��� �ݺ�
        }
    }

    IEnumerator ChargeRoutine()//���� ��¡�� �ϴ� �ڷ�ƾ 
    {
        animator.SetTrigger("ChargeStart");//�ִϸ����Ϳ� ChargeStart ��� Ʈ���Ÿ� ����

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state3));// state3 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state3))// ���� �÷��� ���� �ִϸ��̼��� state3 ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        Vector3 startPos = transform.position;// ��¡ �ϱ��� ��¡ ������ġ�� ���� 

        Quaternion Rot = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z); // Y, Z ȸ������ �����ϰ� X ȸ���� 0���� ���� �ֳĸ� �����θ� ���⶧��

        transform.rotation = Rot;// ȸ������ ����


        //triggerDistance�̻��̵Ǹ� ���������µ� triggerDistance�� ��¡�Ÿ��� Vector3.Distance(transform.position, startPos)�̰Ŵ� ó��������ġ���� �Ÿ�
        while (Vector3.Distance(transform.position, startPos) < triggerDistance)
        {
            myPos = transform.position;
            float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f + fixedY; //���� ���帮�� �ִ� ��ġ�� Ʈ���� ���� ���� ���帮 yũ�� ���ݸ�ŭ ���̷� ���� + fixedY
            transform.position = new Vector3(myPos.x, terrainY, myPos.z);//�� ���̸� ���� 
            transform.position += transform.forward * chargeSpeed * Time.fixedDeltaTime;// �����ΰ����ڵ忡�� chargeSpeed ���� �ٲ㼭 ������ 
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(DieBurnduri());//�ٳ����� ���帮�� �׾���� �״� �ڷ�ƾ��
    }

    IEnumerator DieBurnduri()//���帮 ��¡���ϰ� ������ ���� �ڷ�ƾ 
    {
        myCollider.enabled = false;//�浹ü�� ���ش� 
        animator.SetTrigger("disappear");//�ִϸ����Ϳ� ����������� Ʈ���Ÿ� �ߵ���Ų��
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state5));// state5 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state5))// ���� �÷��� ���� �ִϸ��̼��� state5 ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        if (PhotonNetwork.IsMasterClient)//������ Ŭ���̾�Ʈ�� ��Ƽ�϶� �ٸ��ֵ鵵 �Ҽ� �ֱ䶧�� 
        {
            PoolManager.Instance.DespawnNetworked(gameObject); //PoolManager�� ��ȯ 
        }
    }


    public override void Move()
    {
        StartCoroutine(FreezeCor());//�ص��Ҷ� ���� �ڷ�ƾ
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction, isFreeze);
    }

    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)//���� ��ź�� ������ 
        {
            StopAllCoroutines();//����ڷ�ƾ�� ���� 
            ImFreeze = isFreeze;//������� ���� 
            IsFreeze.SetActive(true);//���� ������ Ű��
            animator.speed = 0f;//�ִϸ��̼� ����� ���� 
        }
        else if (isFreeze == false)//���� ���°� Ǯ���� 
        {
            ImFreeze = isFreeze;//������� ���� 
            StartCoroutine(FreezeCor());//�ص� �ڷ�ƾ�ߵ�
        }
        else
        {
            Debug.Log("���帮 ������ ���峲");
        }
    }

    IEnumerator FreezeCor()// ���帮 �ص��ϴ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(FreezeTime);//������� Ǯ������� �ɸ��� �ð� 
        animator.speed = 1f;//�ִϸ��̼� �ٽ���� 
        StartCoroutine(GoBurnduri());//�ٽ� ���帮 �ൿ�� ���ٲ��� 
        IsFreeze.SetActive(false);//���� �������� ���� 

    }


}
