using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XrControllerMgr : MonoBehaviour
{
    [SerializeField] //������ > ä���� ��ü
    InputActionReference AInputActionReference; 

    //���ӿ�����Ʈ Ȱ��ȭ ��Ȱ��ȭ��
    [SerializeField] GameObject HandHarvestObj;
    [SerializeField] GameObject ItemObj;

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
        if (isItemController == true)
        {
            Debug.Log("��Ʈ�� a��ư ä���� > �������� ��ü");
            IsItemObj(true);
        }
        else if (isItemController == false)
        {
            Debug.Log("��Ʈ�� a��ư �������� > ä���� ��ü");
            IsItemObj(false);
        }

    }

    void IsItemObj(bool isItem)
    {
        ItemObj.SetActive(isItem);
        HandHarvestObj.SetActive(!isItem);
    }






}
