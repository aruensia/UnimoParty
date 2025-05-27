using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Burnduri : EnemyBase
{
    [Header("�÷��̾� ����Ʈ")]
    public List<Transform> players = new List<Transform>();//�÷��̾� ���⿡ �����

    Vector3 Target;
    Vector3 myPos;

    [Header("�̵� ����")]
    public float MoveSpeed = 1f;
    public float chargeSpeed = 10f;
    public float chargeDistance = 3f;

    [Header("�Ÿ� ����")]
    public float triggerDistance = 10f;
    public float fixedY = 0f;


    private bool isCharging = false;


    Terrain terrain;

    private Coroutine updateRoutine;
    private Coroutine moveRoutine;

    Transform nearestPlayer;

    int dashStateHash;


    Animator animator;
    Collider myCollider;

    // Inspector���� �Ҵ����� �ʾƵ�, ��Ÿ�ӿ� ��Ʈ�ѷ����� ã�Ƽ� �����մϴ�.
    private AnimationClip appearanceClip;
    private AnimationClip encounterClip;
    private AnimationClip disappearClip;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);
            //Manager.Instance.observer.HitPlayer(damage);
            StopAllCoroutines();
            StartCoroutine(DieBurnduri());
        }
    }

    //void OnEnable()
    //{
    //    animator = GetComponent<Animator>();
    //    myCollider = GetComponent<Collider>();

    //    myCollider.enabled = false;

    //    if (animator != null)
    //    {
    //        foreach (var clip in animator.runtimeAnimatorController.animationClips)
    //        {
    //            if (clip.name == "anim_01_MON001_Bduri_Appearance")
    //                appearanceClip = clip;
    //            else if (clip.name == "anim_03_MON001_Bduri_Encounter")
    //                encounterClip = clip;
    //            else if (clip.name == "anim_01_MON001_Bduri_Disappearance")
    //                disappearClip = clip;
    //        }
    //    }
    //    // ��Ʈ�ѷ��� ��ϵ� ��� Ŭ���� ������ ���ϴ� �̸��� Ŭ���� ����

    //    if (appearanceClip == null || encounterClip == null || disappearClip == null)
    //        Debug.LogWarning("Appearance �Ǵ� Encounter Ŭ���� ã�� ���߽��ϴ�.");

    //    //�ѹ��� ã������
    //    if (players.Count == 0)
    //    {
    //        var objs = GameObject.FindGameObjectsWithTag("Player");
    //        foreach (var obj in objs)
    //        {
    //            players.Add(obj.transform);
    //        }
    //    }

    //    terrain = Terrain.activeTerrain;
    //    isCharging = false;

    //    StartCoroutine(GoBurnduri());
    //}

    IEnumerator GoBurnduri()
    {
        float SpawnCool = appearanceClip != null ? appearanceClip.length / animator.speed : 0f;
        yield return new WaitForSeconds(SpawnCool);
        myCollider.enabled = true;
        updateRoutine = StartCoroutine(UpdateDistance());
        moveRoutine = StartCoroutine(MoveRoutine());
        yield return null;
    }


    void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator UpdateDistance()
    {
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
        float ChargeCool = encounterClip != null ? encounterClip.length / animator.speed : 0f;
        yield return new WaitForSeconds(ChargeCool);

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
        animator.SetTrigger("disappear");
        float DisappearCool = disappearClip != null ? disappearClip.length / animator.speed : 0f;
        myCollider.enabled = false;
        yield return new WaitForSeconds(DisappearCool);
        gameObject.SetActive(false);
    }



    public override void CsvEnemyInfo()
    {

    }

    public override void Move(Vector3 direction)
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();

        myCollider.enabled = false;

        if (animator != null)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "anim_01_MON001_Bduri_Appearance")
                    appearanceClip = clip;
                else if (clip.name == "anim_03_MON001_Bduri_Encounter")
                    encounterClip = clip;
                else if (clip.name == "anim_01_MON001_Bduri_Disappearance")
                    disappearClip = clip;
            }
        }
        // ��Ʈ�ѷ��� ��ϵ� ��� Ŭ���� ������ ���ϴ� �̸��� Ŭ���� ����

        if (appearanceClip == null || encounterClip == null || disappearClip == null)
            Debug.LogWarning("Appearance �Ǵ� Encounter Ŭ���� ã�� ���߽��ϴ�.");

        //�ѹ��� ã������
        if (players.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");
            foreach (var obj in objs)
            {
                players.Add(obj.transform);
            }
        }

        terrain = Terrain.activeTerrain;
        isCharging = false;

        StartCoroutine(GoBurnduri());
    }

}
