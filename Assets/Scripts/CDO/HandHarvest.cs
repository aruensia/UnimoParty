using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHarvest : MonoBehaviour
{
    [SerializeField] XRRayInteractor RayInteractor;

    [SerializeField]int SpiritPoint = 0; //
    public int spiritPoint {  get { return SpiritPoint; } set { if (value < 0) { Debug.Log("����������");value = 0; } SpiritPoint = value; }}

    [SerializeField] private InputActionReference activateAction;
    Flower flower;

    //�ݹ��� OnEnable �ȴ�
    //player�� �Ȼ�����ϱ� awake, start�� ������ ��� 
    //�ݹ� �����ѰǾ�����
    void OnEnable()
    {
        activateAction.action.performed += OnTriggerPressed;
        activateAction.action.canceled += OnTriggerReleased;
    }

    void OnDisable()
    {
        activateAction.action.performed -= OnTriggerPressed;
        activateAction.action.canceled -= OnTriggerReleased;
    }

    //�����ڵ� ��ߴ� flower������������
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        flower = null;

        if (RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {

            if (!hit.collider.TryGetComponent<Flower>(out flower))
            {
                return;
            }

            //�� ������ �ڵ�
            if (flower != null)
            {
                flower.StartHarvest();
                Debug.Log("Flower ��Ȯ ����!");
            }
        }
    }

    void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger ��");
        if (flower.gameObject.activeSelf == true && flower !=null)
        {
            flower.StopHarvest();
        }
    }

    //���� ����
    public int DeliverySpirit()
    {
        int SpiritReturnPoint = spiritPoint;
        spiritPoint = 0;
        return SpiritReturnPoint;
    }


}
