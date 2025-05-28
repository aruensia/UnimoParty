using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawnBase : MonoBehaviour 
{
    public abstract void Spawn();
    public abstract void StopSpawnCor();

    [SerializeField] protected EnemySpawnerCommand enemySpawnerCommand; //����Ŀ�ǵ�����
    [SerializeField] protected Transform centerTransform; //���߾�
    [SerializeField] protected Transform SpawnTransform;//������ġ
    [SerializeField] protected List<Transform> players; //player��ġ
    [SerializeField] protected float distanceGap; //������ġ ������ġ ������ �Ÿ� //�÷��̾���ġ�� �ȳ�����
    [SerializeField] protected int r = 15; //������ //������ġ
    protected float angle;

    private void Start()
    {
        foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(p.transform);
        }

    }

    protected Vector3 DonutPostion()
    {
        //var tempPostion = new Vector3(20, 0, 16);

        var tempPostion = centerTransform.position;
        angle = UnityEngine.Random.Range(0, 360);
        float x = Mathf.Cos(angle) * r + tempPostion.x;
        float y = SpawnTransform.transform.position.y ;
        float z = Mathf.Sin(angle) * r + tempPostion.z;
        SpawnTransform.position = new Vector3(x, y, z);

        return SpawnTransform.position;
    }


    protected Vector3 isPlayerHere()
    {
        for (int i = 0; i < 5; i++)
        {
            //var dd = donutTransform.position; //�׽�Ʈ����
            var dd = DonutPostion(); //����
            bool isrmscj = false;
            foreach (var player in players)
            {
                if (Vector3.Distance(dd, player.position) < distanceGap)
                {
                    Debug.Log("�÷��̾ ��ó�� ");
                    isrmscj = true; 
                    break;
                }
            }

            if (isrmscj == true)
            {

            }
            else
            {
                return dd;
            }
           
        }
        //���߿� player��ó �ƴҶ����� ������ɵ�
        Debug.Log("i�� ���Ǵµ� �÷��̾ ��ó��");

        var aa = DonutPostion();
        return aa;
    }
    


}
