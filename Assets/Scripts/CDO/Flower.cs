using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flower : MonoBehaviour
{
    XRGrabInteractable grabInteractable;

    //�ڷ�ƾ����
    Coroutine harvestingRoutine = null; //�����ڷ�ƾ
    Coroutine decreaseRoutine = null; //�����ڷ�ƾ

    //��Ÿ�� ������
    [Header("������ �ӵ� ����")]
    [SerializeField] float currentProgress = 0f; // ���������
    [SerializeField] float harvestTime = 2f; // ������ ��Ÿ��
    [SerializeField] float decreaseSpeed = 0.5f; // �پ��� �ӵ�


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("������!");
        StartHarvest();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log("���Ҵ�!");
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
            yield return null;
        }

        CompleteHarvest();
    }

    //��Ȯ ����(�߰� ��������)
    private IEnumerator DecreaseCoroutine()
    {
        while (currentProgress > 0f)
        {
            currentProgress -= Time.deltaTime * decreaseSpeed;
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

    






