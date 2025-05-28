using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHarvest : MonoBehaviourPunCallbacks
{
    [SerializeField] XRRayInteractor RayInteractor;

    [SerializeField] int SpiritPoint = 0; //
    public int spiritPoint { get { return SpiritPoint; } set { if (value < 0) { Debug.Log("����������"); value = 0; } SpiritPoint = value; } }

    [SerializeField] private InputActionReference activateAction;
    Flower flower;

    [Header("Haptic ���� ����")]
    [SerializeField] float HapticAmplitude;
    [SerializeField] float HapticDuraiton;

    //�ݹ��� OnEnable �ȴ�
    //player�� �Ȼ�����ϱ� awake, start�� ������ ��� 
    //�ݹ� �����ѰǾ�����
    public override void OnEnable()
    {
        base.OnEnable();
        if (photonView.IsMine)
        {            
            activateAction.action.performed += OnTriggerPressed;
            activateAction.action.canceled += OnTriggerReleased;
        }        
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (photonView.IsMine)
        {
            activateAction.action.performed -= OnTriggerPressed;
            activateAction.action.canceled -= OnTriggerReleased;
        }
    }

    //�����ڵ� ��ߴ� flower������������
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        flower = null;

        if (RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {

            if (!hit.collider.TryGetComponent<Flower>(out flower))
            {
                return;
            }

            //�� ������ �ڵ�
            if (flower != null)
            {
                //����
                //*�ν�����â���� �ٸ����� Haptic 0���� �ٿ��ߴ�
                //�׷��� �ָ� ������
                RayInteractor.xrController.SendHapticImpulse(HapticAmplitude, HapticDuraiton);

                flower.Init(this);
                flower.StartHarvest();
                Debug.Log("Flower ��Ȯ ����!");
            }
        }
    }

    void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger ��");
        if (flower != null && flower.gameObject.activeSelf == true)
        {
            flower.StopHarvest();
        }
    }


    //���� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Manager.Instance.observer.DeliveryFairy();
        }
    }

}
