using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllSpawn : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] float spawnTimer = 3f;
    [SerializeField] int poolSize = 20;
    [SerializeField] int minSpawn = 1;      // �� ���� �ּ� ���� ��
    [SerializeField] int maxSpawn = 4;      // �� ���� �ִ� ���� �� (n ��)


    [Header("�ڽ� ����")]
    [SerializeField] float rectWidth = 30f;
    [SerializeField] float rectHeight = 30f;

    [Header("���� ����(�߾� ����)")]
    [SerializeField] float innerRadius = 5f; // �߽ɿ��� �� �Ÿ� �̳��� ����X

    [Header("�����")]
    [SerializeField] bool showGizmos = true;

    private List<GameObject> enemyPool = new List<GameObject>();
    private bool isQuitting = false;

    // ���� ��ġ ��Ͽ� (����� ǥ�ÿ�)
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        // Ǯ �̸� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.one * 9999, Quaternion.identity);
            obj.SetActive(false);
            enemyPool.Add(obj);
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int activeCount = GetActiveEnemyCount();
            int spawnable = maxEnemies - activeCount;
            if (spawnable > 0)
            {
                int wantToSpawn = Random.Range(minSpawn, maxSpawn + 1);

                // ��Ȱ��ȭ�� ���� ��
                int available = 0;
                foreach (var obj in enemyPool)
                    if (!obj.activeInHierarchy) available++;

                int toSpawn = Mathf.Min(wantToSpawn, spawnable, available);

                for (int i = 0; i < toSpawn; i++)
                {
                    SpawnOneEnemy();
                }
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    void SpawnOneEnemy()
    {
        GameObject enemy = GetPooledEnemy();
        if (enemy == null) return; // Ǯ�� ���� ����

        Vector3 spawnPos = GetBoxDonutSpawnPosition();
        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);

        spawnPositions.Add(spawnPos);
    }

    GameObject GetPooledEnemy()
    {
        foreach (var obj in enemyPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null; // ��� �����
    }

    int GetActiveEnemyCount()
    {
        int count = 0;
        foreach (var obj in enemyPool)
        {
            if (obj.activeInHierarchy)
                count++;
        }
        return count;
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    // �簢�� ����: �߽ɿ��� innerRadius �̳��� ����
    Vector3 GetBoxDonutSpawnPosition()
    {
        Vector3 center = transform.position;
        float halfWidth = rectWidth / 2f;
        float halfHeight = rectHeight / 2f;
        Vector3 pos;

        int safety = 0;
        do
        {
            float x = Random.Range(center.x - halfWidth, center.x + halfWidth);
            float z = Random.Range(center.z - halfHeight, center.z + halfHeight);
            pos = new Vector3(x, 0, z);
            if (++safety > 20) break;
        } while (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(center.x, center.z)) < innerRadius);

        return pos;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        // �ڽ� �ܰ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(rectWidth, 0.1f, rectHeight));

        // ���� ���� (�߾�)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);

        // ���� ��ġ(���)
        Gizmos.color = Color.yellow;
        foreach (var pos in spawnPositions)
        {
            Gizmos.DrawSphere(pos + Vector3.up * 0.2f, 0.5f);
        }
    }
}
