using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool enemypool;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemySpwanPos;
    [SerializeField] private GameObject InitCount;

    Queue<Enemy> enemyUnitPool = new Queue<Enemy>();

    private void Awake()
    {
        enemypool = this;
    }

    private void InitEnemyPrefab(int initcount)
    {
        for (int i = 0; i < initcount; i++)
        {
            enemyUnitPool.Enqueue(CreatEnemy());
        }
    }

    private Enemy CreatEnemy()
    {
        var newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);

        // �ε��� Csv�� ���̺��� ������ ���� �����Ϳ� �Ľ��Ͽ� ����.

        return newEnemy;
    }

    public Enemy GetEnemy()
    {
        if(enemyUnitPool.Count > 0)
        {
            var enemy = enemyUnitPool.Dequeue();
            enemy.transform.SetParent(enemySpwanPos.transform);
            enemy.transform.position = enemySpwanPos.transform.position;
            enemy.gameObject.SetActive(true);

            return enemy;
        }
        else
        {
            var newenemy = CreatEnemy();
            newenemy.transform.SetParent(enemySpwanPos.transform);
            newenemy.transform.position = enemySpwanPos.transform.position;
            newenemy.gameObject.SetActive(true);

            return newenemy;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);
        enemyUnitPool.Enqueue(enemy);
    }



}
