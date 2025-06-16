using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigbin : EnemyBase
{
    [HideInInspector] public GameObject prefab;

    [Header("�÷��̾� ����Ʈ")]
    public List<Transform> players = new List<Transform>();//�÷��̾� ���⿡ �����

    [Header("�̵� ����")]
    float MoveSpeed = 5f;
    float FirstSpeed;


    [Header("�Ÿ� ����")]
    [SerializeField] float fixedY = 0f;

    Terrain terrain;

    Collider myCollider;

    [SerializeField] GameObject CrashBigbin;

    [SerializeField] GameObject JumpParticles;

    [SerializeField] GameObject JumpExplode;

    Transform nearestPlayer;

    Vector3 myPos;
    Vector3 Target;

    Animator animator;


    string state2 = "anim_MON004_readytojump";
    string state3 = "anim_MON004_jump01";
    string state4 = "anim_MON004_jump02";
    string state5 = "anim_MON004_jump03";


    public override void OnEnable()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        terrain = Terrain.activeTerrain;
        FirstSpeed = MoveSpeed / 2;
        base.OnEnable();
        StartCoroutine(GoBigBin());

    }


    public override void OnDisable()
    {
        base.OnDisable();
        MoveSpeed = FirstSpeed * 2;
    }


    IEnumerator GoBigBin()
    {

        myCollider.enabled = true;

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state2));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state2))
            yield return null;
        JumpParticles.SetActive(true);  
        MoveSpeed += FirstSpeed;
        Debug.Log($"state2 exit �� MoveSpeed={MoveSpeed}");

        StartCoroutine(MoveRoutine());
        StartCoroutine(UpdateDistance());

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state3));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state3))
            yield return null;
        JumpParticles.SetActive(true);

        MoveSpeed += FirstSpeed;
        Debug.Log($"state3 exit �� MoveSpeed={MoveSpeed}");




        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state4));
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(state4))
            yield return null;
        JumpParticles.SetActive(true);

        MoveSpeed += FirstSpeed;
        Debug.Log($"state4 exit �� MoveSpeed={MoveSpeed}");



        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(state5));

        var infos = animator.GetCurrentAnimatorClipInfo(0);
        if (infos.Length > 0)
        {
            var playingClip = infos[0].clip;
            float duration = playingClip.length / animator.GetCurrentAnimatorStateInfo(0).speed;
            yield return new WaitForSeconds(duration);
        }
        else
        {
            Debug.LogWarning("���� ��� ���� Ŭ���� ã�� ���߽��ϴ�.");
        }
        Instantiate(JumpExplode, transform.position, Quaternion.identity);
        StopAllCoroutines();
        PoolManager.Instance.Despawn(prefab, gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Manager.Instance.observer.HitPlayer(damage);
            Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);

            Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������

            Vector3 normal = (hitPoint - transform.position).normalized;// ������
            Quaternion rot = Quaternion.LookRotation(normal);// ������

            GameObject inst = Instantiate(CrashBigbin, hitPoint, rot);


            PoolManager.Instance.Despawn(prefab, gameObject);
        }

    }

    IEnumerator MoveRoutine()
    {

        while (true)
        {
            float CheckNear = Vector3.Distance(myPos, Target);

            myPos = transform.position;
            float terrainY = terrain.SampleHeight(transform.position) + fixedY;
            transform.position = new Vector3(myPos.x, terrainY, myPos.z);

            transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator UpdateDistance()
    {
        if (players.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");
            foreach (var obj in objs)
            {
                players.Add(obj.transform);
            }
        }
        while (true)
        {
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
                float Distance = Vector3.Distance(myPos, player.position);


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
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }


            yield return new WaitForSeconds(0.5f);
        }
    }

    
    public override void Move(Vector3 direction)
    {
        photonView.RPC("MoveRPC", RpcTarget.All, direction);
    }

    [PunRPC]
    public void MoveRPC(Vector3 direction)
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        terrain = Terrain.activeTerrain;
        FirstSpeed = MoveSpeed / 2;
        base.OnEnable();
        StartCoroutine(GoBigBin());
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction);
    }

    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {
        if (isFreeze == true)
        {
            StopAllCoroutines();
        }
        else if (isFreeze == false)
        {
            Move(direction);
        }
        else
        {
            Debug.Log("��� ������ ���峲");
        }
    }

    public override void CsvEnemyInfo()
    {
        throw new System.NotImplementedException();
    }
}
