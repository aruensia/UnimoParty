//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.InputSystem; // New Input System Ű���� �Է¿�
//using UnityEngine;

//// ���� ����Ʈ�� �ε����� ���� �� ������ ��ġ�� �����ϴ� ��ũ��Ʈ
//// �ν����Ϳ��� ���� ����Ʈ ����Ʈ�� �����ϰ�,
//// �ε����� �Է¹޾� �ش� ��ġ�� �÷��̾ �����ϴ� ����� ���

//public class SpawnManager : MonoBehaviour
//{
//    // ������ �÷��̾� �������� �ν����Ϳ��� ����
//    [Header("������ �÷��̾� ������")]
//    public GameObject playerPrefab;

//    // ���� ����Ʈ���� �ν����Ϳ��� ������� �Ҵ� (���ϴ� ��Ģ�� ���ؼ� ����)
//    [Header("���� ����Ʈ ����Ʈ")]
//    public List<Transform> spawnPoints = new List<Transform>();

//    private int currentSpawnIndex = 0; // ������� ������ �ε��� ��ȣ

//    // ���� ���۽� �׽�Ʈ������ �������� (�����ص� �������)
//    private void Start()
//    {
//        // ù ��° ���� ��ġ�� �� �� ����
//        SpawnAtIndex(0);  

//        // ��� ���� ����Ʈ�� ������� �÷��̾� ����
//        //for (int i = 0; i < spawnPoints.Count; i++)
//        //{
//        //    SpawnAtIndex(i);      
//        //}
//    }

//    private void Update()
//    {
//        if (Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            SpawnNext();
//        }
//    }

//    private void SpawnNext()
//    {
//        currentSpawnIndex++;

//        if (currentSpawnIndex >= spawnPoints.Count)
//        {
//            Debug.Log("�� �̻� ������ ��ġ�� �����ϴ�.");
//            return;
//        }

//        SpawnAtIndex(currentSpawnIndex);
//    }



//    // ������ �ε����� �÷��̾ �����ϴ� �޼���
//    // index: ������ ��ġ�� �ε��� ��ȣ (�ν����Ϳ��� ������ ����)
//    public void SpawnAtIndex(int index)
//    {
//        // �ε����� ����Ʈ ������ �ִ��� Ȯ��
//        if (index >=0 &&  index < spawnPoints.Count)
//        {
//            Transform spawnPoint = spawnPoints[index];

//            // �ش� ���� ����Ʈ�� ��ġ,ȸ���� �����ͼ� �÷��̾� ����
//            //Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

//            Vector3 spawnPos = spawnPoint.position;

//            // Y�� ȸ���� ���� (X/Z�� 0����)
//            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

//            Instantiate(playerPrefab, spawnPos, yRotationOnly);

//            Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPoint.name}");
//        }
//        else
//        {
//            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
//        }
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("������ �÷��̾� ������")]
    public GameObject playerPrefab;

    [Header("�߽� ������Ʈ")]
    public Transform centerPoint;

    [Header("���� ������ (�߽ɿ��� �Ÿ�)")]
    public float spawnRadius = 5f;

    [Header("�ڵ� ������ ���� ����Ʈ (�б� ����)")]
    public List<Transform> spawnPoints = new List<Transform>();

    private int currentSpawnIndex = 0;

    private void Start()
    {
        GenerateSpawnPointsAroundCenter();

        if (spawnPoints.Count > 0)
        {
            SpawnAtIndex(0); // ù ��° �ڵ� ������ ��ġ���� ����
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        currentSpawnIndex++;

        if (currentSpawnIndex >= spawnPoints.Count)
        {
            Debug.Log("�� �̻� ������ ��ġ�� �����ϴ�.");
            return;
        }

        SpawnAtIndex(currentSpawnIndex);
    }

    public void SpawnAtIndex(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];
            Vector3 spawnPos = spawnPoint.position;
            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

            Instantiate(playerPrefab, spawnPos, yRotationOnly);
            Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
        }
    }

    private void GenerateSpawnPointsAroundCenter()
    {
        spawnPoints.Clear();

        if (centerPoint == null)
        {
            Debug.LogError("�߽� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * spawnRadius;
            float z = Mathf.Sin(angle) * spawnRadius;

            Vector3 spawnPos = centerPoint.position + new Vector3(x, 0f, z);

            // Terrain ���� ���� (����)
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                float terrainY = terrain.SampleHeight(spawnPos) + terrain.GetPosition().y;
                spawnPos.y = terrainY;
            }
            else
            {
                spawnPos.y = centerPoint.position.y;
            }

            // �� GameObject�� ���� ��ġ ����
            GameObject point = new GameObject($"SpawnPoint_{i}");
            point.transform.position = spawnPos;
            point.transform.SetParent(this.transform); // ���� ������
            spawnPoints.Add(point.transform);
        }

        Debug.Log("8���� ���� ����Ʈ�� �ڵ� �����Ǿ����ϴ�.");
    }
}

