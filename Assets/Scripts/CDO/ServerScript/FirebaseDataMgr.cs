using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseDataMgr : MonoBehaviour
{
    public static FirebaseDataMgr Instance { get; private set; }
    private DatabaseReference dbReference;
    public int userMoney = -1;

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

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            if (FirebaseLoginMgr.user != null)
            {
                //TODO:�ʱ� ���� �ٲٱ� 12000
                userMoney = await LoadUserDataAsync(FirebaseLoginMgr.user.DisplayName, "money", userMoney);
                if (userMoney == -1)
                {
                    SaveUserData(FirebaseLoginMgr.user.DisplayName, "money", 10000);
                    userMoney = 10000;
                }

                // ģ�� ��� �ҷ�����
                await LoadFriends(FirebaseLoginMgr.user.DisplayName);

                Debug.Log("���� �г��� : " + FirebaseLoginMgr.user.DisplayName);
                Debug.Log("���� �� : " + userMoney);
            }
            else
            {
                Debug.LogError("���̾�̽� ����");
            }
        });
    }

    //������ ���� �Լ�
    //SaveUserData(id,"level",5);
    //id�� ������ 5 �߰�?
    //ContinueWithOnMainThread ���ξ����忡�� ��
    public void SaveUserData<T>(string userId, string dataName, T value)
    {
        dbReference.Child("users").Child(userId).Child(dataName).SetValueAsync(value).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(userId + "��" + dataName + value + "�߰���");
            }
            else
            {
                Debug.LogError("������");
            }
        });
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

    // ģ�� �߰� �Լ�
    public void AddFriend(string friendNickname)
    {
        if (!friendList.Contains(friendNickname))
        {
            friendList.Add(friendNickname);
            SaveFriends(FirebaseLoginMgr.user.DisplayName);
        }
    }

    // ģ�� ���� �Լ� (����Ʈ�� ���� ����)
    public void SaveFriends(string userId)
    {
        for (int i = 0; i < friendList.Count; i++)
        {
            dbReference.Child("users").Child(userId).Child("friends").Child(i.ToString())
                .SetValueAsync(friendList[i]);
        }
    }

    // ģ�� ��� �ҷ����� �Լ� (���� �� �ҷ���)
    public async Task LoadFriends(string userId)
    {
        friendList.Clear();
        DataSnapshot snapshot = await dbReference.Child("users").Child(userId).Child("friends").GetValueAsync();
        if (snapshot.Exists)
        {
            foreach (DataSnapshot child in snapshot.Children)
            {
                friendList.Add(child.Value.ToString());
            }
            Debug.Log(userId + "�� ģ�� ��� �ҷ���: " + string.Join(", ", friendList));
        }
        else
        {
            Debug.Log(userId + "�� ģ�� ��� ����");
        }
    }

    // Ư�� �г����� ģ������ Ȯ��
    public bool IsFriend(string nickname)
    {
        return friendList.Contains(nickname);
    }
}
