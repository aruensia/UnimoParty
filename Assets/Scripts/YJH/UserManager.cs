using UnityEngine;
using UnityEngine.UI;
using Oculus.Platform;

public class UserManager : MonoBehaviour
{
    public static string UserID { get; private set; } = "Unknown";

    public delegate void OnUserReadyDelegate(string userID);
    public static event OnUserReadyDelegate OnUserReady;

    [Header("Editor �׽�Ʈ�� ��¥ ID")]
    public string mockUserID = "TEST_USER_01";

    [Header("UI ǥ�ÿ� (���û���)")]
    public Text displayNameText;
    void Start()
    {
        if (!Core.IsInitialized())
            Core.Initialize();

        Entitlements.IsUserEntitledToApplication().OnComplete(entitlementMsg =>
        {
            if (entitlementMsg.IsError)
            {
                Debug.LogError("[UserManager] ��ǰ ���� ����");
                UserID = "Unauthorized";
                if (displayNameText != null)
                    displayNameText.text = "Not entitled";
                OnUserReady?.Invoke(UserID);
                return;
            }

            Debug.Log("[UserManager] ��ǰ ���� ����");

            Users.GetLoggedInUser().OnComplete(userMsg =>
            {
                if (userMsg.IsError)
                {
                    Debug.LogError("[UserManager] ����� ���� �ε� ����: " + userMsg.GetError().Message);
                    UserID = "UserLoadFailed";
                }
                else
                {
                    UserID = userMsg.Data.OculusID;
                    Debug.Log("[UserManager] Oculus ID: " + UserID);
                }

                if (displayNameText != null)
                    displayNameText.text = "Hello, " + UserID + "!";

                OnUserReady?.Invoke(UserID);
            });
        });
    }

}
