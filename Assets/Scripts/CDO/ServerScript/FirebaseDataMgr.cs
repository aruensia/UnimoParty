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
using Random = UnityEngine.Random;

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

    //private void Start()
    //{
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
    //    {
    //        FirebaseApp app = FirebaseApp.DefaultInstance;
    //        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    //        if (FirebaseAuthMgr.user != null)
    //        {
    //            //TODO:�ʱ� ���� �ٲٱ� 12000
    //            if (userMoney == -1)
    //            {
    //                //StartCoroutine(SaveUserData(FirebaseAuthMgr.user.DisplayName, "gold", 10000));
    //                userMoney = 10000;
    //            }

    //            // ģ�� ��� �ҷ�����
    //            //await LoadFriends(FirebaseAuthMgr.user.DisplayName);

    //        }
    //        else
    //        {
    //            Debug.LogError("���̾�̽� ����");
    //        }
    //    });
    //}

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
}
