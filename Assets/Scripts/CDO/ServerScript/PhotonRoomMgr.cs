using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoomMgr : MonoBehaviourPunCallbacks
{
    public Transform RoomListPanel;
    public GameObject RoomUser;
    List<int> readyPlayerIDs = new List<int>();
    public int readyCount => readyPlayerIDs.Count;


    private  void  OnEnable()
    {
        base.OnEnable();


    }

    //�����ϵ� ã�� 
    //�װſ� �ؽ��� ã�Ƽ� ����
    void Start()
    {
        UpdatePlayerList();
    }

    //�������
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("������ ����");
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (readyPlayerIDs.Contains(otherPlayer.ActorNumber))
        {
            readyPlayerIDs.Remove(otherPlayer.ActorNumber);
        }
        Debug.Log("������ ������");
        UpdatePlayerList();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            UpdatePlayerList();
        }
    }

    //0 �ؽ�Ʈ �г���
    //1 �ؽ�Ʈ �����غ�
    //2 ��ư �����غ��ư

    public void UpdatePlayerList()
    {
        for (int i = 0; i < RoomListPanel.childCount; i++)
        {
            Destroy(RoomListPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var RoomList = Instantiate(RoomUser, RoomListPanel); //�� ����Ʈ �г� �Ͽ� �ϳ� ����
            RoomList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[i].NickName;
            //RoomList.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = playerMoney.ToString();

            var player = PhotonNetwork.PlayerList[i];
            bool isLocalPlayer = player == PhotonNetwork.LocalPlayer;

            //�����̳�
            if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                RoomList.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Master";

                var btn = RoomList.transform.GetChild(2).GetComponent<Button>();
                var btnText = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                btnText.text = "GameStart";

                if (isLocalPlayer)
                {
                    btn.onClick.AddListener(StartBtn);
                    //btn.onClick.AddListener(()=>Destroy(dd.transform.GetChild(2).gameObject));
                }
                else
                {
                    Destroy(btn.gameObject);
                }
            }
            //����ƴϳ�
            else
            {
                var statusText = RoomList.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                var btn = RoomList.transform.GetChild(2).GetComponent<Button>();

                // ������ �������� Ȯ��
                //if (readyPlayerIDs.Contains(player.ActorNumber))
                //{
                //    statusText.text = "GameReady";
                //    Destroy(btn.gameObject); // ��ư ����
                //}
                //else
                //{
                //    statusText.text = "GameNoReady";
                //    if (isLocalPlayer)
                //    {
                //        btn.onClick.AddListener(ReadyCountBtn);
                //        btn.onClick.AddListener(() => Destroy(btn.gameObject));
                //        btn.onClick.AddListener(() => statusText.text = "GameReady");
                //    }
                //    else
                //    {
                //        Destroy(btn.gameObject);
                //    }
                //}
                object isReadyObj;
                bool isReady = player.CustomProperties.TryGetValue("IsReady", out isReadyObj) && (bool)isReadyObj;

                if (isReady)
                {
                    statusText.text = "GameReady";
                    Destroy(btn.gameObject); // ��ư ����
                }
                else
                {
                    statusText.text = "GameNoReady";
                    if (isLocalPlayer)
                    {
                        btn.onClick.AddListener(ReadyCountBtn);
                        btn.onClick.AddListener(() => Destroy(btn.gameObject));
                        btn.onClick.AddListener(() => statusText.text = "GameReady");
                    }
                    else
                    {
                        Destroy(btn.gameObject);
                    }
                }


            }
        }



    }

    void ReadyCountBtn()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
    {
        { "IsReady", true }
    };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
            try
            {


                photonView.RPC("ReadyCount", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);

            }
            catch (System.Exception ee)
            {
                Debug.Log(ee);
            }
        }
    }


    //�����ư ������� ��ü�� readyCount�� ����
    //public void ReadyCountBtn()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
    //        try
    //        {


    //            photonView.RPC("ReadyCount", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);

    //        }
    //        catch (System.Exception ee)
    //        {
    //            Debug.Log(ee);
    //        }
    //    }
    //}



    [PunRPC]
    public void ReadyCount(int playerID)
    {
        Debug.Log("����� ������?");
        //readyCount++;
        if (!readyPlayerIDs.Contains(playerID))
        {
            readyPlayerIDs.Add(playerID);
        }

        for (int i = 0; i < RoomListPanel.childCount; i++)
        {
            Transform playerUI = RoomListPanel.GetChild(i);
            TextMeshProUGUI playerNameText = playerUI.GetChild(0).GetComponent<TextMeshProUGUI>();

            Player player = null;
            foreach (var p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == playerID)
                {
                    player = p;
                    break;
                }
            }

            // ���� ������Ʈ�ؾ� �ϴ� �÷��̾� ã��
            //Player player = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == playerID);
            //if (player != null && player.NickName == playerNameText.text)
            //{
            //    playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "�غ� �Ϸ�"; // ��� Ŭ���̾�Ʈ���� UI ����
            //    break;
            //}
        }
    }

    //��ü �������� ���� ������� Ŭ�������ϰ�
    [PunRPC]
    public void StartBtn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("���� ���� ��ư ������");
            if (readyCount >= PhotonNetwork.PlayerList.Length - 1)
            {
                Debug.Log("���ӽ��� ��ư ������ �ΰ��� ������ �ѱ� ");
                PhotonNetwork.CurrentRoom.IsOpen = false; //���� ���� �� �� ������
                PhotonNetworkMgr.Instance.changeScene("InGame");
            }
        }
    }
   
}