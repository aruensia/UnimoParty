using UnityEngine;

public class RotateSetting : MonoBehaviour
{
    public Transform cameraTF;          // HMD ī�޶�
    public Transform spaceShip;         // ������ �Ǵ� ������Ʈ
    public float rotateSpeed = 60f;     // �ʴ� ȸ�� �ӵ� (deg/sec)

    private bool isRotating = false;
    private int rotateDirection = 0;

    void Update()
    {
        Vector3 forward = FlatDirection(spaceShip.forward);
        Vector3 camDir = FlatDirection(cameraTF.forward);
        float angle = Vector3.SignedAngle(forward, camDir, Vector3.up);

        if (!isRotating)
        {
            if (angle > 60f)
            {
                rotateDirection = 1;
                isRotating = true;
                Debug.Log("������ ȸ�� ����");
            }
            else if (angle < -60f)
            {
                rotateDirection = -1;
                isRotating = true;
                Debug.Log("���� ȸ�� ����");
            }
        }
        else
        {
            // ���� ���̸� ��� ȸ��
            bool stillOutside =
                (rotateDirection == 1 && angle > 60f) ||
                (rotateDirection == -1 && angle < -60f);

            if (stillOutside)
            {
                float rotAmount = rotateSpeed * Time.deltaTime * rotateDirection;

                transform.Rotate(0f, rotAmount, 0f);
                spaceShip.Rotate(0f, rotAmount, 0f);
                cameraTF.Rotate(0f, rotAmount, 0f); 
            }
            else
            {
                isRotating = false;
                Debug.Log("ȸ�� ����");
            }
        }
    }

    Vector3 FlatDirection(Vector3 dir)
    {
        dir.y = 0f;
        return dir.normalized;
    }
}
