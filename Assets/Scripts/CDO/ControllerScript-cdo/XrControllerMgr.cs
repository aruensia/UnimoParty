using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XrControllerMgr : MonoBehaviour
{
    [SerializeField] //������ > ä���� ��ü a
    InputActionReference AInputActionReference; 

    //���ӿ�����Ʈ Ȱ��ȭ ��Ȱ��ȭ��
    [SerializeField] GameObject HandHarvestObj;
    [SerializeField] GameObject ItemObj;

    [SerializeField] ItemInputB itemInputB;

    bool isItemController = false;   //ó���� ä���� ����

    private void OnEnable()
    {
        AInputActionReference.action.Enable();
        AInputActionReference.action.performed += ControllerA;

    }

    private void OnDisable()
    {
        AInputActionReference.action.performed -= ControllerA;
        AInputActionReference.action.Disable();

    }


    //������, ä���� ��ü a
    private void ControllerA(InputAction.CallbackContext context)
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

    void IsItemObj(bool isItem)
    {
        ItemObj.SetActive(isItem);
        HandHarvestObj.SetActive(!isItem);
    }






}
