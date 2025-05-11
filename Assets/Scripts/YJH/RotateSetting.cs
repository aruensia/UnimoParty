using UnityEngine;

public class RotateSetting : MonoBehaviour
{
    public Transform cameraTF;         
    public Transform spaceShip;            
    public float rotateSpeed = 60f;        

    private bool isRotating = false;
    private int rotateDirection = 0;
    private Vector3 baseForward;

    void Start()
    {
        baseForward = FlatDirection(spaceShip.forward); // �������� spaceShip�� ����
    }

    void Update()
    {
        Vector3 currentCameraForward = FlatDirection(cameraTF.forward);
        float angle = Vector3.SignedAngle(baseForward, currentCameraForward, Vector3.up);

        if (!isRotating)
        {
            if (angle >= 60)
            {
                Debug.Log("������ ȸ��");
                rotateDirection = 1;
                isRotating = true;
            }
            else if (angle <= -60)
            {
                Debug.Log("���� ȸ��");
                rotateDirection = -1;
                isRotating = true;
            }
        }
        else
        {
            if ((rotateDirection == 1 && angle >= 60) || (rotateDirection == -1 && angle <= -60))
            {
                float rotationAmount = rotateSpeed * Time.deltaTime * rotateDirection;

                transform.Rotate(0f, rotationAmount, 0f);
                spaceShip.Rotate(0f, rotationAmount, 0f);
            }
            else
            {
                Debug.Log("ȸ�� ����");
                isRotating = false;
                baseForward = FlatDirection(spaceShip.forward);
            }
        }
    }

    Vector3 FlatDirection(Vector3 dir)
    {
        Debug.Log("���� ����");
        dir.y = 0f;
        return dir.normalized;
    }
}