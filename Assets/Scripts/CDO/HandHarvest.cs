using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHarvest : MonoBehaviour
{
    [SerializeField] XRRayInteractor RayInteractor;

    //�ڷ�ƾ����
    Coroutine harvestingRoutine = null; //�����ڷ�ƾ
    Coroutine decreaseRoutine = null; //�����ڷ�ƾ

    //��Ÿ�� ������
    [Header("������ �ӵ� ����")]
    public  float currentProgress = 0f; // ���������
    [SerializeField] float harvestTime = 3f; // ������ ��Ÿ��
    [SerializeField] float decreaseSpeed = 0.5f; // �پ��� �ӵ�
    List<float> checkPoints = new List<float>(); // üũ����Ʈ ���

    FlowerUi flowerUi;

    void OnEnable()
    {
        RayInteractor.selectEntered.AddListener(OnGrabbed);
        RayInteractor.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        RayInteractor.selectEntered.RemoveListener(OnGrabbed);
        RayInteractor.selectExited.RemoveListener(OnReleased);
    }

    private void Start()
    {
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

        Debug.Log("ä�� �Ϸ�!");
    }

}
