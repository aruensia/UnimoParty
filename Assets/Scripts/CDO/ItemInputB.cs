using UnityEngine;
using UnityEngine.InputSystem;


//secondaryButton[������ XR ��Ʈ�ѷ�] = B ��ư
//primaryButton[������ XR ��Ʈ�ѷ�] = A ��ư
public class ItemInputB : MonoBehaviour, IFreeze
{
    [SerializeField] InputActionReference BInputActionReference;

    [SerializeField] InputActionReference triggerInputActionReference;

    private void OnEnable()
    {
        BInputActionReference.action.Enable();
        BInputActionReference.action.performed += ControllerB;

        triggerInputActionReference.action.performed += OnTriggerPressed;
        triggerInputActionReference.action.canceled += OnTriggerReleased;
    }

    private void OnDisable()
    {
        BInputActionReference.action.performed -= ControllerB;
        BInputActionReference.action.Disable();

        triggerInputActionReference.action.performed -= OnTriggerPressed;
        triggerInputActionReference.action.canceled -= OnTriggerReleased;
    }

    //Ʈ���� ��
    private void OnTriggerPressed(InputAction.CallbackContext context)
    { 
        //������ ���


    }

    //Ʈ���� ��
    void OnTriggerReleased(InputAction.CallbackContext context)
    {



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
