using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuwPuw : EnemyBase
{
    Vector3 Postion;

    float Radius;
    float fixedY;
    float MoveSpeed=10;
    float Angle;           
    int rotateDirection;   //���ΰ��� �ð� �ݽð�

    Coroutine rotateCoroutine;

    Transform center;

    private void Start()
    {
        //center.position = Vector3.zero;
    }

    public override void Move(Vector3 direction)
    {
        Postion = transform.position;

        int RandomScale = Random.Range(1, 4);
        transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);
        fixedY = RandomScale / 2f;

        //���� �������� ����
        Angle = Random.Range(0f, Mathf.PI * 2f);

        //������ ��ġ 
        Radius = Random.Range(3f, 20f);

        //���� ȸ�� ����(1 or -1)
        rotateDirection = Random.value < 0.5f ? 1 : -1;

        //���� ���� ���� �ڷ�ƾ ����
        rotateCoroutine = StartCoroutine(GoPewPew());//���� ���� �����Ѱ� �� �ʱ�ȭ ����
    }

    public override void CsvEnemyInfo()
    {

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

    private void OnEnable()
    {
        Postion = transform.position;

        int RandomScale = Random.Range(1, 4);
        transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);
        fixedY = RandomScale / 2f;

        //���� �������� ����
        Angle = Random.Range(0f, Mathf.PI * 2f);

        //������ ��ġ 
        Radius = Random.Range(3f, 20f);

        //���� ȸ�� ����(1 or -1)
        rotateDirection = Random.value < 0.5f ? 1 : -1;

        //���� ���� ���� �ڷ�ƾ ����
        rotateCoroutine = StartCoroutine(GoPewPew());//���� ���� �����Ѱ� �� �ʱ�ȭ ����
    }


    // ������Ʈ�� ���� �� �ڷ�ƾ ����
    void OnDisable()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = null;
    }

}
