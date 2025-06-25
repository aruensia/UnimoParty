using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;

public class AgainGame : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // �� �Ѿ�� �� �װ� ��
    }
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
        SceneManager.LoadScene(1);
    }

}
