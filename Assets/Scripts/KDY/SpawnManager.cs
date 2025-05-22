using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[Header("������ �÷��̾� ������")]
    //public GameObject playerPrefab;

    [Header("���� ����Ʈ ����Ʈ (�ν����Ϳ��� �������� ����)")]
    public List<Transform> spawnPoints = new List<Transform>();

    // ������� ������ �ε����� �����ϴ� ����
    private int currentSpawnIndex = 0;

    // ������ ���۵� �� ����Ǵ� �Լ�
    private void Start()
    {
        // spawnPoints ����Ʈ�� ��� �ִ� ��� ��� ���
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Spawn Points�� ��� �ֽ��ϴ�. �ν����Ϳ��� �������� �����ϼ���.");
        }
        else
        {
            // ù ��° ��ġ�� �÷��̾� ����
            SpawnAtIndex(0);
        }
    }

    // �� �����Ӹ��� ����Ǵ� �Լ�
    private void Update()
    {
        // �����̽� Ű�� ������ ���� ��ġ�� �÷��̾� ����
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnNext();
        }
    }

    // ���� �ε����� �÷��̾ �����ϴ� �Լ�
    private void SpawnNext()
    {
        currentSpawnIndex++;

        // ���� ����Ʈ�� ��� ����� ��� �α� ��� �� ����
        if (currentSpawnIndex >= spawnPoints.Count)
        {
            Debug.Log("�� �̻� ������ ��ġ�� �����ϴ�.");
            return;
        }

        // ���� �ε��� ��ġ�� ����
        SpawnAtIndex(currentSpawnIndex);
    }

    // Ư�� �ε��� ��ġ�� �÷��̾ �����ϴ� �Լ�
    public void SpawnAtIndex(int index)
    {
        // ��ȿ�� �ε������� Ȯ��
        if (index >= 0 && index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];
            Vector3 spawnPos = spawnPoint.position;

            // Y�� ȸ���� �����ϰ� ������ ȸ���� ����
            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

            // �÷��̾� ����
            //Instantiate(playerPrefab, spawnPos, yRotationOnly);

            //Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPos}");
        }
        else
        {
            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
        }
    }
}
