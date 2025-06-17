using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Flower : MonoBehaviourPun
{
    private InGameDataController controller;
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
    HandHarvest handHarvest;
    public float HarvestTime => harvestTime;


    [Header("FairyDataType Value��")]
    [SerializeField] int FairyDataType1;
    [SerializeField] int FairyDataType2;
    [SerializeField] int FairyDataType3;

    FairyType fairyType;

    private LaycockSP LaycockSpawner;





    private void Start()
    {
        controller = GameObject.Find("InGameDataController").GetComponent<InGameDataController>();
        //üũ����Ʈ
        checkPoints.Add(harvestTime / 3f);
        checkPoints.Add(harvestTime / 3f * 2f);

        ChangeValueFairyDataType(FairyDataType1, FairyDataType2, FairyDataType3);
    }


    void ChangeValueFairyDataType(int value1, int value2, int value3)
    {
        fairyType.FairyDataType_1 = value1;
        fairyType.FairyDataType_2 = value2;
        fairyType.FairyDataType_3 = value3;
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
            float progressChange = decreaseSpeed * 0.01f;
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
        LaycockSpawner.SpawnLaycock(gameObject.transform);
        harvestingRoutine = null;
        currentProgress = 0f;

        if (handHarvest != null)
        {
            handHarvest.SpiritPoint++;
            Debug.Log("spiritPoint ����!");
        }
        else
        {
            Debug.LogWarning("handHarvest�� �������� �ʾҽ��ϴ�.");
        }

        if (controller.IsTestMode == false)
        {
            //�Ŵ����� ��ȣ�ۿ�
            Manager.Instance.observer.GetFairy(fairyType);
        }

        this.gameObject.SetActive(false);

        photonView.RPC("FlowerSetAcive", RpcTarget.Others,false);

        Debug.Log("ä�� �Ϸ�!");
    }

    public void Init(HandHarvest handHarvest)
    {
        this.handHarvest = handHarvest;
    }

    [PunRPC]
    void FlowerSetAcive(bool isTrue)
    {
        this.gameObject.SetActive(isTrue);
    }

    private void ResetFlower()
    {
        currentProgress = 0;
    }

}








