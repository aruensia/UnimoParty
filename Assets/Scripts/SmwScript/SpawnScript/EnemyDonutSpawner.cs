using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDonutSpawner : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] GameObject enemyPrefab;//�� ������
    [SerializeField] int maxEnemies = 10; //�� �ִ���� 
    [SerializeField] float spawnTimer = 20f; //�� ���� �����ð� 

    [Header("���� ����")]
    [SerializeField] float innerRadius = 5f; //�߾� �������� ����
    [SerializeField] float outerRadius = 20f;//��ü �������� 

    [Header("�����")]
    [SerializeField] bool showGizmos = true;// �̰Ŵ� ����� �����ٲ��� ������ ����������

    private int currentEnemyCount = 0; // �̰Ŵ� �� ������ ���ִ� ��
    private List<Vector3> spawnPositions = new List<Vector3>();//�̰Ŵ� ������ ó�� ������ ��� �ߴ��� ������ ����� 

    void Start()
    {
        // ���ʿ� maxEnemies��ŭ ����
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnOneEnemy();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    // �� 1���� ���� �Լ�
    void SpawnOneEnemy()
    {
        Vector3 spawnPos = GetDonutSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        spawnPositions.Add(spawnPos);
        currentEnemyCount++;

        // ������ ������ ���� ����
        pupu enemyScript = enemy.GetComponent<pupu>();
        if (enemyScript != null)
            enemyScript.spawner = this;
    }

    // ���� ����� �� ȣ��Ǵ� �Լ�
    public void OnEnemyRemoved()
    {
        currentEnemyCount--;

        if (isQuitting) return; // �������̸� �� �̻� ����X

        if (currentEnemyCount < maxEnemies)
            SpawnOneEnemy();
    }

    private bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    // ���� ���� ���� ��ġ ��ȯ
    Vector3 GetDonutSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(innerRadius, outerRadius);
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        Vector3 center = transform.position;
        return new Vector3(center.x + x, 0, center.z + z); 
    }

    // ���� ���� + ���� ���� ����� ǥ��
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        // ���� �ܰ���
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        // ���� ����(����)
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, innerRadius);

        // ������ ����
        Gizmos.color = Color.white;
        foreach (var pos in spawnPositions)
        {
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }
}
