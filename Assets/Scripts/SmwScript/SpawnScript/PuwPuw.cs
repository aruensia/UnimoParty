using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuwPuw : MonoBehaviour
{
    Vector3 Postion;

    float Radius;
    public float fixedY = 0f;
    float MoveSpeed = 10f;
    float Angle;           
    int rotateDirection;   //���ΰ��� �ð� �ݽð�

    Coroutine rotateCoroutine;

    void OnEnable()
    {
        Postion = transform.position;

        //���� �������� ����
        Angle = Random.Range(0f, Mathf.PI * 2f);

        //������ ��ġ 
        Radius = Random.Range(3f,20f);

        //���� ȸ�� ����(1 or -1)
        rotateDirection = Random.value < 0.5f ? 1 : -1;

        //���� ���� ���� �ڷ�ƾ ����
        rotateCoroutine = StartCoroutine(GoPewPew());//���� ���� �����Ѱ� �� �ʱ�ȭ ����
    }

    IEnumerator GoPewPew()
    {
        while (true)
        {

            //�� �ѷ��� ���� �ӵ�
            float angularSpeed =  MoveSpeed / Radius;
            //������ ȸ�� ���⿡ ���� �ٲ���
            Angle -= angularSpeed * Time.deltaTime * rotateDirection;
            //��ġ ����ؼ� �̵�
            float x = Mathf.Cos(Angle) * Radius;
            float z = Mathf.Sin(Angle) * Radius;
            transform.position = new Vector3(Postion.x + x, fixedY, Postion.z + z);

            yield return new WaitForFixedUpdate();
        }
    }

    // ������Ʈ�� ���� �� �ڷ�ƾ ����
    void OnDisable()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = null;
    }

}
