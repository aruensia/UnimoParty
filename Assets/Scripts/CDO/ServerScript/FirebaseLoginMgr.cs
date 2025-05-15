using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

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
    [SerializeField] private GameObject SceneChanege;
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
        Debug.Log("�α� �ƿ�");
    }

    public void CreateNickName()
    {
        StartCoroutine(CreateNickNameCor(NickNameInputField.text));
    }

    IEnumerator CreateNickNameCor(string NickName)
    {
        if (user != null)
        {
            //�г���
            UserProfile profile = new UserProfile { DisplayName = NickName };
            //���̾�̽��� �г��� ���� �ø�
            Task ProfileTask = user.UpdateUserProfileAsync(profile);

            yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

            if (ProfileTask.Exception != null)
            {
                Debug.LogWarning(message: "�г��� ���� ����" + ProfileTask.Exception);
                FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                NickNamewarningText.text = "�г��� ���� ����";
            }
            else
            {
                NickNamewarningText.text = "";
                Debug.Log("�г��� : " + user.DisplayName);
                var dd = string.IsNullOrEmpty(user.DisplayName);
                //�г����� ������
                if (dd != true)
                {
                    NickNameUiPanel.gameObject.SetActive(false);
                    SceneChanege.gameObject.SetActive(true);
                }
            }

        }

    }


    //����� ȸ������ �ڷ�ƾ
    IEnumerator CreateIdCor(string email, string password)
    {
        var createIdTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
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
                    message = "�̸��� ����";
                    break;
                case AuthError.MissingPassword:
                    message = "�н����� ����";
                    break;
                case AuthError.WeakPassword:
                    message = "�н����� ����";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "�ߺ� �̸���";
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
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
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
                    message = "�̸��� ����";
                    break;
                case AuthError.MissingPassword:
                    message = "�н����� ����";
                    break;
                case AuthError.WrongPassword:
                    message = "�н����� Ʋ��";
                    break;
                case AuthError.InvalidEmail:
                    message = "�̸��� ������ ���� ����";
                    break;
                case AuthError.UserNotFound:
                    message = "���̵� �������� ����";
                    break;
                default:
                    message = "�����ڿ��� ���� �ٶ��ϴ�";
                    break;
            }
            LoginwarningText.text = message;
        }
        else
        {
            Debug.Log("�α��� �Ϸ�");
            user = loginTask.Result.User;
            LoginwarningText.text = "";
            LoginUiPanel.gameObject.SetActive(false);
            Debug.Log(user.DisplayName);


            //�г����� ������� �г��� ����
            if (string.IsNullOrEmpty(user.DisplayName) == true)
            {
                Debug.Log("�г����� �����ϴ�");
                NickNamePanel();
                CreateNickName();
                //ServerPanel.gameObject.SetActive(true);
            }
            else
            {
                SceneChanege.gameObject.SetActive(true);
            }
        }
    }
}
