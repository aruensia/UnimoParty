//�ֵ���
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;


public class PhotonLobbyMgr : MonoBehaviourPunCallbacks
{
    public TMP_InputField joinRoomInput;

    public Transform roomListPanel;

    public GameObject roomPrefab;

    public GameObject testImg;

    List<string> names = new List<string>() { "���ɳ��� ����", "�ֻ��� � ��", "�ֻ����� ���� ã�ƶ�", "�� �뵷 �� ��� ����" };
    static int nameCount;

    //��������
    public void isServer()
    {
        //���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    string RandomRoomName()
    {
        string roomname = names[nameCount] + " " + UnityEngine.Random.Range(0, 9999);
        nameCount++;
        if (nameCount >= names.Count) nameCount = 0;
        return roomname;
    }

    public void CreateRoom()
    {
        Debug.Log("CreateRoom() �Լ� ȣ���"); // �� ó�� ������ �������� Ȯ��

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("���� ���� ���� ��ȣ, �� ���� �õ� ��");
            string roomName = RandomRoomName();

            RoomOptions options = new RoomOptions
            {
                MaxPlayers = 4,
                EmptyRoomTtl = 0,
                IsOpen = true
            };

            PhotonNetwork.CreateRoom(roomName, options);
        }
        else if (PhotonNetwork.IsConnected == false)
        {
            Debug.LogWarning("���� ������ ���� ������� �ʾҽ��ϴ�.");
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"�� ���� ����! �ڵ�: {returnCode}, ����: {message}");
    }

    public void JoinRoom()
    {
        Debug.Log("JoinRoom");
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public void JoinRandomRoom()
    {
        Debug.Log("JoinRandomRoom");
        PhotonNetwork.JoinRandomRoom();
    }

    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("�볪��");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �ݹ�");
        testImg.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� ����. ����: " + cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����. ���� �̷��� ���ο� �� ����");
        //PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions()); //�� ������ִ� �޼���. �տ� �� �̸�, �ڿ� �ɼ�
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        PhotonNetworkMgr.Instance.changeScene("Room");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ����");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("�κ� ����");
    }


    private Dictionary<string, GameObject> roomDictionary = new Dictionary<string, GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("����Ʈ����"); // ����� �α�

        // ���� ��ųʸ��� ��ϵ� �� �̸����� �����ؼ� ����Ʈ�� ���� (������ �� ���)
        List<string> removeRoom = new List<string>(roomDictionary.Keys);

        foreach (RoomInfo roomInfo in roomList)
        {
            // ���� ������ ���
            if (roomInfo.RemovedFromList == true)
            {
                // �ش� ���� ��ųʸ��� ������
                if (roomDictionary.ContainsKey(roomInfo.Name) == true)
                {
                    // ��ư ������Ʈ ���� �� ��ųʸ������� ����
                    Destroy(roomDictionary[roomInfo.Name]);
                    roomDictionary.Remove(roomInfo.Name);
                }
            }
            else // ���� ������ ���̰ų� ���� ��
            {
                // ���� ��ư�� ������ ���� �� ���� ���
                if (roomDictionary.ContainsKey(roomInfo.Name) == false)
                {
                    // �� ��ư �������� �����Ͽ� �� ����Ʈ �г� �Ʒ��� ����
                    var roomBtn = Instantiate(roomPrefab, roomListPanel);
                    roomBtn.GetComponentInChildren<TextMeshProUGUI>().text = roomInfo.Name;
                    roomBtn.GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(roomInfo.Name));

                    // ��ųʸ��� �� �̸��� ��ư ������Ʈ ���
                    roomDictionary.Add(roomInfo.Name, roomBtn);
                }
            }
        }
    }

   


}