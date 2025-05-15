using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flower : MonoBehaviour
{

    [SerializeField] FlowerUi flowerUi;

    //�ڷ�ƾ����
    Coroutine harvestingRoutine = null; //�����ڷ�ƾ
    Coroutine decreaseRoutine = null; //�����ڷ�ƾ

    //��Ÿ�� ������
    [Header("������ �ӵ� ����")]
    [SerializeField] float currentProgress = 0f; // ���������
    [SerializeField] float harvestTime = 3f; // ������ ��Ÿ��
    [SerializeField] float decreaseSpeed = 0.5f; // �پ��� �ӵ�
    List<float> checkPoints = new List<float>(); // üũ����Ʈ ���

    //��ǲ�׼�
    [Header("InputSystem")]
    [SerializeField] private InputActionReference activateAction;

    public int SpiritVisagePoint;

    //�׽�Ʈ
    HandHarvest handHarvest;

    public float HarvestTime => harvestTime;

    private void Start()
    {
        //üũ����Ʈ
        checkPoints.Add(harvestTime / 3f);
        checkPoints.Add(harvestTime / 3f * 2f);

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
    void CompleteHarvest()
    {
        harvestingRoutine = null;
        currentProgress = 0f;

        if (handHarvest != null)
        {
            handHarvest.spiritPoint++;
            Debug.Log("spiritPoint ����!");
        }
        else
        {
            Debug.LogWarning("handHarvest�� �������� �ʾҽ��ϴ�.");
        }

        this.gameObject.SetActive(false);

        Debug.Log("ä�� �Ϸ�!");
    }

    public void Init(HandHarvest handHarvest)
    {
        this.handHarvest = handHarvest;
    }


    private void ResetFlower()
    {
        currentProgress = 0;
    }

}

    






