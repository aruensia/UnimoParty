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
    [SerializeField] int harvestPoint = 0;


    [SerializeField] private InputActionReference activateAction;
    Flower flower;

    //�ݹ��� OnEnable �ȴ�
    //player�� �Ȼ�����ϱ� awake, start�� ������ ��� 
    //�ݹ� �����ѰǾ�����
    void OnEnable()
    {
        //RayInteractor.selectEntered.AddListener(OnGrabbed);
        //RayInteractor.selectExited.AddListener(OnReleased);
        
        activateAction.action.performed += OnTriggerPressed;
        activateAction.action.canceled += OnTriggerReleased;
    }

    void OnDisable()
    {
        //RayInteractor.selectEntered.RemoveListener(OnGrabbed);
        //RayInteractor.selectExited.RemoveListener(OnReleased);

         activateAction.action.performed -= OnTriggerPressed;
        activateAction.action.canceled -= OnTriggerReleased;
    }

    //�����ڵ� ��ߴ� flower������������
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        flower = null;
        if (RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            flower = hit.collider.GetComponent<Flower>();
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
        if (this.gameObject.activeSelf == true)
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


    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("�÷��̾� ������ ����");
        flowerUi = args.interactableObject.transform.GetComponent<FlowerUi>();
        StartHarvest();

    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log("�÷��̾� �տ��� ����");
        StopHarvest();

    }


   
    public void StartHarvest()
    {
        if (decreaseRoutine != null)
        {
            StopCoroutine(decreaseRoutine);
            decreaseRoutine = null;
        }

        if (harvestingRoutine == null)
        {
            harvestingRoutine = StartCoroutine(HarvestCoroutine());
        }
    }

    public void StopHarvest()
    {
        if (harvestingRoutine != null)
        {
            StopCoroutine(harvestingRoutine);
            harvestingRoutine = null;
        }

        if (decreaseRoutine == null)
        {
            decreaseRoutine = StartCoroutine(DecreaseCoroutine());
        }
    }

    //��Ȯ ����
    //��ŸŸ�� ��µ� �����ǰ������� �Ⱥ����� �Ű�Ƚᵵ ���
    //��Ȯ���ҷ��� �ٲ�ߴ��
    private IEnumerator HarvestCoroutine()
    {
        while (currentProgress < harvestTime)
        {
            currentProgress += Time.deltaTime;
            flowerUi.UpdateGauge(currentProgress);

            yield return null;
        }

        CompleteHarvest();
    }

    //��Ȯ ����(�߰� ��������)
    private IEnumerator DecreaseCoroutine()
    {
        while (currentProgress > 0f)
        {
            // ���൵ ����
            float progressChange = Time.deltaTime * decreaseSpeed;
            currentProgress = Mathf.Max(0f, currentProgress - progressChange);

            foreach (float checkPoint in checkPoints)
            {
                if (Mathf.Abs(currentProgress - checkPoint) < 0.01f)
                {
                    currentProgress = checkPoint;
                    break;
                }
            }

            // UI ����
            flowerUi.UpdateGauge(currentProgress);

            yield return null;
        }
            decreaseRoutine = null;
        
    }

    //ä�� ���
    private void CompleteHarvest()
    {
        harvestingRoutine = null;
        currentProgress = 0f;

        flowerUi.gameObject.SetActive(false);

        harvestPoint++;

        Debug.Log("ä�� ����");
    }

}
