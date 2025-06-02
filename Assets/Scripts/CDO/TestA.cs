using UnityEngine;
using UnityEngine.InputSystem;


//secondaryButton[������ XR ��Ʈ�ѷ�] = B ��ư
//primaryButton[������ XR ��Ʈ�ѷ�] = A ��ư
public class TestA : MonoBehaviour
{
    [SerializeField]
    private InputActionReference AInputActionReference;
    private InputActionReference BInputActionReference;

    private void OnEnable()
    {
        AInputActionReference.action.Enable();
        AInputActionReference.action.performed += ControllerA;

        BInputActionReference.action.Enable();
        BInputActionReference.action.performed += ControllerB;
    }

    private void OnDisable()
    {
        AInputActionReference.action.performed -= ControllerA;
        AInputActionReference.action.Disable();

        BInputActionReference.action.performed -= ControllerB;
        BInputActionReference.action.Disable();
    }

    //������, ä���� ��ü a
    private void ControllerA(InputAction.CallbackContext context)
    {
        Debug.Log("��Ʈ�� a��ư");
    }

    //������ ��ü b
    private void ControllerB(InputAction.CallbackContext context)
    {
        Debug.Log("��Ʈ�� b��ư");
    }









}
