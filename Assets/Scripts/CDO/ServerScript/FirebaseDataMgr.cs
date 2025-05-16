using System;
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

    private void Awake()
    {
        Debug.Log("2");
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

        try
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
           {

               Debug.Log("1");
               FirebaseApp app = FirebaseApp.DefaultInstance;
               dbReference = FirebaseDatabase.DefaultInstance.RootReference;
               if (FirebaseLoginMgr.user != null)
               {
               Debug.Log("3");
                   //TODO:�ʱ� ���� �ٲٱ� 12000
                   SaveUserData(FirebaseLoginMgr.user.DisplayName, "money", 12000);
                   userMoney = await LoadUserDataAsync(FirebaseLoginMgr.user.DisplayName, "money", userMoney);
                   if (userMoney == -1)
                   {
                       SaveUserData(FirebaseLoginMgr.user.DisplayName, "money", 10000);
                       userMoney = 10000;
                   }
                   Debug.Log("���� �г��� : " + FirebaseLoginMgr.user.DisplayName);
                   Debug.Log("���� �� : " + userMoney);
               }
               else
               {
                   Debug.LogError("���̾�̽� ����");
               }
           });
        }
        catch (Exception dd)
        {
            Debug.Log(dd);
        }
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

        try
        {
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
        }
        catch (System.Exception dd)
        {
            Debug.Log(dd);
            Tvalue = type;
        }

        return Tvalue;
    }
}
