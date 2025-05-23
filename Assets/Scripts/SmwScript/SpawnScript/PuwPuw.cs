using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuwPuw : EnemyBase
{
    Vector3 Position;

    float Radius;
    float MoveSpeed=10;
    float Angle;           
    int rotateDirection;   //���ΰ��� �ð� �ݽð�
    float fixedY = 0f;// ���߿� �����ϰ� ���鲨

    Coroutine rotateCoroutine;

    private void OnEnable()
    {
        Position = transform.position;
        //���� ���� ũ��
        int RandomScale = Random.Range(1, 4);
        transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);

        //���� �������� ����
        Angle = Random.Range(0f, Mathf.PI * 2f);
        //���������� ��ġ 
        Radius = Random.Range(3f, 20f);
        //���� ȸ�� ����(1 or -1)
        rotateDirection = Random.value < 0.5f ? 1 : -1;
        rotateCoroutine = StartCoroutine(GoPewPew());//���� ���� �����Ѱ� �� �ʱ�ȭ ����
    }


    // ������Ʈ�� ���� �� �ڷ�ƾ ����
    void OnDisable()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = null;
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
            float x = Position.x + Mathf.Cos(Angle) * Radius;
            float z = Position.z + Mathf.Sin(Angle) * Radius;

            //���⼭ Y���� Terrain ���̷� ��ü
            float terrainY = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
            terrainY += transform.localScale.y / 2f;

            // Y���� Terrain ���̷� ����
            transform.position = new Vector3(x, terrainY + fixedY, z);

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            damage = 1;
            Manager.Instance.observer.HitPlayer(damage);
            //Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);
        }

    }

    public override void Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public override void CsvEnemyInfo()
    {
        throw new System.NotImplementedException();
    }
}
