using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine.UI; // Text��
// using TMPro; // �� TextMeshProUGUI �� ��� �ּ� ����

public class MetaUserIDDisplay : MonoBehaviour
{
    public Text userIdText; // TextMeshProUGUI userIdText; �� TextMeshPro �� ��� ����

    void Start()
    {
        Core.Initialize();

        Entitlements.IsUserEntitledToApplication().OnComplete(OnEntitlementCheck);
    }

    void OnEntitlementCheck(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("���� ����. ����.");
        }
        else
        {
            Users.GetLoggedInUser().OnComplete(OnUserGet);
        }
    }

    void OnUserGet(Message<User> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("���� ���� �������� ����");
        }
        else
        {
            string id = msg.Data.ID.ToString();
            Debug.Log("���� ID: " + id);
            userIdText.text = "User ID: " + id;
        }
    }
}