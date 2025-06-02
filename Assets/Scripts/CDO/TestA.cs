using UnityEngine;
using UnityEngine.InputSystem;


//secondaryButton[������ XR ��Ʈ�ѷ�] = B ��ư
//primaryButton[������ XR ��Ʈ�ѷ�] = A ��ư
public class TestA : MonoBehaviour
{
    [SerializeField]
    private InputActionReference BInputActionReference;

    private void OnEnable()
    {
        BInputActionReference.action.Enable();
        BInputActionReference.action.performed += ControllerB;
    }

    private void OnDisable()
    {
        BInputActionReference.action.performed -= ControllerB;
        BInputActionReference.action.Disable();
    }



    //������ ��ü b
    private void ControllerB(InputAction.CallbackContext context)
    {
        Debug.Log("��Ʈ�� b��ư");
    }









}
