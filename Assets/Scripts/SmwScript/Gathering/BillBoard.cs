using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        // ī�޶� �ٶ󺸵�, ������Ʈ�� �׻� ������ �����ϰ� ȸ��
        Vector3 camPos = targetCamera.transform.position;
        transform.LookAt(transform.position + (transform.position - camPos));
    }
}
