using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����޼���
public class TestSpawnMgr : MonoBehaviour
{
    public EnemySpawnerCommand enemySpawnerCommand;

    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", 1f, 2f);
    }

    // �������� ���� �����ϴ� �޼���
    void SpawnRandomEnemy()
    {
        string enemyType = "Burnduri";
        //string enemyType = "pupu";

        enemySpawnerCommand.SpawnEnemy(enemyType, Vector3.forward, 5);
    }




}
