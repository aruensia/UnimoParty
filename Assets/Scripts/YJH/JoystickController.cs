using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandController : MonoBehaviourPunCallbacks
{
    public GameObject xrOriginObject;     // XROrigin ������Ʈ
    [Space]
    [Header("���̽�ƽ �̵��ӵ�")]
    public float moveSpeed = 1.0f;        // �̵� �ӵ�
    [Space]
    public XRJoystick joystick;


    [Header("���� ��Ʈ�ѷ�")]
    public ActionBasedController leftController;
    private Renderer[] handRenderers;

    [Header("�� ��� ������")]
    public GameObject controllerPrefab;
    [Space]

    [Header("�� ��ġ")]
    public Transform joystickTF;
    [Space]

    private Transform xrOriginTransform;

    [Header("���� �г���")]
    [SerializeField] TextMeshProUGUI userName;
    //LeftController
    GameObject LController;
    void Start()
    {
        //userName.text = FirebaseLoginMgr.user.DisplayName;
        if(photonView.IsMine)
        {
            handRenderers = leftController.GetComponentsInChildren<Renderer>();

            LController = Instantiate(controllerPrefab, joystickTF);
            LController.SetActive(false);

            xrOriginTransform = xrOriginObject.transform;

            joystick.onValueChangeY.AddListener(OnJoystickMoveY);
            joystick.onValueChangeX.AddListener(OnJoystickMoveX);
        }
        
    }

    void OnDestroy()
    {
        joystick.onValueChangeY.RemoveListener(OnJoystickMoveY);
        joystick.onValueChangeX.RemoveListener(OnJoystickMoveX);
    }

    private void OnJoystickMoveY(float value)
    {
        Vector3 forward = new Vector3(xrOriginTransform.forward.x, 0f, xrOriginTransform.forward.z).normalized;
        xrOriginTransform.position += forward * value * moveSpeed * Time.deltaTime;
    }

    private void OnJoystickMoveX(float value)
    {
        Vector3 right = new Vector3(xrOriginTransform.right.x, 0f, xrOriginTransform.right.z).normalized;
        xrOriginTransform.position += right * value * moveSpeed * Time.deltaTime;
    }

    public void OnSelectEnter()
    {
        if (photonView.IsMine)
        {
            LController.SetActive(true);
            SetHandVisible(false);

            leftController.model.gameObject.SetActive(false);
        }
    }
    public void OnSelectExit()
    {
        if (photonView.IsMine)
        {
            LController.SetActive(false);
            SetHandVisible(true);
            leftController.model.gameObject.SetActive(true);
        }
    }

    void SetHandVisible(bool visible)
    {
        foreach (var renderer in handRenderers)
        {      
            renderer.enabled = visible;
        }
    }
}
