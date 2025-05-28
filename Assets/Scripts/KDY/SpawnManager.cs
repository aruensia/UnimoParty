using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [Header("���� ����Ʈ ����Ʈ (�ν����Ϳ��� �������� ����)")]
    public List<Transform> spawnPoints = new List<Transform>();

    // ������� ������ �ε����� �����ϴ� ����
    private int currentSpawnIndex = 0;

    // ������ ���۵� �� ����Ǵ� �Լ�
    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.IsLocal)
        {
            SpawnAtIndex(PhotonNetwork.LocalPlayer.ActorNumber - 1);
        }
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
            PhotonNetwork.Instantiate("PlayerVariant", spawnPos, yRotationOnly);

            //Debug.Log($"�÷��̾ ������: �ε��� {index}, ��ġ:{spawnPos}");
        }
        else
        {
            Debug.LogWarning($"SpawnAtIndex: �߸��� �ε��� {index}");
        }
    }
}
