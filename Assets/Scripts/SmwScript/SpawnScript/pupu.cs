using UnityEngine;
using System.Collections;

public class Pupu : MonoBehaviour
{
    // ���� �߽��� �׻� (0,0,0)���� ����
    Vector3 center = Vector3.zero;

    // Inspector���� ������ �� �ִ� ��
    [Header("�� ������(�Ÿ�)")]
    public float radius = 5f;

    [Header("�ʱ� Y����")]
    public float fixedY = 0f;

    [Header("�ʴ� ���ѷ� �̵��ӵ�(m/s)")]
    public float moveSpeed = 2f;

    float angle;           // ���� ����(����)
    int rotateDirection;   // 1�̸� �ݽð�, -1�̸� �ð�

    Coroutine rotateCoroutine;

    void Start()
    {
        // 1. ���� �������� ����
        angle = Random.Range(0f, Mathf.PI * 2f);

        // 2. ���� ȸ�� ����(1 or -1)
        rotateDirection = Random.value < 0.5f ? 1 : -1;

        // 3. ���� ��ġ�� �� ���� ���缭 �̵�
        SetPositionOnCircle();

        // 4. ���� ���� ���� �ڷ�ƾ ����
        rotateCoroutine = StartCoroutine(RotateOnCircle());
    }

    void SetPositionOnCircle()
    {
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(center.x + x, fixedY, center.z + z);
    }

    IEnumerator RotateOnCircle()
    {
        while (true)
        {
            // 1. �� �ѷ��� ���� �ӵ� -> ���ӵ��� ��ȯ
            float angularSpeed = moveSpeed / radius; // ����/��

            // 2. ������ ȸ�� ���⿡ ���� �ٲ���
            angle -= angularSpeed * Time.deltaTime * rotateDirection;

            // 3. ��ġ ����ؼ� �̵�
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            transform.position = new Vector3(center.x + x, fixedY, center.z + z);

            yield return new WaitForFixedUpdate();
        }
    }

    // ������Ʈ�� ���� �� �ڷ�ƾ ����
    void OnDisable()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = null;
    }





    // (����) ���� Scene�� �׷��ִ� ���
    void OnDrawGizmos()
    {
        float drawRadius = radius > 0.01f ? radius : 2.0f;
        int segments = 60;
        float theta = 0f;
        float deltaTheta = (2f * Mathf.PI) / segments;
        float y = Application.isPlaying ? transform.position.y : center.y;

        Vector3 oldPos = center + new Vector3(Mathf.Cos(0f) * drawRadius, y, Mathf.Sin(0f) * drawRadius);

        Gizmos.color = Color.white;
        for (int i = 1; i <= segments; i++)
        {
            theta += deltaTheta;
            Vector3 newPos = center + new Vector3(Mathf.Cos(theta) * drawRadius, y, Mathf.Sin(theta) * drawRadius);
            Gizmos.DrawLine(oldPos, newPos);
            oldPos = newPos;
        }
    }
}
