using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("�ǳڵ�")]
    [SerializeField] GameObject LobbyCanvas;
    [SerializeField] GameObject PVECanvas;
    [SerializeField] GameObject sendInvitePanel;
    [SerializeField] GameObject receiveInvitePopup;

    [Space]
    public Transform contentParent;
    public Button userButtonPrefab;

    private Dictionary<string, Button> playerButtons = new Dictionary<string, Button>();

    void Start()
    {
        PVECanvas.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NickName = FirebaseAuthMgr.user.DisplayName;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 20 };
        PhotonNetwork.CreateRoom("LobbyRoom", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            AddPlayerButton(p);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerButton(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayerButton(otherPlayer);
    }


    //�÷��̾� ���ö����� �г����� ���� ��ư ����
    void AddPlayerButton(Player p)
    {
        Button button = Instantiate(userButtonPrefab, contentParent);
        button.GetComponentInChildren<TextMeshProUGUI>().text = p.NickName;
        playerButtons.Add(p.NickName, button);

        if (p.IsLocal)
        {
            button.interactable = false;
        }
        else
        {
            //button.onClick.AddListener(()SendInviteButton(Player p));
            button.transform.SetAsFirstSibling();
        }
    }

    //�÷��̾� ������ �ڱ� �ڽ� ��ư ����
    void RemovePlayerButton(Player p)
    {
        if (playerButtons.TryGetValue(p.NickName, out Button btn))
        {
            Destroy(btn);
            playerButtons.Remove(p.NickName);
        }
    }

    public void OnClickPVESceneButton()
    {
        LobbyCanvas.SetActive(false);
        PVECanvas.SetActive(true);
    }

    public void OnClickBackButton()
    {
        LobbyCanvas.SetActive(true);
        PVECanvas.SetActive(false);
    }

    public void SoloPlayButton()
    {
        SceneManager.LoadScene(2);
    }
    //�г����� �ִ� ��ư�� ������ ��Ƽ�ʴ� ��ư �߱�
    public void SendInviteButton(Player p)
    {
        sendInvitePanel.SetActive(true);
        sendInvitePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{p.NickName} ���� �ʴ� �ϰڽ��ϱ�?";
    }

    //��Ƽ�ʴ븦 ���� �� ��뿡�� ��Ƽ�ʴ� �Դٴ� �ڵ� ������
    [PunRPC]
    public void PartyInvite()
    {
        receiveInvitePopup.SetActive(true);
    }


}
