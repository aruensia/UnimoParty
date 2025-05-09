using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [Header("������ �÷��̾� ������")]
    public GameObject playerPrefab;

    [Header("���� ����Ʈ ����Ʈ")]
    public List<Transform> spawnPoints = new List<Transform>();


    private void Start()
    {
        // �Ѹ��� �÷��̾� ����
        //SpawnAtIndex(0);

        // 8�� �÷��̾� ����
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            SpawnAtIndex(i);
        }
    }


    public void SpawnAtIndex(int index)
    {
        if (index >=0 &&  index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];

            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPoint.name}");
        }
        else
        {
            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
        }
    }
    




}
