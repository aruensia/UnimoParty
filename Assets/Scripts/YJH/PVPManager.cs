using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPManager : MonoBehaviourPunCallbacks
{
    [Header("�ǳڵ�")]
    [SerializeField] GameObject LobbyCanvas;
    [SerializeField] GameObject PVECanvas;
    [SerializeField] GameObject sendInvitePanel;
    [SerializeField] GameObject receiveInvitePopup;

    [Space]
    public Transform contentParent;
    public Button userButtonPrefab;


    [SerializeField] TextMeshProUGUI inviteText;
    private Dictionary<string, Button> playerButtons = new Dictionary<string, Button>();
    private Player selectedPlayerForInvite; //�ʴ� ��� �����

    

    void Start()
    {
        PVECanvas.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.NickName = FirebaseAuthMgr.user.DisplayName;

        sendInvitePanel.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("LobbyRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("�� �̸� : " +PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.PlayerList.Length);

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

    // �÷��̾� ���� ������ �г����� ���� ��ư ����
    void AddPlayerButton(Player p)
    {
        Button button = Instantiate(userButtonPrefab, contentParent);
        button.GetComponentInChildren<TextMeshProUGUI>().text = p.NickName;
        playerButtons.Add(p.NickName, button);

        if (p.IsLocal)
        {
            button.interactable = false;
            button.transform.SetAsFirstSibling();
        }
        else
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SendInviteButton(p));            
        }
    }

    // �÷��̾� ������ ��ư ����
    void RemovePlayerButton(Player p)
    {
        if (playerButtons.TryGetValue(p.NickName, out Button btn))
        {
            Destroy(btn.gameObject);
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

    // ��ư Ŭ�� ��, �ʴ� ��� ����
    public void SendInviteButton(Player p)
    {
        selectedPlayerForInvite = p;

        sendInvitePanel.SetActive(true);
        inviteText.text = $"{p.NickName} ���� �ʴ� �ϰڽ��ϱ�?";
    }

    // "��" ��ư Ŭ�� �� RPC�� �ʴ� ����
    public void YesButton()
    {
        if (selectedPlayerForInvite != null)
        {
            Debug.Log(selectedPlayerForInvite.NickName);
            Debug.Log(photonView);
            photonView.RPC("PartyInvite", selectedPlayerForInvite);
        }

        sendInvitePanel.SetActive(false);
    }

    public void NoButton()
    {
        sendInvitePanel.SetActive(false);
        selectedPlayerForInvite = null;
    }

    // ���濡�� �ʴ� UI ����
    [PunRPC]
    public void PartyInvite()
    {
        inviteText.text = "�ʴ� ����";
        receiveInvitePopup.SetActive(true);
        selectedPlayerForInvite = null;
    }
}
