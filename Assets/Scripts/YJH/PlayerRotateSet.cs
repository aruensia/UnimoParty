using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateSet : MonoBehaviour
{
    public Transform cameraTF;     // HMD ī�޶�
    public Transform spaceShip;    // ȸ�� ���� ������Ʈ
    public Transform leftLimit;    // -60�� ��ġ
    public Transform rightLimit;   // +60�� ��ġ

    [Header("Settings")]
    public float rotateSpeed = 60f;  // �ʴ� ȸ�� �ӵ�

    private bool isRotating = false;
    private int rotateDirection = 0;

    void Update()
    {
        Vector3 camDir = FlatDirection(cameraTF.forward);
        Vector3 toLeft = FlatDirection(leftLimit.position - spaceShip.position);
        Vector3 toRight = FlatDirection(rightLimit.position - spaceShip.position);

        float leftAngle = Vector3.SignedAngle(toLeft, camDir, Vector3.up);
        float rightAngle = Vector3.SignedAngle(camDir, toRight, Vector3.up);

        // �þ߰� ���� ���̿� �ִ��� Ȯ��
        bool isInLimitRange = leftAngle > 0 && rightAngle > 0;

        if (!isRotating && !isInLimitRange)
        {
            rotateDirection = GetTurnDirection(camDir, spaceShip.forward);
            isRotating = true;
        }

        if (isRotating)
        {
            if (!isInLimitRange)
            {
                float rotAmount = rotateSpeed * Time.deltaTime * rotateDirection;
                transform.Rotate(0f, rotAmount, 0f);
                spaceShip.Rotate(0f, rotAmount, 0f);
            }
            else
            {
                isRotating = false;
                Debug.Log("ȸ�� ����");
            }
        }
    }

    // ����/������ ȸ�� ���� ���� (+1 or -1)
    int GetTurnDirection(Vector3 from, Vector3 to)
    {
        float angle = Vector3.SignedAngle(from, to, Vector3.up);
        return angle > 0 ? -1 : 1;
    }

    // Y�� ��� ����
    Vector3 FlatDirection(Vector3 dir)
    {
        dir.y = 0f;
        return dir.normalized;
    }
}
