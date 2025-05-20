using UnityEngine;
using System.Collections;

public class ShookShook : MonoBehaviour
{
    [HideInInspector] public CornerSpawner spawner;

    public float moveSpeed = 10f;
    public float fixedY = 0.5f; // ���� ���� ������

    private Vector3 targetPos;

    public void Init(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;
        targetPos = endPos;
        StopAllCoroutines();
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = dir;

        // ���(xz) �Ÿ��� üũ
        while (Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetPos.x, targetPos.z)
        ) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Terrain ���� ����
            Vector3 temp = transform.position;
            if (Terrain.activeTerrain)
                temp.y = Terrain.activeTerrain.SampleHeight(temp) + fixedY;
            else
                temp.y = fixedY;
            transform.position = temp;

            yield return null;
        }
        // �������� ��Ȯ�� ��ġ ����
        transform.position = targetPos;
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        if (spawner != null)
            spawner.OnEnemyRemoved();
    }
}
