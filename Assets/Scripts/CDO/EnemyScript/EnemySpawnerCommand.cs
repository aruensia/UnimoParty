using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//invoke
//���⼭ ������ �� �־ ����
public class EnemySpawnerCommand : MonoBehaviour
{
    public EnemyBase burnduriEnemyPrefab;
    public EnemyBase pupuEnemyPrefab;
    private GameController gameController;

    // �� ���� �� Ŀ�ǵ� �Ҵ�
    public void SpawnEnemy(string enemyType, Vector3 direction, float speed )
    {
        EnemyBase enemyObject = null;
        ICommand command = null;

        // ���� ������ ���� �����հ� Ŀ�ǵ� ����
        if (enemyType == "Burnduri")
        {
            //�̰Ű�����? 
            //�ٵ� �� �ָ��Ѱ� �̷��� �ϳ��� �����ո� ���������ȳ�?
            //�̰Ŵ� ���� ������ �ϴ� �����Ѱ� �ؾߵɵ�? ������ �ٸ��ֵ鵵 Ǯ���ϰ� 
            //������ߴ� �ƴҼ�����
            //var enemyObject1 = EnemyPool.enemypool.GetEnemy();

            enemyObject = Instantiate(burnduriEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }
        else if (enemyType == "pupu")
        {
            enemyObject = Instantiate(pupuEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }

        // ������ ���� Ŀ�ǵ� ����
        //gameController.SetCommand(command);
        //gameController.ExecuteCommand();
    }






}
