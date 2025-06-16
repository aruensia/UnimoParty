using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XrControllerMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject optionPanel;
    //�������� ��ǲ�׼�(��ǲ �����̸Ӹ�)
    [SerializeField] InputActionReference AInputActionReference;
    [SerializeField] InputActionReference MenuInputActionReference;

    [SerializeField] ActionBasedController rightController;

    //���ӿ�����Ʈ Ȱ��ȭ ��Ȱ��ȭ��
    [Header("�� ��ũ��Ʈ ���ִ� ������Ʈ")]
    [SerializeField] GameObject handHarvestObj;
    [SerializeField] GameObject ItemObj;
    [SerializeField] ItemInputB itemInputB;

    [Header("�� ������ (��)")]
    [SerializeField] GameObject harvestGun;
    [SerializeField] GameObject ItemGun;

    private bool isOption = false;
    bool isItemController = false;   //ó���� ä���� ����
    GameObject RController;
    Transform lasetRcontroller; //temp

    private void Start()
    {
        RController = Instantiate(ItemGun, rightController.gameObject.transform);
        RController.SetActive(false);

        optionPanel.SetActive(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AInputActionReference.action.Enable();
        AInputActionReference.action.performed += ControllerA;

        MenuInputActionReference.action.Enable();
        MenuInputActionReference.action.performed += ControllerMenu;

    }

    public override void OnDisable()
    {
        base.OnDisable();
        AInputActionReference.action.performed -= ControllerA;
        AInputActionReference.action.Disable();

        MenuInputActionReference.action.performed -= ControllerMenu;
        MenuInputActionReference.action.Disable();

    }

    //���� , �޴� ��ư Ŭ���� �ɼ� â
    private void ControllerMenu(InputAction.CallbackContext context)
    {
        if(photonView.IsMine)
        {
            isOption = !isOption;
            if(isOption)
            {
                optionPanel.SetActive(true);
            }
            else
            {
                optionPanel.SetActive(false);
            }
        }
    }

    //������, ä���� ��ü a
    private void ControllerA(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {

            if (isItemController == false)
            {
                Debug.Log("��Ʈ�� a��ư ä���� > �������� ��ü");

                isItemController = true;
                IsItemObj(true);
                itemInputB.StateItem(true);
            }
            else if (isItemController == true)
            {
                Debug.Log("��Ʈ�� a��ư �������� > ä���� ��ü");

                isItemController = false;
                itemInputB.StateItem(false);
                IsItemObj(false);
            }
        }

    }

    public void IsItemObj(bool isItem)
    {
        ItemObj.SetActive(isItem);
        handHarvestObj.SetActive(!isItem);
        if (isItem == true)
        {
            lasetRcontroller = rightController.model;
            rightController.model.gameObject.SetActive(false);
            RController.SetActive(true);
            rightController.model = RController.transform;
            rightController.modelPrefab = RController.transform;
        }
        else if (isItem == false)
        {
            rightController.model = lasetRcontroller;
            rightController.model.gameObject.SetActive(true);
            RController.SetActive(false);
            rightController.model = lasetRcontroller;
            rightController.modelPrefab = lasetRcontroller;

        }

    }








}
