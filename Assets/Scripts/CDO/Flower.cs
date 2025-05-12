using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flower : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;

    [SerializeField] FlowerUi flowerUi;

    //�ڷ�ƾ����
    Coroutine harvestingRoutine = null; //�����ڷ�ƾ
    Coroutine decreaseRoutine = null; //�����ڷ�ƾ

    //��Ÿ�� ������
    [Header("������ �ӵ� ����")]
    [SerializeField] float currentProgress = 0f; // ���������
    [SerializeField] float harvestTime = 2f; // ������ ��Ÿ��
    [SerializeField] float decreaseSpeed = 0.5f; // �پ��� �ӵ�
    List<float> checkPoints = new List<float>(); // üũ����Ʈ ���

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        //üũ����Ʈ
        checkPoints.Add(harvestTime / 3f);
        checkPoints.Add(harvestTime / 3f * 2f);
    }



    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("������!");
        StartHarvest();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log("���Ҵ�!");
        if (this.gameObject.activeSelf == true)
        {
            StopHarvest();
        }
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

        this.gameObject.SetActive(false);

        Debug.Log("ä�� �Ϸ�!");
    }
}

    






