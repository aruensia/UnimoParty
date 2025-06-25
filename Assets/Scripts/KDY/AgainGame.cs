using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class AgainGame : MonoBehaviourPunCallbacks
{
    // "�� �� �� �ϱ�" Ŭ�� �� ȣ���
    public void OnClickPlayAgain()
    {
        // 1. ���� �� �ڵ� ���� �� Manager�� ����
        string roomCode = Random.Range(10000, 99999).ToString();
        Manager.Instance.PlayAgainRoomCode = roomCode;
        Manager.Instance.IsPlayAgainPending = true;

        Debug.Log($"[AgainGame] �ٽ��ϱ� ���� - �� �ڵ�: {roomCode}");

        // 2. �� ������
        PhotonNetwork.LeaveRoom();
    }

    // "�κ�� ����" Ŭ�� �� ȣ���
    public void OnClickGoToLobby()
    {
        Manager.Instance.IsPlayAgainPending = false;
        Manager.Instance.PlayAgainRoomCode = "";

        Debug.Log("[AgainGame] �κ�� ���� ����");
        PhotonNetwork.LeaveRoom();
    }

    // LeaveRoom �Ϸ� �� �ݹ�
    public override void OnLeftRoom()
    {
        Debug.Log("[AgainGame] �� ������ �Ϸ� �� �κ�� �̵�");
        SceneManager.LoadScene("Lobby 1"); // ���� ó��
    }
}
