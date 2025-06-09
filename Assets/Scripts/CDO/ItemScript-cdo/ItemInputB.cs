using Photon.Pun;
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

    public override void OnEnable()
    {
        base.OnEnable();
        bInputActionReference.action.Enable();
        bInputActionReference.action.performed += ControllerB;

        triggerInputActionReference.action.performed += OnTriggerPressed;
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
            itemUse.Use(firepos, grenadePower);
        }

        //�����ۻ���ϸ� ť ����
        itemQueue.Dequeue();
        string nextItem = itemQueue.Peek();
        currentItem = ItemCreate(nextItem);

    }

    GameObject ItemCreate(string ItemPrefabName)
    {
        firepos = rightController.gameObject.GetComponentInChildren<ActionBasedController>().model.GetChild(0).transform;
        newItem = PhotonNetwork.Instantiate(ItemPrefabName, firepos.position, Quaternion.identity);
        newItem.transform.parent = firepos;
        newItem.gameObject.GetComponent<Rigidbody>().useGravity = false;

        return newItem;
    }

    //������ ����
    public void StateItem(bool isItemInputB)
    {
        if (isItemInputB == true)
        {
            if (itemQueue.Count == 0)
            {
                itemQueue.Enqueue("Boomprefab");
                itemQueue.Enqueue("PotionPrefab1");
            }
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
        if (itemQueue.Count <= 0)
        {
            return;
        }
        Debug.Log("��Ʈ�� b��ư, ������ ��ü");
        Debug.Log("itemQueue.count = " + itemQueue.Count);
        string oldItem = itemQueue.Dequeue();
        itemQueue.Enqueue(oldItem);

        PhotonNetwork.Destroy(currentItem);

        if (itemQueue.Count <= 0)
        {
            return;
        }
        Debug.Log("ť�����������̸� = " + itemQueue.Peek());
        string nextItem = itemQueue.Peek();
        currentItem = ItemCreate(nextItem);


    }

    public void AddQueueItem(string ItemName)
    {
        itemQueue.Enqueue(ItemName);
    }

    public void Freeze(bool IsFreeze)
    {
        throw new System.NotImplementedException();
    }
}
