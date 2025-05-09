using UnityEngine;

public class RotateSetting : MonoBehaviour
{
    public Transform hmdTransform;         // HMD ī�޶�
    public Transform spaceShip;            // ������ & ���̽�ƽ ���� ������Ʈ
    public float thresholdAngle = 60f;     // Ʈ���� ����
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
        Vector3 currentHMDForward = FlatDirection(hmdTransform.forward);
        float angle = Vector3.SignedAngle(baseForward, currentHMDForward, Vector3.up);

        if (!isRotating)
        {
            if (angle > thresholdAngle)
            {
                rotateDirection = 1;
                isRotating = true;
            }
            else if (angle < -thresholdAngle)
            {
                rotateDirection = -1;
                isRotating = true;
            }
        }
        else
        {
            // HMD�� ��� ȸ�� ���� �ۿ� �ִ� ���� ȸ�� ����
            if ((rotateDirection == 1 && angle > thresholdAngle) ||
                (rotateDirection == -1 && angle < -thresholdAngle))
            {
                float rotationAmount = rotateSpeed * Time.deltaTime * rotateDirection;

                // �������� spaceShip�� ���� ȸ��
                transform.Rotate(0f, rotationAmount, 0f);
                spaceShip.Rotate(0f, rotationAmount, 0f);
            }
            else
            {
                // HMD�� �ٽ� ���� ���� �ȿ� ���� �� ȸ�� ���� + ���� ����
                isRotating = false;
                baseForward = FlatDirection(spaceShip.forward); // �� �������� ���� spaceShip�� ����
            }
        }
    }

    Vector3 FlatDirection(Vector3 dir)
    {
        dir.y = 0f;
        return dir.normalized;
    }
}