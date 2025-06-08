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

    int grenadePower = 5;

    public Queue<string> itemQueue = new Queue<string>();

    GameObject currentItem = null;
    GameObject newItem;


    GameObject ItemAdd(string ItemPrefabName)
    {
        firepos = rightController.gameObject.GetComponentInChildren<ActionBasedController>().model.GetChild(0).transform;
        newItem = PhotonNetwork.Instantiate(ItemPrefabName, firepos.position, Quaternion.identity);
        newItem.transform.parent = firepos.transform;
        newItem.gameObject.GetComponent<Rigidbody>().useGravity = false;

        return newItem;
    }

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

        Rigidbody rb = currentItem.gameObject.GetComponent<Rigidbody>();
        Vector3 throwDirection = firepos.transform.forward + firepos.transform.up;
        currentItem.transform.parent = null;
        rb.useGravity = true;
        rb.AddForce(throwDirection * grenadePower, ForceMode.VelocityChange);

        //�����ۻ���ϸ� ť ����
        itemQueue.Dequeue();

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
                currentItem = ItemAdd(QueueItem);
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
        //StartCoroutine(NetworkDestroyWait());

        Debug.Log("��Ʈ�� b��ư, ������ ��ü");
        Debug.Log("itemQueue.count = " + itemQueue.Count);
        string oldItem = itemQueue.Dequeue();
        itemQueue.Enqueue(oldItem);

        PhotonNetwork.Destroy(currentItem);

        string nextItem = itemQueue.Peek();
        Debug.Log("ť�����������̸� = "+ itemQueue.Peek());
        currentItem = ItemAdd(nextItem);


    }


    IEnumerator NetworkDestroyWait()
    {
        Debug.Log("��Ʈ�� b��ư, ������ ��ü");
        Debug.Log("itemQueue.count = " + itemQueue.Count);
        string oldItem = itemQueue.Dequeue();
        itemQueue.Enqueue(oldItem);

        PhotonNetwork.Destroy(currentItem);

        yield return new WaitUntil(() => currentItem == null);

        string nextItem = itemQueue.Peek();
        if(nextItem == "")
        {
            Debug.Log("ť���������̺������");
        }
        currentItem = ItemAdd(nextItem);
    }


    public void Freeze(bool IsFreeze)
    {
        throw new System.NotImplementedException();
    }
}
