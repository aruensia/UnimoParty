using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class AgainGame : MonoBehaviourPunCallbacks
{
    private string roomCode;
    private NextAction nextAction;

    // enum Ŭ����
    private enum NextAction
    {
        None,
        PlayAgain,
        GoToLobby
    }

    // ���� �� �ϱ� ��ư Ŭ�� �� ȣ��
    public void OnClickPlayAgain()
    {
        roomCode = Random.Range(10000,99999).ToString();

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);


        //�̰� �ڷ�ƾ���� ���� �ϴ°� ���� ���� �ٵ� ���� �Ѿ�� ���� ���� ������ ������ �𸣴� GPT�� ������ �غ���
        //���� 1 �ε� ���� �� ���Ŀ��� ������ �ǹ���
        //���� 2 �ε� ���� �Ϸ� �� �Ŀ� ���� �����ϰ� LOBBYMANAGER�� �ִ� CREATROOM �Լ��� ��ߵ� ROOMCODE�� ������
    }

    // �κ� ��ư Ŭ�� �� ȣ��
    public void OnClickGoToLobby()
    {
        if (!PhotonNetwork.InRoom) return;

        nextAction = NextAction.GoToLobby;
        PhotonNetwork.LeaveRoom();
    }

    // LeaveRoom �Ϸ� �� �ڵ� ȣ���
    public override void OnLeftRoom()
    {
        if (nextAction == NextAction.PlayAgain)
        {
            RoomOptions options = new RoomOptions { MaxPlayers = 8 };
            PhotonNetwork.JoinOrCreateRoom(roomCode, options, TypedLobby.Default);
        }
        else if (nextAction == NextAction.GoToLobby)
        {
            SceneManager.LoadScene("Lobby 1");
        }

        // �ʱ�ȭ
        nextAction = NextAction.None;
        roomCode = "";
    }
}
