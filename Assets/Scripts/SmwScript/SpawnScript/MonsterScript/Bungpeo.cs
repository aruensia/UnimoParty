//����
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Bungpeo : EnemyBase
{

    [SerializeField] float explosionForce = 20f;//���߽� ��ũ��
    [SerializeField] float explosionRadius = 20f;//���߽� ���� 
    [SerializeField] float upwardsModifier = 1f;//���߽������ִ���
    [SerializeField] LayerMask explosionMask;// ������ ���� ���� ���̾� 

    [SerializeField] GameObject[] Fragment;//������������� �����

    [SerializeField] GameObject explosionPartycle;//���� ��ƼŬ

    [SerializeField] GameObject[] Body;// ���߽� �ڽ� �� �޽���Ų�� ����

    Animator animator;//�ִϸ��̼� ����

    Collider myCollider;//�ڱ��浹ü �޴� ����
    string AppearAni = "anim_MON003_appear";// �ִϸ��̼� ������Ʈ �̸� 
    string explodeStateName = "anim_MON003_ready02";// �ִϸ��̼� ������Ʈ �̸� 

    [SerializeField] GameObject CrashBunpeo;// ������ ����� ���Ǵ� ��ƼŬ 

    Terrain terrain;//������

    int IsActivateFragment = 0;//������� ���������� üũ��

    [SerializeField] GameObject IsFreeze;//�������� 

    [SerializeField] float FreezeTime = 3;//�ص��ð� ����

    private void OnTriggerEnter(Collider other)// �浹
    {
        if (!photonView.IsMine) return;//��ü�� ���� ���� �ƴϸ� �Լ����� 
        if (other.CompareTag("Player"))//�浹ü ��ũ�� �÷��̾�� 
        {
            if (ImFreeze == true)//�����������¸� 
            {
                ImFreeze = false;//���� ���� 
                StartCoroutine(FreezeCor());//�ص� �ڷ�ƾ
            }
            else if (ImFreeze == false)//���� �������°� �ƴϴ� 
            {
                damage = 1;//�ϴ� ������ 1 
                Manager.Instance.observer.HitPlayer(damage);//�÷��̾�� ������ �ִ� ������

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(CrashBunpeo, hitPoint, rot);//������ �״� ��ƼŬ ����


                if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ�� �������ٲ��� (�ߺ� ���� ����)
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);
                }

            }
        }

        else if (other.CompareTag("Monster"))//��ũ�� ���͸� 
        {
            EnemyBase otherEnemy = other.GetComponent<EnemyBase>();//���͵��� EnemyBase �� �־ otherEnemy �� �޾��� 

            if (otherEnemy == null)//EnemyBase Ȥ�� ������ ����ؼ� ����������
            {
                Debug.Log("���� EnemyBase �� ����");
                return;
            }

            if (ImFreeze == false && otherEnemy.ImFreeze == true)//���� �������°� �ƴϰ� ��밡 �������¸� 
            {

                otherEnemy.ImFreeze = false;//��� �������� ���ְ� 
                otherEnemy.Move();//�ص�������

                Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
                Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
                Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
                Instantiate(CrashBunpeo, hitPoint, rot);//������ �״� ��ƼŬ ����


                if (PhotonNetwork.IsMasterClient)//������ Ŭ���̾�Ʈ�� 
                {
                    PoolManager.Instance.DespawnNetworked(gameObject);//���͸� ���� 
                }

            }
        }

        else if (other.CompareTag("Aube"))// �ױװ� �����ϰ�� (�ظ��ϸ� �������� �����ѵ� üũ)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);//�� ������Ʈ�� ���� ����� ������ ���
            Vector3 normal = (hitPoint - transform.position).normalized;// ���� �߽ɿ��� �浹 �������� ���ϴ� ���� ���͸� ���� ���ͷ� ��ȯ
            Quaternion rot = Quaternion.LookRotation(normal);// ȸ������ ����
            Instantiate(CrashBunpeo, hitPoint, rot);//������ �״� ��ƼŬ ����

            if (PhotonNetwork.IsMasterClient)//������ Ŭ���̾�Ʈ�� 
            {
                PoolManager.Instance.DespawnNetworked(gameObject);//�� ���� 
            }

        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();//�ڽ��� �ִϸ��̼� ���
        myCollider = GetComponent<Collider>();//�ڽ��� �浹ü ��� 


        terrain = Terrain.activeTerrain;//Ʈ���� �� �޾ƿ��� 

        float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f - 0.5f;// ���� ���� ��
        transform.position = new Vector3(transform.position.x, terrainY, transform.position.z);//���̰� ����

        for (int i = 0; i < Body.Length; i++)// �޽���Ų ���������� �ٽ�Ű�� 
        {
            Body[i].SetActive(true);
        }


        for (int i = 0; i < Fragment.Length; i++)//����� ���������� �ٽ� Ű��
        {
            Fragment[i].SetActive(true);
        }

        StartCoroutine(WaitAndExplode());//���� �غ� �ڷ�ƾ ���� 
    }

    private IEnumerator WaitAndExplode()//�����غ� �ڷ�ƾ 
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni));// AppearAni �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AppearAni))// ���� �÷��� ���� �ִϸ��̼��� AppearAni ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;//���� ���������� �ѱ��

        myCollider.enabled = true;

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(explodeStateName));// explodeStateName �ִϸ��̼��� ���۵� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)// ���� �÷��� ���� �ִϸ��̼��� explodeStateName ��� �̸��� ���� ��� �ݺ��϶�
            yield return null;//���� ���������� �ѱ��

        Explode();//����
    }


    public override void OnDisable()//��Ȱ��ȭ �� �˻� (����� Ǯ������ ���� ���� �ۿ� ���ϹǷ� �۵��� ����)
    {
        base.OnDisable();
        for (int i = 0; i < Fragment.Length; i++)
        {

            if (Fragment[i].GetComponent<Collider>() != null)//����� �浹ü üũ
            {
                Fragment[i].GetComponent<Collider>().enabled = false;//�浹ü�����ش� (���� �������� )
            }

            if (Fragment[i].GetComponent<Rigidbody>() != null)// ����� ������ٵ� üũ
            {
                Fragment[i].GetComponent<Rigidbody>().isKinematic = true;// ���� ������ ���� �ȹް���
                Fragment[i].GetComponent<Rigidbody>().useGravity = false;// �߷��� ����
            }
            Fragment[i].transform.localPosition = Vector3.zero;//�ڱ� ���� ��ġ�� �ٽõǵ���
            Fragment[i].SetActive(true);//������� �ٽ�����
        }
        for (int i = 0; i < Body.Length; i++)//��ü �޽� ��Ų ���ֱ� 
        {
            Body[i].SetActive(true);
        }
        myCollider.enabled = false;// ��ü �浹���ֱ�
    }

    public void Explode()// ���� �Լ� 
    {

        Vector3 explosionPosition = transform.position;

        for (int i = 0; i < Fragment.Length; i++)//����� ����ó��
        {

            if (Fragment[i].GetComponent<Collider>() != null)//�ִ��� üũ
            {
                Fragment[i].GetComponent<Collider>().enabled = true;//�浹ü ���ְ� 
            }

            if (Fragment[i].GetComponent<Rigidbody>() != null)//�ִ��� üũ
            {
                Fragment[i].GetComponent<Rigidbody>().isKinematic = false;//�������� ���� �ް��ϰ� 
                Fragment[i].GetComponent<Rigidbody>().useGravity = true;//�߷��� ����
            }
            Fragment[i].transform.localPosition = Vector3.zero;
        }

        //Physics.OverlapSphere �� ������ ��ġ(explosionPosition)�� ������(explosionRadius)**���� �̷���� �� �ȿ� ������ ��� Collider�� ��ȯ�մϴ�.
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, explosionMask);// ���߿� ���� �����͵��� ������

        foreach (Collider hit in colliders)//������ ã�� �� �浹ü(hit)�� ���� ó���ϴ°�
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();//�浹�� ��ü�� ������ �ٵ��� �޾ƿ�

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);// AddExplosionForce�� Rigidbody�� ���߷��� ���ϴ� �޼��� ���⼭ ��¡������ ������ ���� ��
            }
        }

        Instantiate(explosionPartycle, transform.position, Quaternion.identity);//��ü�� ���� �ؾ��ϹǷ� ��ƼŬ ����

        for (int i = 0; i < Body.Length; i++)// ��ü�� ������ ���� ���̸� �ȵǴϱ� �޽���Ų �̶� �浹ü ����
        {
            Body[i].SetActive(false);//��Ų �޽��ִµ� ��Ȱ��ȭ 
            myCollider.enabled = false;//�浹ü����
        }


    }


    [PunRPC]
    public void IsActivateRPC()//������ �� ���ŵǾ����� üũ�ϴ� �Լ�
    {
        //�̰� ���� ���� �Ȱ���
        IsActivateFragment++;
        if (IsActivateFragment == 4)//������ �����ŵǸ� 
        {

            IsActivateFragment = 0;//���ʱ�ȭ 


            if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ��
            {
                PoolManager.Instance.DespawnNetworked(gameObject);
            }

        }
    }

    public override void Move()
    {
        StartCoroutine(FreezeCor());//�ص���ƾ
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction, isFreeze);
    }


    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)// �������°� �Ƕ���ϸ� 
        {
            ImFreeze = isFreeze;//�ϴ� ���¸� ������
            animator.speed = 0f;//�ִϸ��̼��� �����
            IsFreeze.SetActive(true);//���� ���� �������� ȭ��ȭ 
        }
        else if (isFreeze == false)//���� ���� ����̿��� 
        {
            Debug.Log(isFreeze + " ������ ����");// �̰� �Ѿ� �����ϴ°ǰ�
            ImFreeze = isFreeze;//��������
            StartCoroutine(FreezeCor());//�ص��ڷ�ƾ
        }
        else
        {
            Debug.Log("���� ������ ���峲");
        }
    }
    IEnumerator FreezeCor()//�ص��ڷ�ƾ
    {
        yield return new WaitForSeconds(FreezeTime);//�ص��ϴ½ð�
        animator.speed = 1f;//�ִϸ��̼� ����
        IsFreeze.SetActive(false);//���� ������ ��Ȱ��ȭ
    }

}
