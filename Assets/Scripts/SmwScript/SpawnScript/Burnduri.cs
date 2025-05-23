using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Burnduri : EnemyBase
{
    [Header("�÷��̾� ����Ʈ")]
    public List<Transform> players = new List<Transform>(); // �ν����Ϳ� ���!

    Vector3 Target;

    [Header("�̵� ����")]
    public float MoveSpeed = 1f;
    public float chargeSpeed = 10f;
    public float chargeDistance = 3f;

    [Header("�Ÿ� ����")]
    public float triggerDistance = 10f;
    public float fixedY = 0f;

    private Transform currentTarget;
    private bool isCharging = false;


    Terrain terrain;

    private Coroutine updateRoutine;
    private Coroutine moveRoutine;

    private void Awake()
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

    public override void CsvEnemyInfo()
    {

    }

    public override void Move(Vector3 direction)
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);
    //        Manager.Instance.observer.HitPlayer(damage);
    //    }
    //}

    void OnEnable()
    {
        currentTarget = transform;
        terrain = Terrain.activeTerrain;
        isCharging = false;
        updateRoutine = StartCoroutine(UpdateDistance());
        moveRoutine = StartCoroutine(MoveRoutine());
    }


    void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator UpdateDistance()
    {
        while (true)
        {
            players.RemoveAll(p => p == null || !p.gameObject.activeInHierarchy);

            Vector3 myPos = transform.position;
            Transform nearestPlayer = null;
            float minDistance = Mathf.Infinity;// �ϴ� ���� ū��


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
                currentTarget = nearestPlayer;
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
                Vector3 myPos = transform.position;
                float CheckNear = Vector3.Distance(myPos, Target);

                if (CheckNear < chargeDistance)
                {
                    isCharging = true;
                    StopCoroutine(updateRoutine);
                    StopCoroutine(moveRoutine);
                    StartCoroutine(ChargeRoutine());
                    yield break;
                }
                else
                {
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
        StopCoroutine(updateRoutine);
        StopCoroutine(moveRoutine);

        yield return new WaitForSeconds(1f);

        Vector3 startPos = transform.position;
        Vector3 dir = transform.forward;  // �� ������ ������ ����

        while (Vector3.Distance(transform.position, startPos) < triggerDistance)
        {
            float terrainY = terrain.SampleHeight(transform.position);
            terrainY += transform.localScale.y / 2f;
            dir.y = terrainY + fixedY;
            transform.position += dir * chargeSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        enabled = false;
        Destroy(gameObject);
    }

}
