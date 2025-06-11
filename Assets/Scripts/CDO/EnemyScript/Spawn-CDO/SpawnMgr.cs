using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    //0 : ���θ�
    [SerializeField] List<EnemySpawnBase> enemySpawnBase;

    List<Laycock> Laycocks = new List<Laycock>();

    int LaycockCount = 0;

    public void LaycockCountCheck(int Count, Laycock Monster)
    {
        if (Count > 0)
        {
            Laycocks.Add(Monster);
        }
        else if (Count < 0)
        {
            Laycocks.Remove(Monster);
        }
        else
        {
            Debug.Log("�����Ŵ��� ������ �� ����");
        }
  
        LaycockCount = LaycockCount + Count;

        if (LaycockCount == 10)
        {
            foreach(Laycock laycock in Laycocks)
            {
                laycock.ShootLazer();
            }
        }
    }



    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            AllSpawn();
        }
    }

    void AllSpawn()
    {
        enemySpawnBase[0].Spawn(); //���帮
        enemySpawnBase[1].Spawn(); //ǻǻ
        enemySpawnBase[2].Spawn(); //������
    }

    void StopAllCor()
    {
        enemySpawnBase[0].StopAllCoroutines();
        enemySpawnBase[1].StopAllCoroutines();
        enemySpawnBase[2].StopAllCoroutines();
    }
}
