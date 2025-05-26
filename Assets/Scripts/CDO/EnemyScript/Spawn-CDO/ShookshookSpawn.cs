using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
// - �� �𼭸�
// - 15�ʸ��� 1~2���� ����
// - �ִ� ��ü��
public class ShookshookSpawn : EnemySpawnBase
{
    [SerializeField] List<Transform> corners;
    int enemyMaxCount = 5; //�ִ� �� ����
    int currentEnemyCount = 0; //���� ��� ��ȯ �Ѵ���
    float cycleSecond = 2f; //�����ֱ�
    int maxRandomSpawnCount = 2; //�ѹ��� �������� ��� ��ȯ�ϴ��� 
    LinkedList<EnemyBase> Shookshook = new LinkedList<EnemyBase>();

    public override void Spawn()
    {
        StartCoroutine(ShookshookSpawnInstantiateCor(cycleSecond));
        StartCoroutine(isSetactiveFalse());

    }
    public override void StopSpawnCor()
    {
        StopAllCoroutines();
    }

    IEnumerator ShookshookSpawnInstantiateCor(float CycleSecond)
    {
        while (true)
        {
            //���� ���� �� == �ִ� ���� ������
            if (currentEnemyCount >= enemyMaxCount)
            {
                yield return null;
            }

            for (int i = 0; i < enemyMaxCount - currentEnemyCount; i++)
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
                    var Enemy = enemySpawnerCommand.SpawnEnemy("Shookshook", isPlayerHere(), 5);
                    Enemy.transform.position = corners[0].position;
                    yield return new WaitForSeconds(1f);

                    Shookshook.AddFirst(Enemy);
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
            var node = Shookshook.First;

            while (node != null)
            {
                var next = node.Next;
                //if (node.Value == null)
                if (node.Value.gameObject.activeInHierarchy == false)
                {
                    Shookshook.Remove(node);
                    currentEnemyCount--;
                }

                node = next;
            }

            yield return null;
        }


    }


}
