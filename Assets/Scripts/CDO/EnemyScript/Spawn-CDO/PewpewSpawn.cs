using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//15rm �������ȿ� ����
//20�ʸ��� 1���� ����
//���� ������ ��ü ������� 1���� �ٷ� ����
public class PewpewSpawn : EnemySpawnBase
{
    int EnemyMaxCount = 5;
    List<EnemyBase> pewpew =new List<EnemyBase>();

    public override void Spawn()
    {
        dsdsd();

    }

    void dsdsd()
    {
        StartCoroutine(SpawnCor());



    }

    IEnumerator SpawnCor()
    {
        while (true)
        {

                oneSpawn();
            for (int i = 0; i < EnemyMaxCount; i++)
            {
                pewpew.Add(enemySpawnerCommand.SpawnEnemy("Pewpew", isPlayerHere(), 5));
                yield return new WaitForSeconds(2);
            }
            yield return null;  

        }


    }

    void oneSpawn()
    {
        foreach (EnemyBase p in pewpew)
        {
            if (p.gameObject.activeInHierarchy == false)
            {
                pewpew.Add(enemySpawnerCommand.SpawnEnemy("Pewpew", isPlayerHere(), 5));
            }
        }
    }



}
