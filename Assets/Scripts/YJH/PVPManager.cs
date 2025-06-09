using System.Collections;
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
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject PVEPanel;
    [SerializeField] GameObject PVPPanel;
    [SerializeField] GameObject roomPanel;

    [Header("��Ī �з�")]
    [SerializeField] Image matchImage;
    [SerializeField] TextMeshProUGUI Count;

    [Header("�� �ȿ�")]
    [SerializeField] TextMeshProUGUI roomNumber;
    [SerializeField] GameObject checkPanel;

    [SerializeField] GameObject nicknamePanel;
    [SerializeField] Transform nicknamePanelParent;
    [SerializeField] Button actionButton;
    [SerializeField] TextMeshProUGUI actionButtonText;

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

    
    IEnumerator Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        Debug.Log("��Ʈ��ũ ���� ��");

        ShowPanel(lobbyPanel);
        PVEPanel.SetActive(false);
        PVPPanel.SetActive(false);
        roomPanel.SetActive(false);


        PhotonNetwork.NickName = FirebaseAuthMgr.user.DisplayName;
    }
    private void ShowPanel(GameObject nextPanel)
    {
        if (currentPanel != null)
        {
            panelStack.Push(currentPanel);
            currentPanel.SetActive(false);
        }

        currentPanel = nextPanel;
        currentPanel.SetActive(true);
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
        if (panelStack.Count > 0)
        {
            currentPanel.SetActive(false);
            currentPanel = panelStack.Pop();
            currentPanel.SetActive(true);
        }
    }

    public void Stage1()
    {
        SceneManager.LoadScene(2);
    }

    public void CreatRoom()
    {
        PVPPanel.SetActive(false);
        roomPanel.SetActive(true);
        PhotonNetwork.CreateRoom($"{Random.Range(10000, 99999)}", new RoomOptions {IsVisible = false,MaxPlayers=8 });
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
            
            if(PhotonNetwork.InRoom)
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
            
            Debug.Log($"[��ġ����ŷ] �ο� ���� �� maxPlayers: {maxPlayers}");
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
            UpdateActionButton();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                AddNicknameUI(p);
            }

            roomNumber.text = PhotonNetwork.CurrentRoom.Name;
        }
    }
    //�÷��̾� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.Name != "Random")
        {
            AddNicknameUI(newPlayer);
            UpdateActionButton();
            if(PhotonNetwork.IsMasterClient)
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

            if(PhotonNetwork.IsMasterClient)
            {
                CheckAllReady();
            }
            UpdateActionButton();
        }
    }
    private void CheckAllReady()
    {
        int totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        actionButton.interactable = (readyCount >= totalPlayers - 1);
    }
    private void AddNicknameUI(Player p)
    {
        GameObject panel = PhotonNetwork.Instantiate("PlayerPanel", Vector3.zero, Quaternion.identity, 0);
        panel.transform.SetParent(nicknamePanelParent, false);

        TextMeshProUGUI texts = panel.GetComponentInChildren<TextMeshProUGUI>();
        texts.text = p.NickName;
        
        if(p.ActorNumber != PhotonNetwork.MasterClient.ActorNumber)
        {
            panel.GetComponentInChildren<Image>().enabled = false;
        }
        playerUIMap[p.ActorNumber] = panel;
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ã�� ����");
    }
    public void CodeJoinRoom()
    {
        Debug.Log("���� ��");
        PhotonNetwork.JoinRoom(codeInput.text);
    }




    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateActionButton();
    }
    public void ReadyButton()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("SetReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }


    public void StartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
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
    public override void OnLeftRoom()
    {
        maxPlayers--;
        if (PhotonNetwork.CurrentRoom.Name == "Random")
        {
            Debug.Log(maxPlayers + " ������ �� ����");
            PhotonNetwork.JoinRandomOrCreateRoom(null, maxPlayers, MatchmakingMode.FillRoom, null, null, "Random", new RoomOptions { MaxPlayers = maxPlayers }, null);
        }
    }
    [PunRPC]
    public void SetReadyState(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber &&
            !PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            readyCount++;
            if (playerUIMap.TryGetValue(actorNumber, out GameObject ui))
            {
                Image stateImage = ui.GetComponentInChildren<Image>();
                stateImage.gameObject.SetActive(true);

                stateImage.GetComponentInChildren<TextMeshProUGUI>().text = "�غ�";
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            CheckAllReady();
        }
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
