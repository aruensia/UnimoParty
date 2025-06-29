//���
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigbin : EnemyBase
{

    [Header("�÷��̾� ����Ʈ")]
    public List<Transform> players = new List<Transform>();//�÷��̾� ���⿡ �����

    [Header("�̵� ����")]
    float MoveSpeed = 5f;//�����ӵ�
    float FirstSpeed; //ó���ӵ��� ������ ����


    [Header("�Ÿ� ����")]
    [SerializeField] float fixedY = 0f;//��� ���̸� ������ ���� 

    Terrain terrain; //Ʈ������ ���� ���� 

    Collider myCollider;//�ڱ��ڽ��� �浿ü������ ����

    [SerializeField] GameObject CrashBigbin;// ��� ������ ����ƼŬ

    [SerializeField] GameObject JumpParticles;//�����ϰ� ���� ���������� ���� ��ƼŬ

    [SerializeField] GameObject JumpExplode;//���� ����Ǹ� ����� ������� ��ƼŬ

    Transform nearestPlayer; // ���� ����� �÷��̾� Transform�� ������ ����

    Vector3 myPos;// ����� ���� ��ġ�� ������ ����
    Vector3 Target;// ���� ����� �÷��̾� ��ġ�� ������ ����

    Animator animator;//����� �ִϸ����͸� ���� ����

    [SerializeField] float FreezeTime = 3; //�ص��Ҷ� �ɸ��½ð� 

    //�ִϸ��̼� ������Ʈ �̸���
    string AppearAni = "anim_MON004_appear";
    string state2 = "anim_MON004_readytojump";
    string state3 = "anim_MON004_jump01";
    string state4 = "anim_MON004_jump02";
    string state5 = "anim_MON004_jump03";

    [SerializeField] GameObject IsFreeze;//���� ������

    float MoveSpeedSave;//���� �̵��ӵ������� ����

    public override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();// animator�� �ִϸ����͸� ����
        myCollider = GetComponent<Collider>();//myCollider�� �ڽ��� �浹ü������
        terrain = Terrain.activeTerrain;//terrain�� Ʈ���������� ����
        FirstSpeed = MoveSpeed / 2;//���� �̵��ӵ��� 0.5��ŭ ������ų���⶧����
        StartCoroutine(GoBigBin());//GoBigBin() �ڷ�ƾ���� 
    }


    public override void OnDisable()
    {
        base.OnDisable();
        MoveSpeed = FirstSpeed * 2;//���� �ٽ� �ʱ�ȭ����
        myCollider.enabled = false;//�浹ü �ٽò���
    }


    IEnumerator GoBigBin()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni));// AppearAni �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni))// ���� �÷��� ���� �ִϸ��̼��� AppearAni ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        myCollider.enabled = true;//�浹ü�� �ٽ� Ű�� 

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state2));//state2 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state2))// ���� �÷��� ���� �ִϸ��̼��� state2 ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        JumpParticles.SetActive(true);//���� ��ƼŬ Ȱ��ȭ 
        MoveSpeed += FirstSpeed;//�ӵ� ����


        StartCoroutine(MoveRoutine());//������ ���� �ڷ�ƾ ����
        StartCoroutine(UpdateDistance());//������ ����ִ� �ڷ�ƾ ����

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state3));//state3 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state3))// ���� �÷��� ���� �ִϸ��̼��� state3 ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        JumpParticles.SetActive(true);//���� ��ƼŬ Ȱ��ȭ 
        MoveSpeed += FirstSpeed;//�ӵ� ����


        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state4));//state4 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state4))// ���� �÷��� ���� �ִϸ��̼��� state4 ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        JumpParticles.SetActive(true);//���� ��ƼŬ Ȱ��ȭ 
        MoveSpeed += FirstSpeed;//�ӵ� ����

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state5));//state5 �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)// ���� �÷��� ���� �ִϸ��̼��� ������ ����ɶ����� ��� �ݺ��϶�
            yield return null;// �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        Instantiate(JumpExplode, transform.position, Quaternion.identity);//�������ϰ� �������� ����� ������� ��ƼŬ����
        StopAllCoroutines();//����ڷ�ƾ ���߰��ϱ�

        if (PhotonNetwork.IsMasterClient)
        {
            PoolManager.Instance.DespawnNetworked(gameObject);//PoolManager�� �ڽ��� ��ȯ
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;//��ü�� ���� ���� �ƴϸ� �Լ����� 
        if (other.CompareTag("Player"))//�ش簴ü�� �÷��̾��ϰ�� 
        {
            if (ImFreeze == true)//���� ����������̸�
            {
                ImFreeze = false;//��������� ���� 
                StartCoroutine(FreezeCor());//�ص� �ڷ�ƾ �ߵ� 
            }
            else if (ImFreeze == false)//���� ������ ���°� �ƴϸ� 
            {
                damage = 1;//���Ƿ� �������� 1�� ��
                Manager.Instance.observer.HitPlayer(damage);//�÷��̾�� ���������ذ�

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������
                Instantiate(CrashBigbin, hitPoint, rot);//����� ������ ��ƼŬ ���� 

                if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ��
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);//PoolManager�� ��ȯ�Ҳ���
                }

            }
        }

        else if (other.CompareTag("Monster"))//�ױװ� ���Ͷ�� 
        {
            EnemyBase otherEnemy = other.GetComponent<EnemyBase>();//Monster �������� EnemyBase��ũ��Ʈ�� �����Ƿ� ��������

            if (otherEnemy == null)//����ȉ����� 
            {
                Debug.Log("���� EnemyBase �� ����");
                return;
            }

            if (ImFreeze == false && otherEnemy.ImFreeze == true)//���� ����� �ƴѵ� ��� ���Ͱ� �������ϰ�� 
            {

                otherEnemy.ImFreeze = false;//���� ������� Ǯ���ش� 
                otherEnemy.Move();//��� �ص����·� ������ش� 

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������
                Instantiate(CrashBigbin, hitPoint, rot);//����� ������ ��ƼŬ ���� 
                if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ��
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }
            }
        }


        else if (other.CompareTag("Aube"))//�浹�Ȱ� �����̸� 
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������
            Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ȸ������
            Instantiate(CrashBigbin, hitPoint, rot);//����� ������ ��ƼŬ ���� 

            if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ��
            {
                PoolManager.Instance.DespawnNetworked(gameObject);
            }

        }

    }

    IEnumerator MoveRoutine()//�����θ� �����̴� �ڷ�ƾ
    {

        while (true)
        {
            myPos = transform.position;//���� ���� �ʿ��Ѻ���
            float terrainY = terrain.SampleHeight(transform.position) + fixedY;//������ ����
            transform.position = new Vector3(myPos.x, terrainY, myPos.z);//��������

            transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;//�����θ� �̵�

            yield return new WaitForFixedUpdate();//�ֱ�������
        }
    }

    IEnumerator UpdateDistance()
    {
        if (players.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");//Player ����ױװ� �ֵ� ���� ã��
            foreach (var obj in objs)
            {
                players.Add(obj.transform);//�߰��Ҷ� �� ����Ʈ�� �߰�����
            }
        }
        while (true)
        {
            for (int i = players.Count - 1; i >= 0; i--)
            {
                Transform p = players[i];

                if (p == null || !p.gameObject.activeInHierarchy)//���� �÷��̾��Ʈ�� �ش��÷��̾ ���ų� ���̷�Űâ���� ������ 
                {
                    players.RemoveAt(i);//����Ʈ���� ������
                }
            }

            myPos = transform.position;//Ȥ�ø� �ʱ�ȭ 
            float minDistance = Mathf.Infinity;// �ϴ� ���� ū�������� 

            foreach (var player in players)
            {
                float Distance = Vector3.Distance(myPos, player.position);//�÷��̾� �Ÿ��� ���ƿ�


                if (Distance < minDistance)//���������� ������ �ٲ��ִ°� 
                {
                    minDistance = Distance;
                    nearestPlayer = player;// ���� �߰��� �÷��̾� nearestPlayer�� �����
                }
            }

            if (nearestPlayer != null)
            {
                Target = nearestPlayer.position;//Ÿ���� ���ϰ��� �̱⶧���� Target�� �����

                transform.LookAt(Target);//Ÿ���� �Ĵٺ�
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);// �ѹ��Ĵٺ��µ� y���������� �ٸ����� ���������ʰ� 
            }


            yield return new WaitForSeconds(0.5f);//0.5���� ���� ã��
        }
    }


    public override void Move()//�ٸ��뿡�� ����� �ص��Ǵ°� ���� �Լ� 
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
            ImFreeze = isFreeze;
            IsFreeze.SetActive(true);
            MoveSpeedSave = MoveSpeed;
            MoveSpeed = 0;
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
            Debug.Log("��� ������ ���峲");
        }
    }

    IEnumerator FreezeCor()
    {
        yield return new WaitForSeconds(FreezeTime);
        MoveSpeed = MoveSpeedSave;
        animator.speed = 1f;
        IsFreeze.SetActive(false);
    }

}
