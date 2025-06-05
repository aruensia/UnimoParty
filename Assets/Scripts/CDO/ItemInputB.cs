using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


//secondaryButton[������ XR ��Ʈ�ѷ�] = B ��ư
//primaryButton[������ XR ��Ʈ�ѷ�] = A ��ư
public class ItemInputB : MonoBehaviourPunCallbacks, IFreeze
{
    [SerializeField] InputActionReference BInputActionReference; //xr b

    [SerializeField] InputActionReference triggerInputActionReference; //Ʈ����

    Transform firepos; //���� ���κ�

    [SerializeField] Transform rightController; //������ ��Ʈ�ѷ�

    GameObject Item1 = null;

    int grenadePower = 5;



    public override void OnEnable()
    {
        base.OnEnable();
        BInputActionReference.action.Enable();
        BInputActionReference.action.performed += ControllerB;

        triggerInputActionReference.action.performed += OnTriggerPressed;
        triggerInputActionReference.action.canceled += OnTriggerReleased;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        BInputActionReference.action.performed -= ControllerB;
        BInputActionReference.action.Disable();

        triggerInputActionReference.action.performed -= OnTriggerPressed;
        triggerInputActionReference.action.canceled -= OnTriggerReleased;
    }

    //Ʈ���� ��
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        //photonView.RPC("Item1Rpc",RpcTarget.All);
        //������ ���
      
        Rigidbody rb = Item1.gameObject.GetComponent<Rigidbody>();
        Vector3 throwDirection = firepos.transform.forward + firepos.transform.up;
        Item1.transform.parent = null;
        rb.useGravity = true;
        rb.AddForce(throwDirection * grenadePower, ForceMode.VelocityChange);

    }

   


    //Ʈ���� ��
    void OnTriggerReleased(InputAction.CallbackContext context)
    {



    }
    public void StateItem(bool isItemInputB)
    {
        if (isItemInputB == true)
        {
            firepos = rightController.gameObject.GetComponentInChildren<ActionBasedController>().model.GetChild(0).transform;
            Item1 = PhotonNetwork.Instantiate("Boomprefab", firepos.position, Quaternion.identity);
            Item1.transform.parent = firepos.transform;
            Item1.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        else if(isItemInputB == false)
        {
            PhotonNetwork.Destroy(Item1);
        }
    }

    //������ ��ü b
    private void ControllerB(InputAction.CallbackContext context)
    {
        Debug.Log("��Ʈ�� b��ư, ������ ��ü");
    }

    public void Freeze(bool IsFreeze)
    {
        throw new System.NotImplementedException();
    }
}
