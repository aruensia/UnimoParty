using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestPvP : MonoBehaviourPunCallbacks
{
    [Header("���� ��ŸƮ ��ư ��")]
    [SerializeField] Button developerGameStartBtn;
    [SerializeField] Button designerGameStartBtn;

    [Header("�� ���� ��ư ��")]
    [SerializeField] Button developerCreateRoom;
    [SerializeField] Button designerCreateRoom;


    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        designerGameStartBtn.interactable = false;

        designerGameStartBtn.onClick.AddListener(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3);
            }
        });

        developerGameStartBtn.interactable = false;

        developerGameStartBtn.onClick.AddListener(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3);
            }
        });
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ������");
    }


    //��ȹ �� ���� �� ����
    public void DesignerPVPJoinOrCreatRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("Designer", new RoomOptions { MaxPlayers = 8 },null);
    }

    //���� �� ���� �� ����
    public void DeveloperPVPJoinOrCreatRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("Developer", new RoomOptions { MaxPlayers = 8 }, null);
    }



    public override void OnConnected()
    {
        OnJoinedLobby();
    }


    //���� ����
    public override void OnJoinedRoom()
    {
        //��ȹ�� �� ����
        int designerInRoom = 1;
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.Name== "Designer")
        {
            designerGameStartBtn.interactable = true;
            designerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{designerInRoom} �� �غ�Ϸ�\n���� ����";
        }
        else if(PhotonNetwork.CurrentRoom.Name == "Designer")
        {
            designerInRoom++;
            designerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{designerInRoom} �� ����";
        }



        //���� �� ����
        int developerInRoom = 1;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.Name== "Developer")
        {
            developerGameStartBtn.interactable = true;
            developerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{developerInRoom} �� �غ�Ϸ�\n���� ����";
        }
        else if(PhotonNetwork.CurrentRoom.Name == "Developer")
        {
            developerInRoom++;
            developerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{developerInRoom} �� ����";
        }

    }
}
