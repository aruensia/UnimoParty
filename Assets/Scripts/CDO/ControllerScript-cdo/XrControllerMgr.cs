using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XrControllerMgr : MonoBehaviourPunCallbacks
{
    //�������� ��ǲ�׼�(��ǲ �����̸Ӹ�)
    [SerializeField] InputActionReference AInputActionReference;
    [SerializeField] ActionBasedController rightController;

    //���ӿ�����Ʈ Ȱ��ȭ ��Ȱ��ȭ��
    [Header("�� ��ũ��Ʈ ���ִ� ������Ʈ")]
    [SerializeField] GameObject handHarvestObj;
    [SerializeField] GameObject ItemObj;
    [SerializeField] ItemInputB itemInputB;
    [SerializeField] HandHarvest handHarvest;

    [Header("�� ������ (��)")]
    [SerializeField] GameObject harvestGun;
    [SerializeField] GameObject ItemGun;


    bool isItemController = false;   //ó���� ä���� ����
    GameObject RController;
    Transform lasetRcontroller; //temp

    //�� ������ �ִ� ������
    public Queue<string> publicitemQueue = new Queue<string>();

    private void Start()
    {
        RController = Instantiate(ItemGun, rightController.gameObject.transform);
        RController.SetActive(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AInputActionReference.action.Enable();
        AInputActionReference.action.performed += ControllerA;

        //�ӽ� ������ �߰�
        ItemQueueAdd("start");

    }

    public override void OnDisable()
    {
        base.OnDisable();
        AInputActionReference.action.performed -= ControllerA;
        AInputActionReference.action.Disable();

    }

    //������, ä���� ��ü a
    private void ControllerA(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
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

    }

    public void IsItemObj(bool isItem)
    {
        ItemQueueAdd("start");
        ItemObj.SetActive(isItem);
        handHarvestObj.SetActive(!isItem);
        if (isItem == true)
        {
            lasetRcontroller = rightController.model;
            rightController.model.gameObject.SetActive(false);
            RController.SetActive(true);
            rightController.model = RController.transform;
            rightController.modelPrefab = RController.transform;
        }
        else if (isItem == false)
        {
            rightController.model = lasetRcontroller;
            rightController.model.gameObject.SetActive(true);
            RController.SetActive(false);
            rightController.model = lasetRcontroller;
            rightController.modelPrefab = lasetRcontroller;

        }


    }

    void ItemQueueAdd(string item)
    {
        if (publicitemQueue.Count == 0)
        {
            publicitemQueue.Enqueue("Boomprefab");
            publicitemQueue.Enqueue("PotionPrefab1");
            publicitemQueue.Enqueue("TestItem1");
        }

        if(item == "start")
        {
            return;
        }

        publicitemQueue.Enqueue(item);
        var newItem = publicitemQueue.Peek();
        itemInputB.AddQueueItem(newItem);
        handHarvest.AddQueueItem(newItem);
    }
  




}
