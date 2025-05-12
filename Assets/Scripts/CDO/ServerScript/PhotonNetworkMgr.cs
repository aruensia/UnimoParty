using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonNetworkMgr : MonoBehaviour
{
    public static PhotonNetworkMgr Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != null) 
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        // �� ����ȭ ����
        // ������ Ŭ���̾�Ʈ�� LoadLevel�� ���� �ٲٸ� ����� ��� Ŭ���̾�Ʈ�� ���� ������ �ڵ� �̵���
        PhotonNetwork.AutomaticallySyncScene = true;

        // �ӽ÷� â���� ����ǰ� ����
        // �ػ�: 1366 x 768 / ��üȭ��(false) �� â���
        Screen.SetResolution(1366, 768, false);

        // ���� �α� ���� ���� (�αװ� �ʹ� ���� ��µ��� �ʵ��� ����)
        // ErrorsOnly: ������ ��� (Debug.Log ���� �� ������)
        PhotonNetwork.LogLevel = PunLogLevel.ErrorsOnly;
        //TODO : ���� �����ؾߴ� ������
        //Debug.unityLogger.logEnabled = false;

    }

    // �� ��ȯ �Լ�
    // �ܺο��� �� �Լ� ȣ�� ��, ���� ��Ʈ��ũ�� ���� �� ��ȯ�� ������
    // ����: PhotonNetwork.LoadLevel�� ���� AutomaticallySyncScene ������ �Ǿ� �־�� ��� Ŭ���̾�Ʈ�� ������ ������ �̵��� (��Ƽ�÷��� ����ȭ ����)
    public void changeScene(string SceneName)
    {
        PhotonNetwork.LoadLevel(SceneName);
    }
}
