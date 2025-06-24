using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [Header("���� ����Ʈ ����Ʈ (�ν����Ϳ��� �������� ����)")]
    public List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        StartCoroutine(wait());
        Debug.Log($"���� ĳ���� �ε���: {SelectedData.characterIndex}");
        Debug.Log($"���� ���ּ� �ε���: {SelectedData.shipIndex}");
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnAtIndex(PhotonNetwork.LocalPlayer.ActorNumber - 1);
        }
    }

    public void SpawnAtIndex(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];
            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

            GameObject player = PhotonNetwork.Instantiate("PlayerVariant", spawnPoint.position, yRotationOnly);

            if (player.GetComponent<PhotonView>().IsMine)
            {
                PhotonView pv = player.GetComponent<PhotonView>();
                pv.RPC("SetupPlayer", RpcTarget.AllBuffered, SelectedData.characterIndex, SelectedData.shipIndex);
            }
        }
    }

}
