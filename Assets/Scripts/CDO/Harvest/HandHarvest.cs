using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHarvest : MonoBehaviourPunCallbacks, IFreeze
{
    [SerializeField] GameObject cameraOffset;

    [SerializeField] XRRayInteractor rayInteractor;

    [SerializeField] int spiritPoint = 0; //
    public int SpiritPoint { get { return spiritPoint; } set { if (value < 0) { Debug.Log("����������"); value = 0; } spiritPoint = value; } }

    [SerializeField] private InputActionReference activateAction;
    Flower flower;

    [Header("Haptic ���� ����")]
    [SerializeField] float hapticAmplitude;
    [SerializeField] float hapticDuraiton;
    int gap = 10;

    //�ݹ��� OnEnable �ȴ�
    //player�� �Ȼ�����ϱ� awake, start�� ������ ��� 
    //�ݹ� �����ѰǾ�����
    private void Start()
    {
        if (!photonView.IsMine)
        {
            cameraOffset.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FlowerDistanceCor());
        }
    }

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

    public void Freeze(bool isFreeze)
    {
        Debug.Log("HandHarvest�� Freeze������");
        if (isFreeze)
        {
            activateAction.action.performed -= OnTriggerPressed;
            activateAction.action.canceled -= OnTriggerReleased;
        }
        else if (!isFreeze)
        {
            activateAction.action.performed += OnTriggerPressed;
            activateAction.action.canceled += OnTriggerReleased;
        }

    }


    //�����ڵ� ��ߴ� flower������������
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        flower = null;
        IsHarvest(false);

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
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
                //rayInteractor.xrController.SendHapticImpulse(hapticAmplitude, hapticDuraiton);

                IsHarvest(true);

                flower.Init(this);
                flower.StartHarvest();
                Debug.Log("Flower ��Ȯ ����!");




            }
        }
    }
    Coroutine cor;
    public void IsHarvest(bool isHarvest)
    {
        if (isHarvest)
        {
            cor = StartCoroutine(IsHarvestCor());
            //rayInteractor.xrController.SendHapticImpulse(hapticAmplitude, hapticDuraiton);
        }
        else if (!isHarvest)
        {
            if (cor != null)
            {
                StopCoroutine(cor);
            }
            rayInteractor.xrController.SendHapticImpulse(0, 0);
        }

    }

    IEnumerator IsHarvestCor()
    {
        while (true)
        {
            rayInteractor.xrController.SendHapticImpulse(hapticAmplitude, hapticDuraiton);
            yield return new WaitForSeconds(hapticDuraiton);
        }

    }



    private void Update()
    {
        if (flower != null)
        {
            if (Vector3.Distance(flower.gameObject.transform.position, rayInteractor.transform.position) >= 5)
            {
                if (flower != null && flower.gameObject.activeSelf == true)
                {
                    IsHarvest(false);
                    flower.StopHarvest();

                }
               
            }

            if (flower.gameObject.activeSelf == false)
            {
                IsHarvest(false);

            }

        }
    }


    IEnumerator FlowerDistanceCor()
    {
        while (true)
        {
            if (flower != null)
            {
                if (Vector3.Distance(flower.gameObject.transform.position, rayInteractor.transform.position) >= 5)
                {
                    if (flower != null && flower.gameObject.activeSelf == true)
                    {
                        IsHarvest(false);
                        flower.StopHarvest();

                    }

                }

                if (flower.gameObject.activeSelf == false)
                {
                    IsHarvest(false);

                }

            }


            yield return null;
        }
    }


    void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger ��");
        if (flower != null && flower.gameObject.activeSelf == true)
        {
            IsHarvest(false);
            flower.StopHarvest();
            flower = null;
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
