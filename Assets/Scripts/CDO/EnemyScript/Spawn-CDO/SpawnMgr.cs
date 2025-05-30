using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    //0 : ���θ�
    [SerializeField] List<EnemySpawnBase> enemySpawnBase;

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
