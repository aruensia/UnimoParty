using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XrControllerMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] //������ > ä���� ��ü a
    InputActionReference AInputActionReference;

    //���ӿ�����Ʈ Ȱ��ȭ ��Ȱ��ȭ��
    [SerializeField] GameObject handHarvestObj;
    [SerializeField] GameObject ItemObj;

    [SerializeField] ItemInputB itemInputB;

    [SerializeField] GameObject ItemGun;
    [SerializeField] GameObject harvestGun;
    [SerializeField] ActionBasedController rightController;

    GameObject RController;

    bool isItemController = false;   //ó���� ä���� ����

    private void Start()
    {
        RController = Instantiate(ItemGun, rightController.gameObject.transform);
        RController.SetActive(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AInputActionReference.action.Enable();
        AInputActionReference.action.performed += ControllerA;

    }

    public override void OnDisable()
    {
        base.OnDisable();
        AInputActionReference.action.performed -= ControllerA;
        AInputActionReference.action.Disable();

    }

    Transform lasetRcontroller;
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

    void IsItemObj(bool isItem)
    {
        ItemObj.SetActive(isItem);
        handHarvestObj.SetActive(!isItem);
        if(isItem == true)
        {
            lasetRcontroller = rightController.model;
            rightController.model.gameObject.SetActive(false);
            RController.SetActive(true);
            rightController.model = RController.transform;
            rightController.modelPrefab = RController.transform;
        }
        else if(isItem == false)
        {
            rightController.model = lasetRcontroller;
            rightController.model.gameObject.SetActive(true);
            RController.SetActive(false);
            rightController.model = lasetRcontroller;
            rightController.modelPrefab = lasetRcontroller;

        }

    }


   

    



}
