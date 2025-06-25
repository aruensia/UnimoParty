using System.Collections;
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
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject PVEPanel;
    [SerializeField] GameObject PVPPanel;
    [SerializeField] GameObject roomPanel;

    [Header("��Ī �з�")]
    [SerializeField] Image matchImage;
    [SerializeField] TextMeshProUGUI Count;
    [SerializeField] GameObject failPanel;
    [Header("�� �ȿ�")]
    [SerializeField] TextMeshProUGUI roomNumber;
    [SerializeField] GameObject checkPanel;

    [SerializeField] Transform nicknamePanelParent;
    [SerializeField] Button actionButton;
    [SerializeField] TextMeshProUGUI actionButtonText;
    [SerializeField] GameObject gameSettingPanel;
    [SerializeField] GameObject playerPanelPrefab;
    [SerializeField] GameObject gameSettingButton;


    [Header("��Ī �з�")]
    [SerializeField] Button makeRoomBtn;
    [SerializeField] TMP_InputField codeInput;


    private byte maxPlayers = 8;
    private const byte minPlayers = 2;

    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private GameObject currentPanel;
    private int readyCount = 0;


    private Dictionary<int, GameObject> playerUIMap = new Dictionary<int, GameObject>();
    Coroutine matchCorutine;
    private bool isMatchMaking;

    public List<Player> playerList = new List<Player>();
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        PhotonNetwork.NickName = FirebaseAuthMgr.user.DisplayName;

        Debug.Log("��Ʈ��ũ ���� ��");

        ShowPanel(lobbyPanel);
        PVEPanel.SetActive(false);
        gameSettingPanel.SetActive(false);
        PVPPanel.SetActive(false);
        roomPanel.SetActive(false);
        failPanel.SetActive(false);

        if (Manager.Instance.IsPlayAgainPending)
        {
            yield return new WaitForSeconds(0.5f);
            Manager.Instance.IsPlayAgainPending = false;

            string roomCode = Manager.Instance.PlayAgainRoomCode;
            codeInput.text = roomCode;

            PhotonNetwork.JoinOrCreateRoom(roomCode, new RoomOptions { MaxPlayers = 8 }, TypedLobby.Default);

            PVPPanel.SetActive(false);
            roomPanel.SetActive(true);
        }
    }
    private void ShowPanel(GameObject nextPanel)
    {
        if (panelStack.Count > 0)
        {
            panelStack.Peek().SetActive(false);
        }
        panelStack.Push(nextPanel);
        nextPanel.SetActive(true);
        currentPanel = panelStack.Peek().gameObject;
    }
    //�ν����� ���� �ֱ� ��
    public void PVEButton()
    {
        ShowPanel(PVEPanel);
    }
    //�ν����� ���� �ֱ� ��
    public void PVPButton()
    {
        ShowPanel(PVPPanel);
    }

    public void BackButton()
    {
        if (panelStack.Count > 1)
        {
            GameObject last = panelStack.Pop();
            last.SetActive(false);

            currentPanel = panelStack.Peek();
            currentPanel.SetActive(true);
        }

    }
    //PVE �������� ����
    public void Stage1()
    {
        SceneManager.LoadScene(2);
    }

    public void CreatRoom()
    {
        StartCoroutine(WaitCreatRoom());
    }
    IEnumerator WaitCreatRoom()
    {
        PhotonNetwork.JoinLobby();
        //yield return new WaitUntil(() => PhotonNetwork.InLobby);
        //yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() =>
      PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

        PhotonNetwork.CreateRoom($"{Random.Range(10000, 99999)}", new RoomOptions { IsVisible = false, MaxPlayers = 8 });
        PVPPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
    public void InRoomBackButton()
    {
        checkPanel.SetActive(true);
    }
    public void StayButton()
    {
        checkPanel.SetActive(false);
    }
    public void LeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();
        PVPPanel.SetActive(true);
        checkPanel.SetActive(false);
        roomPanel.SetActive(false);
    }

    public void MatchmakingButton()
    {
        isMatchMaking = !isMatchMaking;

        if (!isMatchMaking)
        {
            makeRoomBtn.interactable = true;
            codeInput.interactable = true;
            Count.gameObject.SetActive(false);
            matchImage.color = new Color(1, 1, 0, 0);

            StopCoroutine(matchCorutine);
            matchCorutine = null;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            makeRoomBtn.interactable = false;
            codeInput.interactable = false;

            Count.gameObject.SetActive(true);
            matchCorutine = StartCoroutine(MatchmakingRoutine());
            matchImage.color = new Color(1, 1, 0, 1);
        }
    }

    private IEnumerator MatchmakingRoutine()
    {
        maxPlayers = 8;
        yield return new WaitForSeconds(0.1f);
        int timeElapsed = 0;
        int addTime = 30;
        Count.text = "00:00";
        while (maxPlayers >= minPlayers)
        {

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom(true);
                yield return new WaitUntil(() => !PhotonNetwork.InRoom);
            }
            else
            {
                PhotonNetwork.JoinRandomOrCreateRoom(null, maxPlayers, MatchmakingMode.FillRoom, null, null, "Random", new RoomOptions { MaxPlayers = 8 }, null);
            }
            while (timeElapsed < addTime)
            {
                int minutes = timeElapsed / 60;
                int seconds = timeElapsed % 60;
                Count.text = $"{minutes:D2}:{seconds:D2}";

                yield return new WaitForSeconds(1f);
                timeElapsed++;
            }
            addTime += 30;

        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name == "Random")
        {
            Debug.Log("��ġ����ŷ ����");
        }
        else
        {
            Debug.Log("���� ����");
            roomNumber.text = PhotonNetwork.CurrentRoom.Name;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                AddNicknameUI(p);
            }
            UpdateActionButton();
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.Name != "Random")
        {
            AddNicknameUI(newPlayer);
            UpdateActionButton();
            if (PhotonNetwork.IsMasterClient)
            {
                CheckAllReady();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.Name != "Random")
        {
            if (playerUIMap.TryGetValue(otherPlayer.ActorNumber, out GameObject ui))
            {
                Destroy(ui);
                playerUIMap.Remove(otherPlayer.ActorNumber);
            }

            UpdateActionButton();
            if (PhotonNetwork.IsMasterClient)
            {
                CheckAllReady();
            }
        }
    }
    private void CheckAllReady()
    {
        int totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        if (readyCount >= totalPlayers - 1)
        {
            actionButton.interactable = true;
        }
        else
        {
            actionButton.interactable = false;
        }
    }

    //
    private void AddNicknameUI(Player player)
    {
        if (playerUIMap.ContainsKey(player.ActorNumber))
            return;

        GameObject panel = Instantiate(playerPanelPrefab, nicknamePanelParent);
        RectTransform rt = panel.GetComponent<RectTransform>();

        PlayerPanel pPanel = panel.GetComponent<PlayerPanel>();
        pPanel.Setup(player);
        pPanel.SetReady(false);
        pPanel.MasterClient(player.ActorNumber == PhotonNetwork.MasterClient.ActorNumber);

        playerUIMap[player.ActorNumber] = panel;

        int charIndex = player.CustomProperties.ContainsKey("CharacterIndex") ? (int)player.CustomProperties["CharacterIndex"] : 0;
        int shipIndex = player.CustomProperties.ContainsKey("ShipIndex") ? (int)player.CustomProperties["ShipIndex"] : 0;

        GameObject[] characters = Resources.LoadAll<GameObject>("Characters");
        GameObject[] ships = Resources.LoadAll<GameObject>("Prefabs");

        Transform characterPos = panel.transform.Find("CharacterPos");
        Transform shipPos = panel.transform.Find("SpaceShipPos");

        GameObject charObj = Instantiate(characters[charIndex], characterPos.position, Quaternion.Euler(0,180,0), characterPos);
        GameObject shipObj = Instantiate(ships[shipIndex], shipPos.position, Quaternion.Euler(0, 180, 0), shipPos);
    }
    


    public void CodeJoinRoom()
    {

        Debug.Log("���� ��");
        PhotonNetwork.JoinRoom(codeInput.text);
        PVPPanel.SetActive(false);
        roomPanel.SetActive(true);
    }



    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateActionButton();
    }
    public void ReadyButton()
    {
        photonView.RPC("SetReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);

    }


    public void StartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            Manager.Instance.players.Clear();
            Manager.Instance.players.AddRange(PhotonNetwork.PlayerList);


            Manager.Instance.SetGameList();

            for(int i=0; i< Manager.Instance.players.Count ; i++)
            {
                Debug.Log(Manager.Instance.players[i] + "�÷��̾�� setting");
                Debug.Log(Manager.Instance.players[i].ActorNumber);
            }
            
            PhotonNetwork.LoadLevel(3);

        }
    }
    private void UpdateActionButton()
    {
        actionButton.onClick.RemoveAllListeners();

        if (PhotonNetwork.IsMasterClient)
        {
            actionButtonText.text = "���� ����";
            actionButton.onClick.AddListener(StartGameButton);
        }
        else
        {
            actionButtonText.text = "�غ� �Ϸ�";
            actionButton.onClick.AddListener(ReadyButton);
        }
    }
    [PunRPC]
    public void SetReadyState(int actorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount++;
            CheckAllReady();
        }

        if (playerUIMap.TryGetValue(actorNumber, out var panel))
        {
            panel.GetComponent<PlayerPanel>().SetReady(true);
        }
    }



    public void GameSettingButton()
    {
        gameSettingPanel.SetActive(true);
    }
    public void ExitSetting()
    {
        gameSettingPanel.SetActive(false);
    }
    public void SaveSetting()
    {
        gameSettingPanel.SetActive(false);
    }
    #region ���� ���� ���Ź��� 

    //��ư ���� �� ��ư Ŭ���� ��Ƽ�ʴ�
    //public override void OnJoinedLobby()
    //{
    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        AddPlayerButton(p);
    //    }
    //}

    //public override void OnConnectedToMaster()
    //{
    //    PhotonNetwork.JoinLobby(TypedLobby.Default);
    //}

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("�� �̸� : " +PhotonNetwork.CurrentRoom.Name);
    //    Debug.Log(PhotonNetwork.PlayerList.Length);

    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        AddPlayerButton(p);
    //    }
    //}


    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    RemovePlayerButton(otherPlayer);
    //}

    //// �÷��̾� ���� ������ �г����� ���� ��ư ����
    //void AddPlayerButton(Player p)
    //{
    //    Button button = Instantiate(userButtonPrefab, contentParent);
    //    button.GetComponentInChildren<TextMeshProUGUI>().text = p.NickName;
    //    playerButtons.Add(p.NickName, button);

    //    if (p.IsLocal)
    //    {
    //        button.interactable = false;
    //        button.transform.SetAsFirstSibling();
    //    }
    //    else
    //    {
    //        button.onClick.RemoveAllListeners();
    //        button.onClick.AddListener(() => SendInviteButton(p));
    //    }
    //}

    ////// �÷��̾� ������ ��ư ����
    //void RemovePlayerButton(Player p)
    //{
    //    if (playerButtons.TryGetValue(p.NickName, out Button btn))
    //    {
    //        Destroy(btn.gameObject);
    //        playerButtons.Remove(p.NickName);
    //    }
    //}

    // ��ư Ŭ�� ��, �ʴ� ��� ����
    //public void SendInviteButton(Player p)
    //{
    //    selectedPlayerForInvite = p;

    //    sendInvitePanel.SetActive(true);
    //    inviteText.text = $"{p.NickName} ���� �ʴ� �ϰڽ��ϱ�?";
    //}

    //// �� ��ư Ŭ�� �� RPC�� �ʴ�
    //public void YesButton()
    //{
    //    if (selectedPlayerForInvite != null)
    //    {
    //        photonView.RPC("PartyInvite", selectedPlayerForInvite);
    //    }

    //    sendInvitePanel.SetActive(false);
    //}

    //public void NoButton()
    //{
    //    sendInvitePanel.SetActive(false);
    //    selectedPlayerForInvite = null;
    //}

    // ���濡�� �ʴ� UI ����
    //[PunRPC]
    //public void PartyInvite()
    //{
    //    inviteText.text = "�ʴ� ����";
    //    receiveInvitePopup.SetActive(true);
    //    selectedPlayerForInvite = null;
    //}





    //fakeroom ������� 

    //[Header("Fake�� ����")]
    //[SerializeField] TextMeshProUGUI roomNameText;

    //public void CreateFakeRoom(string rName, string mName, bool isLock)
    //{
    //    var fakeRoom = new FakeRoom()
    //    {
    //        roomName = rName,
    //        mapName = mName,
    //        isLocked = isLock,
    //        isMaster = true
    //    };

    //    roomNameText.text = fakeRoom.roomName;

    //}
    #endregion
}
