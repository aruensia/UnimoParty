using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class FirebaseLoginMgr : MonoBehaviour
{
    //���� 
    static public FirebaseUser user;
    static public FirebaseAuth auth;


    //�α��ο�(TMP�� �ֽ�ȭ ����)
    [Header("�α��ο�")]
    [SerializeField] private TMP_InputField LoginIdInputField;
    [SerializeField] private TMP_InputField LoginPasswordInputField;
    [SerializeField] TextMeshProUGUI LoginwarningText;

    //ȸ�����Կ�
    [Header("ȸ�����Կ�")]
    [SerializeField] private TMP_InputField CreateIdInputField;
    [SerializeField] private TMP_InputField CreatePasswordInputField;
    [SerializeField] TextMeshProUGUI CreatewarningText;

    //�г��� ������
    [Header("�г��� ������")]
    [SerializeField] private TMP_InputField NickNameInputField;
    [SerializeField] TextMeshProUGUI NickNamewarningText;

    [Header("ū�׵θ�Ui")]
    //[SerializeField] private GameObject SceneChanege;
    [SerializeField] private GameObject LoginUiPanel;
    [SerializeField] private GameObject CreateUiIdPanel;
    [SerializeField] private GameObject NickNameUiPanel;

    private void Awake()
    {
        //�����ڵ� auth����
        //�񵿱�� ContinueWith
        //���̾�̽� �ʱ�ȭ
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            }
        });


        CreatewarningText.text = "";
        LoginwarningText.text = "";

        CreateUiIdPanel.gameObject.SetActive(false);
        LoginUiPanel.gameObject.SetActive(true);
        NickNameUiPanel.gameObject.SetActive(false);
    }

    //ȸ�������гη� �Ѿ
    //���߿��� ���ڰ����� �ް� �ϸ� �ɵ�
    public void CreateIdPanel()
    {
        //ȸ�������г�
        CreateUiIdPanel.gameObject.SetActive(true);
        LoginUiPanel.gameObject.SetActive(false);
        NickNameUiPanel.gameObject.SetActive(false);

    }

    public void CreateIdPanelFalse()
    {
        LoginUiPanel.gameObject.SetActive(true);
        CreateUiIdPanel.gameObject.SetActive(false);
    }

    //�г����гη� �Ѿ
    public void NickNamePanel()
    {
        //ȸ�������г�
        CreateUiIdPanel.gameObject.SetActive(false);
        LoginUiPanel.gameObject.SetActive(false);
        NickNameUiPanel.gameObject.SetActive(true);

    }

    //�α����гη� �Ѿ
    public void LoginPanel()
    {
        //ȸ�������г�
        CreateUiIdPanel.gameObject.SetActive(false);
        LoginUiPanel.gameObject.SetActive(true);
        NickNameUiPanel.gameObject.SetActive(false);

    }



    //ȸ������
    public void CreateId()
    {
        StartCoroutine(CreateIdCor(CreateIdInputField.text, CreatePasswordInputField.text));
    }

    //�α���
    public void Login()
    {
        StartCoroutine(LoginCor(LoginIdInputField.text, LoginPasswordInputField.text));
    }

    //�α׾ƿ�
    public void Logout()
    {
        auth.SignOut();
    }

    //public void CreateNickName()
    //{
    //    StartCoroutine(CreateNickNameCor(NickNameInputField.text));
    //}

    //IEnumerator CreateNickNameCor(string NickName)
    //{
    //    if (user != null)
    //    {
    //        UserProfile profile = new UserProfile { DisplayName = NickName };

    //        Task profileTask = user.UpdateUserProfileAsync(profile);
    //        while (profileTask.IsCompleted ==false)
    //        {
    //            NickNamewarningText.text += "1";
    //            yield return null;
    //        }

    //        yield return new WaitUntil(() => profileTask.IsCompleted);


    //        if (profileTask.Exception != null)
    //        {
    //            Debug.LogWarning("�г��� ���� ����: " + profileTask.Exception);
    //            FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
    //            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
    //            NickNamewarningText.text = "�г��� ���� ����";
    //        }
    //        else
    //        {
    //            int delay = 0;
    //            while (user.DisplayName==null || user.DisplayName != NickName)
    //            {
    //                yield return new WaitForSeconds(0.2f);
    //                delay++;
    //                //NickNamewarningText.text = $"�г��� ����... {delay}";
    //            }

    //            //yield return new WaitUntil(() => XRGeneralSettings.Instance.Manager.isInitializationComplete);
    //            SceneManager.LoadScene("Lobby");
    //        }
    //    }
    //}

    //IEnumerator CreateNickNameCor(string nickName)
    //{
    //    if (user != null)
    //    {
    //        UserProfile profile = new UserProfile { DisplayName = nickName };
    //        var updateTask = user.UpdateUserProfileAsync(profile);
    //        yield return new WaitUntil(() => updateTask.IsCompleted);

    //        if (updateTask.Exception != null)
    //        {
    //            NickNamewarningText.text = "�г��� ���� ����";
    //            yield break;
    //        }

    //        // �ִ� 5��(10ȸ)���� �г��� �ݿ� ��ٸ���
    //        int retry = 0;
    //        while (retry < 10)
    //        {
    //            var reloadTask = user.ReloadAsync();
    //            yield return new WaitUntil(() => reloadTask.IsCompleted);
    //            yield return new WaitForSeconds(0.3f);

    //            if (!string.IsNullOrEmpty(user.DisplayName) && user.DisplayName == nickName)
    //                break;

    //            retry++;
    //        }

    //        if (user.DisplayName != nickName)
    //        {
    //            NickNamewarningText.text = "�г��� ���� ����. �ٽ� �õ����ּ���";
    //            yield break;
    //        }

    //        // VR�� XR �ʱ�ȭ ���
    //        yield return new WaitUntil(() => XRGeneralSettings.Instance.Manager.isInitializationComplete);

    //        // �г��� ���� �ݿ� �� �� �̵�
    //        SceneManager.LoadScene("Lobby");
    //    }
    //}

    //����� ȸ������ �ڷ�ƾ
    IEnumerator CreateIdCor(string ID, string password)
    {
        var createIdTask = auth.CreateUserWithEmailAndPasswordAsync(ID + "@unimo.com", password);
        //ȸ������ �����Ҷ� ����
        yield return new WaitUntil(predicate: () => createIdTask.IsCompleted);
        if (createIdTask.Exception != null)
        {
            Debug.LogWarning(message: "������ ���� ������ ȸ������ ����:" + createIdTask.Exception);
            FirebaseException firebaseEx = createIdTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = " ȸ������ ����";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "���̵� �Է����ּ���";
                    break;
                case AuthError.MissingPassword:
                    message = "�н����带 �Է����ּ���";
                    break;
                case AuthError.WeakPassword:
                    message = "�ּ� 6�ڸ� �̻����� ������ּ���";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "�ߺ��� ���̵� �Դϴ�";
                    break;
                default:
                    message = "�����ڿ��� ���� �ٶ��ϴ�";
                    break;
            }
            CreatewarningText.text = message;
        }
        else
        {
            Debug.Log("ȸ������ �Ϸ�");
            user = createIdTask.Result.User;
            CreatewarningText.text = "";
            LoginUiPanel.gameObject.SetActive(true);
            CreateUiIdPanel.gameObject.SetActive(false);
        }

    }

    //����� �α��� �ڷ�ƾ 
    IEnumerator LoginCor(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email + "@unimo.com", password);
        //�α��� �����Ҷ� ����
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogWarning(message: "������ ���� ������ �α��� ����:" + loginTask.Exception);
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "�α��� ����";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "���̵� �Է����ּ���";
                    break;
                case AuthError.MissingPassword:
                    message = "�н����带 �Է����ּ���";
                    break;
                case AuthError.WrongPassword:
                    message = "�н����尡 ��Ƚ��ϴ�";
                    break;
                case AuthError.UserNotFound:
                    message = "���̵� ã�� �� �����ϴ�";
                    break;
                default:
                    message = "�����ڿ��� ���� �ٶ��ϴ�";
                    break;
            }
            LoginwarningText.text = message;
        }
        else
        {
            user = loginTask.Result.User;
            LoginwarningText.text = "";
            LoginUiPanel.gameObject.SetActive(false);

            SceneManager.LoadScene("Lobby");
        }
    }
}
