using System.Collections;
using UnityEngine;

public class Burnduri : MonoBehaviour
{
    [Header("�̵� ����")]
    public float normalSpeed = 10f;       // �⺻ �̵��ӵ�
    public float chargeSpeed = 60f;       // ���� �� �̵��ӵ�
    public float chargeDistance = 6f;    // ���� �Ÿ�

    [Header("�Ÿ� ����")]
    public float triggerDistance = 3f;   // ���� �ߵ� �Ÿ�

    public float fixedY = 0f;            // ���� ���� ����

    private Transform player;
    private bool isCharging = false;

    void Start()
    {
        // �÷��̾� ã�� (Player �±�)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (player == null)
            {
                // ���� �÷��̾ ������ ��� Ž��
                player = GameObject.FindGameObjectWithTag("Player")?.transform;
                yield return null;
                continue;
            }

            if (!isCharging)
            {
                // �÷��̾� ����(���� ����)
                Vector3 target = player.position;
                target.y = 0;
                Vector3 pos = transform.position;
                pos.y = 0;
                Vector3 dir = (target - pos).normalized;

                // �ٶ󺸱�
                if (dir != Vector3.zero)
                    transform.forward = dir;

                // �̵�
                transform.Translate(Vector3.forward * normalSpeed * Time.deltaTime);

                // ���� ���� (Terrain ������ ���� ����)
                Vector3 temp = transform.position;
                if (Terrain.activeTerrain)
                    temp.y = Terrain.activeTerrain.SampleHeight(temp) + fixedY;
                else
                    temp.y = fixedY;
                transform.position = temp;

                // �÷��̾���� �Ÿ� üũ
                float dist = Vector3.Distance(transform.position, player.position);
                if (dist <= triggerDistance)
                {
                    // ���� �ڷ�ƾ ����
                    StartCoroutine(ChargeRoutine());
                    isCharging = true;
                    yield break; // �̵� �ڷ�ƾ ����(���� ������ ��Ȱ��ȭ)
                }
            }

            yield return null;
        }
    }

    IEnumerator ChargeRoutine()
    {
        // ���� ���� = ���� ������ �÷��̾� ����
        Vector3 start = transform.position;
        Vector3 toPlayer = (player.position - start);
        toPlayer.y = 0;
        Vector3 dir = toPlayer.normalized;

        float moved = 0f;
        while (moved < chargeDistance)
        {
            float step = chargeSpeed * Time.deltaTime;
            transform.position += dir * step;
            moved += step;

            // ���� ����
            Vector3 temp = transform.position;
            if (Terrain.activeTerrain)
                temp.y = Terrain.activeTerrain.SampleHeight(temp) + fixedY;
            else
                temp.y = fixedY;
            transform.position = temp;

            yield return null;
        }

        // ���� ��, ������Ʈ ��Ȱ��ȭ!
        gameObject.SetActive(false);
    }
}
