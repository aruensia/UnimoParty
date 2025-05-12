using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateSetting : MonoBehaviour
{
    public Transform cameraTF;
    public Transform spaceShip;
    public float rotateSpeed = 60f;

    private bool isRotating = false;
    private int rotateDirection = 0;
    private float baseYAngle;

    void Start()
    {
        // ������ �׻� spaceShip�� ���� Y ȸ��
        baseYAngle = spaceShip.eulerAngles.y;
    }

    void Update()
    {
        float currentYAngle = cameraTF.eulerAngles.y;

        // ���� ����(spaceShip)�� HMD ������ Y�� ȸ�� ���� ��� (-180~180)
        float deltaAngle = Mathf.DeltaAngle(baseYAngle, currentYAngle);

        if (!isRotating)
        {
            if (deltaAngle >= 60f)
            {
                Debug.Log("������ ȸ�� ����");
                rotateDirection = 1;
                isRotating = true;
            }
            else if (deltaAngle <= -60f)
            {
                Debug.Log("���� ȸ�� ����");
                rotateDirection = -1;
                isRotating = true;
            }
        }
        else
        {
            // ��� ȸ�� ���� ���� (��� ���� �ٱ��� ���� ����)
            if ((rotateDirection == 1 && deltaAngle >= 60f) ||
                (rotateDirection == -1 && deltaAngle <= -60f))
            {
                float rotationAmount = rotateSpeed * Time.deltaTime * rotateDirection;
                transform.Rotate(0f, rotationAmount, 0f);
                spaceShip.Rotate(0f, rotationAmount, 0f);
            }
            else
            {
                Debug.Log("ȸ�� ���� + ���� ����");
                isRotating = false;

                // ������ ���� spaceShip �������� ����
                baseYAngle = spaceShip.eulerAngles.y;
            }
        }
    }

    //public Transform cameraTF;
    //public Transform spaceShip;            
    //public float rotateSpeed = 60f;        

    //private bool isRotating = false;
    //private int rotateDirection = 0;
    //private Vector3 baseForward;

    //void Start()
    //{
    //    baseForward = FlatDirection(spaceShip.forward); // �������� spaceShip�� ����
    //}

    //void Update()
    //{
    //    float angle = cameraTF.eulerAngles.y;

    //    // ���� ȸ�� ������ baseForward ������ �ƴ϶� ���밪 �������� ó��
    //    float relativeAngle = Mathf.DeltaAngle(baseForward.y, angle); // -180~180

    //    if (!isRotating)
    //    {
    //        if (relativeAngle >= 60f)
    //        {
    //            Debug.Log("������ ȸ�� ����");
    //            rotateDirection = 1;
    //            isRotating = true;
    //        }
    //        else if (relativeAngle <= -60f)
    //        {
    //            Debug.Log("���� ȸ�� ����");
    //            rotateDirection = -1;
    //            isRotating = true;
    //        }
    //    }
    //    else
    //    {
    //        if ((rotateDirection == 1 && relativeAngle >= 60f) || (rotateDirection == -1 && relativeAngle <= -60f))
    //        {
    //            float rotationAmount = rotateSpeed * Time.deltaTime * rotateDirection;
    //            transform.Rotate(0f, rotationAmount, 0f);
    //            spaceShip.Rotate(0f, rotationAmount, 0f);
    //        }
    //        else
    //        {
    //            Debug.Log("ȸ�� ����");
    //            isRotating = false;
    //            baseForward = spaceShip.eulerAngles; // ���ο� ���� Y���� ����
    //        }
    //    }
    //}

    //Vector3 FlatDirection(Vector3 dir)
    //{
    //    Debug.Log("���� ����");
    //    dir.y = 0f;
    //    return dir.normalized;
    //}
}