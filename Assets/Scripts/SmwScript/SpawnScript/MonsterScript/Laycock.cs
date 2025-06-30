//������
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laycock : EnemyBase
{

    public List<Transform> players = new List<Transform>();//�÷��̾� ���⿡ �����

    Vector3 myPos;// �ڽ��� ���� ��ġ 
    Vector3 Target;//����� �÷��̾� ��ġ 

    Transform nearestPlayer;//���ϰ���� �÷��̾� Transform �ޱ�

    [SerializeField] ParticleSystem ChargeParticles;//������ �߻��� ���� ��ƼŬ 
    [SerializeField] ParticleSystem ShootParticles;//������ �߻�Ǵ� ���� ������ ��ƼŬ(��)
    [SerializeField] GameObject DieParticles;//������ �״� ��ƼŬ

    Terrain terrain;//������ ���� ��������

    float LazerLoopTime = 3;//��������� �ش� �ʰ� ��޸��� ��

    Animator animator;//�ڽ� �ִϸ����� �޴º���

    Collider myCollider;//�ڱ��浹ü �޴º���
    string AppearAni = "anim_MON006_Appear";// �ָ��̼� ������Ʈ �̸�

    [SerializeField] float FreezeTime = 3;//�ص� �ð�

    [SerializeField] GameObject IsFreeze;//���� ���� ������

    private Coroutine lazerCoroutine;//������ �ڷ�ƾ ó���� ���� �ڷ�ƾ������ �����

    LaycockSP laycockSP;//������ �����ʶ����� (Ư����Ȳ������ �������� ����)

    private void OnTriggerEnter(Collider other)//�浹ü ó��
    {
        if (!photonView.IsMine) return;//��ü�� ���� ���� �ƴϸ� �Լ����� 
        if (other.CompareTag("Player"))//�ױװ� �÷��̾� �ϰ�� 
        {
            if (ImFreeze == true)// ���� ���������̸�
            {
                ImFreeze = false;//�������� ����
                StartCoroutine(FreezeCor());// �ص��ڷ�ƾ
            }
            else if (ImFreeze == false)
            {
                damage = 1;//������ 1
                Manager.Instance.observer.HitPlayer(damage);//�÷��̾�� �������� ��

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(DieParticles, hitPoint, rot);//��� ��ƼŬ

                laycockSP.DisCountLaycock();//���ڰ� �پ��ٴ°� �˷���


                if (PhotonNetwork.IsMasterClient)//������ Ŭ���̾�Ʈ��
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }

            }
        }

        else if (other.CompareTag("Monster"))//�ױ� ���� �浹
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
                Instantiate(DieParticles, hitPoint, rot);// ���帮 ��� ��ƼŬ ���� 
                if (PhotonNetwork.IsMasterClient)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);//���ڽ��� PoolManager�� ��ȯ
                }

            }
        }

        else if (other.CompareTag("Aube"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
            Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
            Instantiate(DieParticles, hitPoint, rot);// ���帮 ��� ��ƼŬ ���� 
            if (PhotonNetwork.IsMasterClient)
            {
                PoolManager.Instance.DespawnNetworked(gameObject);//���ڽ��� PoolManager�� ��ȯ
            }

        }

    }


    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Distance());//�ൿ����
    }


    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();//����ڷ�ƾ ����
        myCollider.enabled = false;//�浹ü ���� 
    }

    IEnumerator Distance()
    {
        animator = GetComponent<Animator>();//�ڽ� �ִϸ��̼� ��� 
        myCollider = GetComponent<Collider>();//�ڽ� �浹ü ���
        terrain = Terrain.activeTerrain;//�������� ��� 
        laycockSP = FindObjectOfType<LaycockSP>();//������ ���� ��� (����)

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni));// AppearAni �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni))// ���� �÷��� ���� �ִϸ��̼��� AppearAni ��� �̸��� ���� ��� �ݺ��϶�
            yield return null; // �� �����Ӹ� �����ٰ� ���� �����ӿ� �ٽ� �̾ �����ϰ���

        myCollider.enabled = true;//�浹ü Ű�� 

        float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f;//���̰� ��� 
        transform.position = new Vector3(transform.position.x, terrainY, transform.position.z);//���̰� ����
        if (players.Count == 0)//�÷��̾ ������ 
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");//�÷��̾� ��ũ�޸��ֵ� ���� ã�� 
            foreach (var obj in objs)//�÷��̾� ���ڸ�ŭ
            {
                players.Add(obj.transform);//����Ʈ�� ��� 
            }
        }
        myPos = transform.position;// �ڽ���ġ�� �־���
        float minDistance = Mathf.Infinity;// �ϴ� ���� ū�������� 

        foreach (var player in players)
        {
            float Distance = Vector3.Distance(myPos, player.position);//�÷��̾���� �Ÿ���� 


            if (Distance < minDistance)//���� ���� ������� ��
            {
                minDistance = Distance;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null)
        {
            Target = nearestPlayer.position;//Ÿ���� ���� �����ַ��� 

            transform.LookAt(Target);//�����ָ� �ٶ�
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//Ȥ�� �� ���������°Ź��� 
        }
        yield return null;
    }
    [PunRPC]
    public void ShootLazer()// ������ ��� ���� �ϴ� �Լ� 
    {
        //�̹� ���� ���� Lazer �ڷ�ƾ�� ������ ����
        if (lazerCoroutine != null)
        {
            StopCoroutine(lazerCoroutine);
            lazerCoroutine = null;
        }
        lazerCoroutine = StartCoroutine(Lazer());//�������� �� (�ڷ�ƾ)
    }


    IEnumerator Lazer()
    {

        yield return new WaitForSeconds(3f);//3�ʰ� ��޷ȴٰ� 
        ChargeParticles.gameObject.SetActive(true);//�⸦ ���� ��ƼŬ Ȱ��ȭ 

        yield return new WaitUntil(() => !ChargeParticles.IsAlive(true));//�⸦ ������ ��ƼŬ�� ������ 

        animator.SetBool("action", true);//�ִϸ��̼� bool action ����
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("anim_MON006_ShootStart") && !animator.IsInTransition(0)); //anim_MON006_ShootStart�ִϸ��̼� �������� üũ
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);//anim_MON006_ShootStart�ִϸ��̼��� �������� üũ

        yield return StartCoroutine(LoopLazer());

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("anim_MON006_Disappear") && !animator.IsInTransition(0));//anim_MON006_Disappear�ִϸ��̼� �������� üũ
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);//anim_MON006_Disappear�ִϸ��̼��� �������� üũ

        lazerCoroutine = null;//�ڷ�ƾ �ʱ�ȭ 

        if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ��
        {
            PoolManager.Instance.DespawnNetworked(gameObject);//������ 
        }

    }

    IEnumerator LoopLazer()//������ �߻����� �ڷ�ƾ
    {
        ShootParticles.gameObject.SetActive(true);//��¥������ Ȱ��ȭ 
        yield return new WaitForSeconds(LazerLoopTime);// ������ �üӽð� 
        ShootParticles.gameObject.SetActive(false);//������ ���� 

        animator.SetTrigger("disappear");//������ ������� �ִϸ��̼� �ߵ�
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)//������¶�� �ϸ� 
        {
            ImFreeze = isFreeze;//���� ����
            animator.speed = 0f;//�ִϸ��̼� ����
            IsFreeze.SetActive(true);//���� ������ Ȱ��ȭ
        }
        else if (isFreeze == false)//���� ���� ������ 
        {
            Debug.Log(isFreeze + " ������ ����");// �̰� �Ѿ� �����ϴ°ǰ�
            animator.SetTrigger("Freeze");//�������� �ʿ��� �ִϸ��̼�ó��
            ImFreeze = isFreeze;//������� ���� 
            StartCoroutine(FreezeCor());//�ص� �ڷ�ƾ
        }
        else
        {
            Debug.Log("������ ������ ���峲");
        }
    }

    public override void Move()//�ܺο��� �ص� 
    {
        StartCoroutine(FreezeCor());//�ص� �ڷ�ƾ����
    }

    IEnumerator FreezeCor()//�ص��ڷ�ƾ 
    {
        yield return new WaitForSeconds(FreezeTime);//�ص��ð�
        animator.speed = 1f;//�ִϸ��̼� ���� 
        IsFreeze.SetActive(false);//���� ������ ���� 
    }


}
