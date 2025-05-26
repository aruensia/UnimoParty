using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//15rm �������ȿ� ����
//20�ʸ��� 1���� ����
//���� ������ ��ü ������� 1���� �ٷ� ����
public class PewpewSpawn : EnemySpawnBase
{
    int enemyMaxCount = 5; //�ִ� �� ����
    float cycleSecond = 20f; //�����ֱ�
    LinkedList<EnemyBase> pewpew = new LinkedList<EnemyBase>();


    public override void Spawn()
    {
        StartCoroutine(PewpewSpawnInstantiateCor(cycleSecond));
        StartCoroutine(isSetactiveFalse());

    }



    IEnumerator PewpewSpawnInstantiateCor(float CycleSecond)
    {
        for (int i = 0; i < enemyMaxCount; i++)
        {
            pewpew.AddFirst(enemySpawnerCommand.SpawnEnemy("Pewpew", isPlayerHere(), 5));
            yield return new WaitForSeconds(CycleSecond);
        }
    }


    IEnumerator isSetactiveFalse()
    {

        while (true)
        {
            var node = pewpew.First;

            while (node != null)
            {
                var next = node.Next;

                if (node.Value.gameObject.activeInHierarchy == false)
                {
                    pewpew.Remove(node);
                    var enemy = enemySpawnerCommand.SpawnEnemy("Pewpew", isPlayerHere(), 5);
                    pewpew.AddAfter(pewpew.First, enemy);
                }

                node = next;
            }

            yield return null;
        }


    }

    void PewpewSpawnStopCor()
    {
        StopAllCoroutines();
    }

}
