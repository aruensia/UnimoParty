using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//���θ�
// - ���� ���� (r�ٲٸ��)
// - 10�ʸ��� 1~2���� ����
// - �ִ� ��ü��
public  class BurnduriSpawn : EnemySpawnBase
{
    int enemyMaxCount = 5; //�ִ� �� ����
    int currentEnemyCount = 0; //���� ��� ��ȯ �Ѵ���
    float cycleSecond = 2f; //�����ֱ�
    int maxRandomSpawnCount = 2; //�ѹ��� �������� ��� ��ȯ�ϴ��� 
    LinkedList<EnemyBase> Burnduri = new LinkedList<EnemyBase>();

    public override void Spawn()
    {
        StartCoroutine(BurnduriSpawnInstantiateCor(cycleSecond));
        StartCoroutine(isSetactiveFalse());

    }
    public override void StopSpawnCor()
    {
        StopAllCoroutines();
    }

    IEnumerator BurnduriSpawnInstantiateCor(float CycleSecond)
    {
        while (true)
        {
            //���� ���� �� == �ִ� ���� ������
            if (currentEnemyCount>=enemyMaxCount)
            {
               yield return null;
            }

            for (int i = 0; i < enemyMaxCount- currentEnemyCount; i++)
            {
                var randomInstantiateNum = Random.Range(0, maxRandomSpawnCount); //0~2

                Debug.Log(randomInstantiateNum);

                for (int j = 0; j <= randomInstantiateNum; j++)
                {
                    if (currentEnemyCount >= enemyMaxCount)
                    {
                        break;
                    }

                    currentEnemyCount++;
                    Burnduri.AddFirst(enemySpawnerCommand.SpawnEnemy("Burnduri", isPlayerHere(), 5));
                }


                yield return new WaitForSeconds(CycleSecond);
            }

            yield return null;
        }

    }


    IEnumerator isSetactiveFalse()
    {

        while (true)
        {
            var node = Burnduri.First;

            while (node != null)
            {
                var next = node.Next;
                //if (node.Value == null)
                if (node.Value.gameObject.activeInHierarchy == false)
                {
                    Burnduri.Remove(node);
                    currentEnemyCount--;
                }

                node = next;
            }

            yield return null;
        }


    }



}
