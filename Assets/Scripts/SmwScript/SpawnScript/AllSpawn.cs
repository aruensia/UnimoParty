using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllSpawn : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] float spawnInterval = 3f;

    [Header("�ڽ� ����")]
    [SerializeField] float rectWidth = 30f;
    [SerializeField] float rectHeight = 30f;

    [Header("���� ����(�߾� ����)")]
    [SerializeField] float innerRadius = 5f; // �߽ɿ��� �� �Ÿ� �̳��� ����X

    [Header("�����")]
    [SerializeField] bool showGizmos = true;

    private int currentEnemyCount = 0;
    private bool isQuitting = false;

    // ���� ��ġ ��Ͽ� (����� ǥ�ÿ�)
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int spawnable = maxEnemies - currentEnemyCount;
            if (spawnable > 0)
            {
                int toSpawn = Mathf.Min(Random.Range(1, 3), spawnable);
                for (int i = 0; i < toSpawn; i++)
                {
                    SpawnOneEnemy();
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOneEnemy()
    {
        Vector3 spawnPos = GetBoxDonutSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;

        // ���� ��ġ ����(����� ǥ�ÿ�)
        spawnPositions.Add(spawnPos);

    }

    public void OnEnemyRemoved()
    {
        currentEnemyCount--;
        if (isQuitting) return;
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
            if (++safety > 20) break; // ���ѷ��� ����
        } while (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(center.x, center.z)) < innerRadius);

        return pos;
    }

    // �����
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
        Gizmos.color = Color.red;
        foreach (var pos in spawnPositions)
        {
            Gizmos.DrawSphere(pos + Vector3.up * 0.2f, 0.5f);
        }
    }
}
