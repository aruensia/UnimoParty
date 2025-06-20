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

    // Ư�� �ε��� ��ġ�� �÷��̾ �����ϴ� �Լ�
    public void SpawnAtIndex(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            Transform spawnPoint = spawnPoints[index];
            Quaternion yRotationOnly = Quaternion.Euler(0, spawnPoint.rotation.eulerAngles.y, 0);

            GameObject player = PhotonNetwork.Instantiate("PlayerVariant", spawnPoint.position, yRotationOnly);

            if (player.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("���� ������?");
                StartCoroutine(SetUpCharacterAndShip(player));
            }
        }
    }

    IEnumerator SetUpCharacterAndShip(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);

        Transform characterPos = player.transform.Find("XR Origin (XR Rig)/CharacterPos");
        Transform xrOrigin = player.transform.Find("XR Origin (XR Rig)");

        Transform shipPos = null;

        Transform spaceShip = xrOrigin.GetChild(3);
        shipPos = spaceShip.GetChild(1);

        Debug.Log(characterPos + " �� ĳ���� ������");
        Debug.Log(shipPos + " �� ���ּ� ������");

        GameObject[] characters = Resources.LoadAll<GameObject>("Characters");
        GameObject[] ships = Resources.LoadAll<GameObject>("Prefabs");

        GameObject charObj = Instantiate(characters[SelectedData.characterIndex], characterPos.position, Quaternion.identity, characterPos);
        charObj.transform.localPosition = Vector3.zero;

        GameObject shipObj = Instantiate(ships[SelectedData.shipIndex], shipPos.position, Quaternion.identity, shipPos);
        shipObj.transform.localPosition = Vector3.zero;

    }
}
