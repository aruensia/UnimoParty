using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHarvest : MonoBehaviour
{
    [SerializeField] XRRayInteractor RayInteractor;
    FlowerUi flowerUi; //ä����

    //�ڷ�ƾ����
    Coroutine harvestingRoutine = null; //�����ڷ�ƾ
    Coroutine decreaseRoutine = null; //�����ڷ�ƾ

    //��Ÿ�� ������
    [Header("������ ����")]
    public  float currentProgress = 0f; // ���������
    [SerializeField] float harvestTime = 3f; // ������ ��Ÿ��
    [SerializeField] float decreaseSpeed = 0.5f; // �پ��� �ӵ�
    List<float> checkPoints = new List<float>(); // üũ����Ʈ ���

    [Header("�׽�Ʈ ä������ ����Ʈ")]
    [SerializeField] int SpiritVisagePoint = 0;


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

  
   

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger ��");
        if (this.gameObject.activeSelf == true && flower !=null)
        {
            flower.StopHarvest();
        }
    }


    private void Start()
    {
        //üũ����Ʈ
        checkPoints.Add(harvestTime / 3f);
        checkPoints.Add(harvestTime / 3f * 2f);
    }

    //ä�� ���
    private void CompleteHarvest()
    {
        harvestingRoutine = null;
        currentProgress = 0f;

        //���߿� rpc�ٲ�ߴ�
        flowerUi.gameObject.SetActive(false);

        SpiritVisagePoint++;

        Debug.Log("ä�� ����");
    }

}
