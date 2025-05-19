using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerSpawner : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnInterval = 3f;
    public int poolSize = 20;

    [Header("�簢�� ũ��")]
    public float rectWidth = 30f;
    public float rectHeight = 30f;

    [Header("�����")]
    public bool showGizmos = true;

    private List<GameObject> enemyPool = new List<GameObject>();
    private int currentEnemyCount = 0;

    void Start()
    {
        // Ǯ �̸� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.one * 9999, Quaternion.identity);
            obj.SetActive(false);
            enemyPool.Add(obj);

            // CornerEnemy�� ������ ���� ����
            var enemyScript = obj.GetComponent<ShookShook>();
            if (enemyScript != null)
                enemyScript.spawner = this;
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
                SpawnAtRandomEdge();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAtRandomEdge()
    {
        GameObject enemy = GetPooledEnemy();
        if (enemy == null) return;

        int edge = Random.Range(0, 4);
        Vector3 spawnPos, targetPos;
        GetEdgePair(edge, out spawnPos, out targetPos);

        // Terrain ���� ����
        spawnPos.y = Terrain.activeTerrain ? Terrain.activeTerrain.SampleHeight(new Vector3(spawnPos.x, 100f, spawnPos.z)) : spawnPos.y;
        targetPos.y = Terrain.activeTerrain ? Terrain.activeTerrain.SampleHeight(new Vector3(targetPos.x, 100f, targetPos.z)) : targetPos.y;

        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);

        var enemyScript = enemy.GetComponent<ShookShook>();
        if (enemyScript != null)
            enemyScript.Init(spawnPos, targetPos);

        currentEnemyCount++;
    }

    // �Ʒ�(��), ������(��), ��(��), ����(��) �𼭸� ���� �̵� ��ȯ
    void GetEdgePair(int edge, out Vector3 spawn, out Vector3 target)
    {
        Vector3 center = transform.position;
        float halfWidth = rectWidth / 2f;
        float halfHeight = rectHeight / 2f;
        float t = Random.Range(0f, 1f);

        switch (edge)
        {
            case 0: // �Ʒ�(����) �� ��(����), x�� ����, z�� ����
                spawn = new Vector3(Mathf.Lerp(center.x - halfWidth, center.x + halfWidth, t), center.y, center.z - halfHeight);
                target = new Vector3(spawn.x, center.y, center.z + halfHeight);
                break;
            case 1: // ������(����) �� ����(����), z�� ����, x�� ����
                spawn = new Vector3(center.x + halfWidth, center.y, Mathf.Lerp(center.z - halfHeight, center.z + halfHeight, t));
                target = new Vector3(center.x - halfWidth, center.y, spawn.z);
                break;
            case 2: // ��(����) �� �Ʒ�(����), x�� ����, z�� ����
                spawn = new Vector3(Mathf.Lerp(center.x - halfWidth, center.x + halfWidth, t), center.y, center.z + halfHeight);
                target = new Vector3(spawn.x, center.y, center.z - halfHeight);
                break;
            case 3: // ����(����) �� ������(����), z�� ����, x�� ����
                spawn = new Vector3(center.x - halfWidth, center.y, Mathf.Lerp(center.z - halfHeight, center.z + halfHeight, t));
                target = new Vector3(center.x + halfWidth, center.y, spawn.z);
                break;
            default:
                spawn = target = center;
                break;
        }
    }


    GameObject GetPooledEnemy()
    {
        foreach (var obj in enemyPool)
            if (!obj.activeInHierarchy) return obj;
        return null;
    }

    int GetActiveEnemyCount()
    {
        int count = 0;
        foreach (var obj in enemyPool)
            if (obj.activeInHierarchy) count++;
        return count;
    }

    public void OnEnemyRemoved()
    {
        currentEnemyCount--;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(rectWidth, 0.1f, rectHeight));

        // 4�� Edge(��) ���� ǥ��
        Vector3 center = transform.position;
        float halfWidth = rectWidth / 2f;
        float halfHeight = rectHeight / 2f;
        Vector3[] points = new Vector3[5];
        points[0] = new Vector3(center.x - halfWidth, center.y, center.z - halfHeight);
        points[1] = new Vector3(center.x + halfWidth, center.y, center.z - halfHeight);
        points[2] = new Vector3(center.x + halfWidth, center.y, center.z + halfHeight);
        points[3] = new Vector3(center.x - halfWidth, center.y, center.z + halfHeight);
        points[4] = points[0];
        for (int i = 0; i < 4; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);
    }
}
