using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


//1. �����۵������� //������1, ������2, ������3 
//2. �����۵�(�������̸��� ��� ����Ѵ�)
//3. ť�� �����۵��� ���� 
//4. ���� ������ = ť�� ù��°
//5. �������Ǳ�� ��������use�Լ��� ���
//6. ������ ����Լ��� ��Ʈ�ѷ��� �Լ� ���
//7. ��ü�� ù��°�������� �ٽ� ť������


//secondaryButton[������ XR ��Ʈ�ѷ�] = B ��ư
//primaryButton[������ XR ��Ʈ�ѷ�] = A ��ư
public class ItemInputB : MonoBehaviourPunCallbacks, IFreeze
{
    [SerializeField] InputActionReference bInputActionReference; //xr b

    [SerializeField] InputActionReference triggerInputActionReference; //Ʈ����

    Transform firepos; //���� ���κ�

    [SerializeField] Transform rightController; //������ ��Ʈ�ѷ�

    int grenadePower = 2;

    public Queue<string> itemQueue = new Queue<string>();

    GameObject currentItem = null;
    GameObject newItem;

    //��ũ��Ʈ
    [SerializeField] XrControllerMgr xrControllerMgr;


    //������ ��ġ (�ڽİ�ü)
    Transform invenItem1;
    Transform invenItem2;
    Transform invenItem3;

    //������ ���� ������
    GameObject item1;
    GameObject item2;
    GameObject item3;


    void OnEnableItem()
    {
        var modelGun2 = rightController.gameObject.GetComponent<ActionBasedController>().model;


        invenItem1 = modelGun2.GetChild(2);
        invenItem2 = modelGun2.GetChild(3);
        invenItem3 = modelGun2.GetChild(4);

    }

    void InvenCreateItem()
    {

        if (item1 != null)
        {
            PhotonNetwork.Destroy(item1);
        }
        if (item2 != null)
        {
            PhotonNetwork.Destroy(item2);
        }
        if (item3 != null)
        {
            PhotonNetwork.Destroy(item3);
        }




        string[] arr = itemQueue.ToArray();
        if (arr.Length == 0)
        {
            return;
        }

        item1 = PhotonNetwork.Instantiate(arr[0], transform.position, Quaternion.identity);
        item1.transform.SetParent(invenItem1, false);
        item1.transform.localPosition = Vector3.zero;
        item1.GetComponent<Rigidbody>().useGravity = false;
        item1.transform.localScale = Vector3.one * 0.5f;

        if (arr.Length == 1)
        {
            return;
        }

        item2 = PhotonNetwork.Instantiate(arr[1], transform.position, Quaternion.identity);
        item2.transform.SetParent(invenItem2, false);
        item2.transform.localPosition = Vector3.zero;
        item2.GetComponent<Rigidbody>().useGravity = false;
        item2.transform.localScale = Vector3.one * 0.5f;

        if (arr.Length == 2)
        {
            return;
        }

        item3 = PhotonNetwork.Instantiate(arr[2], transform.position, Quaternion.identity);
        item3.transform.SetParent(invenItem3, false);
        item3.transform.localPosition = Vector3.zero;
        item3.GetComponent<Rigidbody>().useGravity = false;
        item3.transform.localScale = Vector3.one * 0.5f;



    }

    public override void OnEnable()
    {
        base.OnEnable();
        bInputActionReference.action.Enable();
        bInputActionReference.action.performed += ControllerB;

        triggerInputActionReference.action.performed += OnTriggerPressed;


        //������ ����
        itemQueue = xrControllerMgr.publicitemQueue;

    }

    public override void OnDisable()
    {
        base.OnDisable();
        bInputActionReference.action.performed -= ControllerB;
        bInputActionReference.action.Disable();

        triggerInputActionReference.action.performed -= OnTriggerPressed;
    }

    //Ʈ���� ��
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        //������ ���
        if (currentItem.TryGetComponent<IItemUse>(out var itemUse))
        {
            if (itemUse.Use(firepos, grenadePower) == false)
            {
                return;
            }

        }



        //�����ۻ���ϸ� ť ����
        itemQueue.Dequeue();
        //ť�� ����� �������� ������
        if (itemQueue.Count <= 0)
        {
            xrControllerMgr.IsItemObj(false);
            return;
        }
        string nextItem = itemQueue.Peek();
        currentItem = ItemCreate(nextItem);
    }



    GameObject ItemCreate(string ItemPrefabName)
    {
        if(ItemPrefabName == "TestItem1")
        {
            ItemPrefabName = "TestItem";
        }
        firepos = rightController.gameObject.GetComponentInChildren<ActionBasedController>().model.GetChild(0).transform;
        newItem = PhotonNetwork.Instantiate(ItemPrefabName, firepos.position, Quaternion.identity);
        newItem.transform.rotation = rightController.transform.rotation;
        newItem.transform.parent = firepos;
        newItem.gameObject.GetComponent<Rigidbody>().useGravity = false;

        //OnEnableInvenItem();
        InvenCreateItem();
        return newItem;
    }

    //������ ����
    public void StateItem(bool isItemInputB)
    {
        if (isItemInputB == true)
        {
            OnEnableItem();
            //if (itemQueue.Count == 0)
            //{
            //    itemQueue.Enqueue("Boomprefab");
            //    itemQueue.Enqueue("PotionPrefab1");
            //    itemQueue.Enqueue("TestItem");
            //}
            if (currentItem == null)
            {
                string QueueItem = itemQueue.Peek();
                currentItem = ItemCreate(QueueItem);
            }

        }
        else if (isItemInputB == false)
        {
            if (currentItem != null)
            {
                PhotonNetwork.Destroy(currentItem);
                currentItem = null;
            }
        }
    }

    //������ ��ü b
    private void ControllerB(InputAction.CallbackContext context)
    {
        //Ȥ�� ���� �����ڵ�
        if (itemQueue.Count <= 0)
        {
            xrControllerMgr.IsItemObj(false);
            return;
        }

        Debug.Log("��Ʈ�� b��ư, ������ ��ü");
        string oldItem = itemQueue.Dequeue();
        itemQueue.Enqueue(oldItem);

        PhotonNetwork.Destroy(currentItem);

        if (itemQueue.Count <= 0)
        {
            xrControllerMgr.IsItemObj(false);
            return;
        }
        string nextItem = itemQueue.Peek();
        currentItem = ItemCreate(nextItem);


    }

    public void IsItemQueueCountZero()
    {
        //ť�� ����� �������� ������
        if (itemQueue.Count <= 0)
        {
            xrControllerMgr.IsItemObj(false);
            return;
        }
    }

    public void AddQueueItem(string ItemName)
    {
        itemQueue.Enqueue(ItemName);
        InvenCreateItem();
    }

    public void Freeze(bool IsFreeze)
    {
        throw new System.NotImplementedException();
    }
}
