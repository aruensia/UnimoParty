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
    [SerializeField] Button developerJoinRoomBtn;
    [SerializeField] Button designerJoinRoomBtn;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        designerGameStartBtn.interactable = false;
        designerJoinRoomBtn.interactable = false;

        designerGameStartBtn.onClick.AddListener(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3);
            }
        });
        designerJoinRoomBtn.onClick.AddListener(() => DesignerPVPJoinRoom());

        developerGameStartBtn.interactable = false;
        developerJoinRoomBtn.interactable = false;

        developerGameStartBtn.onClick.AddListener(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3);
            }
        });
        developerJoinRoomBtn.onClick.AddListener(() => DeveloperPVPJoinRoom());
    }


    //��ȹ �� ����
    public void DesignerPVPCreatRoom()
    {
        PhotonNetwork.CreateRoom("Designer", new RoomOptions { MaxPlayers = 8 });
        
    }
    //��ȹ �� ����
    public void DesignerPVPJoinRoom()
    {
        PhotonNetwork.JoinRoom("Designer");
    }




    //���� �� ����
    public void DeveloperPVPCreatRoom()
    {
        PhotonNetwork.CreateRoom("Developer", new RoomOptions { MaxPlayers = 8 });
    }
    //���� �� ����
    public void DeveloperPVPJoinRoom()
    {
        PhotonNetwork.JoinRoom("Developer");
    }



    //���� ����
    public override void OnJoinedRoom()
    {
        //��ȹ�� �� ����
        int designerInRoom = 1;
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.Name== "Designer")
        {
            designerGameStartBtn.interactable = true;
            designerJoinRoomBtn.interactable = true;
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
            developerJoinRoomBtn.interactable = true;
            developerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{developerInRoom} �� �غ�Ϸ�\n���� ����";
        }
        else if(PhotonNetwork.CurrentRoom.Name == "Developer")
        {
            developerInRoom++;
            developerGameStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{developerInRoom} �� ����";
        }


    }
}
