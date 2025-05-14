using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ����Ʈ�� �ε����� ���� �� ������ ��ġ�� �����ϴ� ��ũ��Ʈ
// �ν����Ϳ��� ���� ����Ʈ ����Ʈ�� �����ϰ�,
// �ε����� �Է¹޾� �ش� ��ġ�� �÷��̾ �����ϴ� ����� ���

public class SpawnManager : MonoBehaviour
{
    // ������ �÷��̾� �������� �ν����Ϳ��� ����
    [Header("������ �÷��̾� ������")]
    public GameObject playerPrefab;

    // ���� ����Ʈ���� �ν����Ϳ��� ������� �Ҵ� (���ϴ� ��Ģ�� ���ؼ� ����)
    [Header("���� ����Ʈ ����Ʈ")]
    public List<Transform> spawnPoints = new List<Transform>();


    // ���� ���۽� �׽�Ʈ������ �������� (�����ص� �������)
    private void Start()
    {
        // ù ��° ���� ��ġ�� �� �� ����
        SpawnAtIndex(0);  

        // ��� ���� ����Ʈ�� ������� �÷��̾� ����
        //for (int i = 0; i < spawnPoints.Count; i++)
        //{
        //    SpawnAtIndex(i);      
        //}
    }

    // ������ �ε����� �÷��̾ �����ϴ� �޼���
    // index: ������ ��ġ�� �ε��� ��ȣ (�ν����Ϳ��� ������ ����)
    public void SpawnAtIndex(int index)
    {
        // �ε����� ����Ʈ ������ �ִ��� Ȯ��
        if (index >=0 &&  index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];

            // �ش� ���� ����Ʈ�� ��ġ,ȸ���� �����ͼ� �÷��̾� ����
            //Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            Vector3 spawnPos = spawnPoint.position;

            // Y�� ȸ���� ���� (X/Z�� 0����)
            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

            Instantiate(playerPrefab, spawnPos, yRotationOnly);

            Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPoint.name}");
        }
        else
        {
            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
        }
    }
    




}
