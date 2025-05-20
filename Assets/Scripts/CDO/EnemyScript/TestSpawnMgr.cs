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
        string enemyType = "";

        if(Random.Range(0, 2) == 0)
        {
            enemyType = "Up";
        }
        else if(Random.Range (0, 2) == 1)
        {
            enemyType = "Right";
        }

        enemySpawnerCommand.SpawnEnemy(enemyType, Vector3.forward, 5);
    }




}
