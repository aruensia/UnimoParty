using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FirebaseDataMgr : MonoBehaviour
{
    public static FirebaseDataMgr Instance { get; private set; }
    private DatabaseReference dbReference;
    public int userMoney = -1;

    [SerializeField] GameObject NickNamePanel;

    [Space]
    [Header("�г��� ����ĭ")]
    [SerializeField] TMP_InputField nickInputField;

    [Space]
    [Header("�г��� ���â")]
    [SerializeField] TextMeshProUGUI NickNamewarningText;

    // �߰�: ģ�� ��� ����Ʈ
    public List<string> friendList = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {


        yield return new WaitUntil(() => FirebaseApp.DefaultInstance != null);

        yield return new WaitUntil(() => FirebaseLoginMgr.user != null);

        if (string.IsNullOrWhiteSpace(FirebaseLoginMgr.user.DisplayName))
        {
            NickNamePanel.SetActive(true);
            yield return new WaitUntil(() => !string.IsNullOrEmpty(FirebaseLoginMgr.user.DisplayName));
        }


        NickNamePanel.SetActive(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            if (FirebaseLoginMgr.user != null)
            {
                //TODO:�ʱ� ���� �ٲٱ� 12000
                userMoney = await LoadUserDataAsync(FirebaseLoginMgr.user.DisplayName, "gold", userMoney);
                if (userMoney == -1)
                {
                    StartCoroutine(SaveUserData(FirebaseLoginMgr.user.DisplayName, "gold", 10000));
                    userMoney = 10000;
                }

                // ģ�� ��� �ҷ�����
                //await LoadFriends(FirebaseLoginMgr.user.DisplayName);

            }
            else
            {
                Debug.LogError("���̾�̽� ����");
            }
        });
    }

    public void CreateNickName()
    {
        StartCoroutine(CreateNickNameCor());
    }
    IEnumerator CreateNickNameCor()
    {
        if (FirebaseLoginMgr.user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = nickInputField.text
            };

            Task profileTask = FirebaseLoginMgr.user.UpdateUserProfileAsync(profile);

            yield return new WaitUntil(() => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                Debug.LogWarning("�г��� ���� ����: " + profileTask.Exception);
                FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                NickNamewarningText.text = "�г��� ���� ����";
            }
            else
            {
                if (!string.IsNullOrEmpty(FirebaseLoginMgr.user.DisplayName))
                {
                    NickNamewarningText.text = "�г��� ���� �Ϸ�!";
                }
                else
                {
                    NickNamewarningText.text = "�г��� ���� �ε� ����";
                }
            }
        }
    }

    //������ ���� �Լ�
    //SaveUserData(id,"level",5);
    //id�� ������ 5 �߰�?
    //ContinueWithOnMainThread ���ξ����忡�� ��
    public IEnumerator SaveUserData<T>(string userId, string dataName, T value)
    {
        var task = dbReference.Child("users").Child(userId).Child(dataName).SetValueAsync(value);

        yield return new WaitUntil(()=> task.IsCompleted);
    }

    //������ �ҷ����� �Լ�
    //�Լ����� �տ� await �ٿ��ߴ�
    //playerLevel = await LoadUserDataAsync(id, "level", useLevel);
    //id�� ���� �ҷ����� playerLevel ������ ����
    //playerLevel =  ������value; �̷���
    //await�Ҷ����� ��޸�
    //https://ljhyunstory.tistory.com/284 
    public async Task<T> LoadUserDataAsync<T>(string userId, string dataName, T type)
    {
        // �񵿱������� ������ �ҷ�����
        DataSnapshot snapshot = await dbReference.Child("users").Child(userId).Child(dataName).GetValueAsync();
        T Tvalue;

        Tvalue = type;
        if (snapshot.Exists)
        {
            //Ÿ���� �ٲ㼭 �������
            Tvalue = (T)Convert.ChangeType(snapshot.Value, typeof(T));
            Debug.Log(userId + "�� " + dataName + "�ҷ���");
            Debug.Log("Tvalue : " + Tvalue);
        }
        else
        {
            Debug.Log("����� ������ ����");
        }
        return Tvalue;
    }

    //// ģ�� �߰� �Լ�
    //public void AddFriend(string friendNickname)
    //{
    //    if (!friendList.Contains(friendNickname))
    //    {
    //        friendList.Add(friendNickname);
    //        SaveFriends(FirebaseLoginMgr.user.DisplayName);
    //    }
    //}

    //// ģ�� ���� �Լ� (����Ʈ�� ���� ����)
    //public void SaveFriends(string userId)
    //{
    //    for (int i = 0; i < friendList.Count; i++)
    //    {
    //        dbReference.Child("users").Child(userId).Child("friends").Child(i.ToString())
    //            .SetValueAsync(friendList[i]);
    //    }
    //}

    //// ģ�� ��� �ҷ����� �Լ� (���� �� �ҷ���)
    //public async Task LoadFriends(string userId)
    //{
    //    friendList.Clear();
    //    DataSnapshot snapshot = await dbReference.Child("users").Child(userId).Child("friends").GetValueAsync();
    //    if (snapshot.Exists)
    //    {
    //        foreach (DataSnapshot child in snapshot.Children)
    //        {
    //            friendList.Add(child.Value.ToString());
    //        }
    //        Debug.Log(userId + "�� ģ�� ��� �ҷ���: " + string.Join(", ", friendList));
    //    }
    //    else
    //    {
    //        Debug.Log(userId + "�� ģ�� ��� ����");
    //    }
    //}

    //// Ư�� �г����� ģ������ Ȯ��
    //public bool IsFriend(string nickname)
    //{
    //    return friendList.Contains(nickname);
    //}

}
