using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//invoke
//���⼭ ������ �� �־ ����
public class EnemySpawnerCommand : MonoBehaviour
{
    public EnemyBase UpEnemyPrefab; //ENEMY5  
    public EnemyBase RightEnemyPrefab;
    private GameController gameController;

    // �� ���� �� Ŀ�ǵ� �Ҵ�
    public void SpawnEnemy(string enemyType, Vector3 direction, float speed )
    {
        EnemyBase enemyObject = null;
        ICommand command = null;

        // ���� ������ ���� �����հ� Ŀ�ǵ� ����
        if (enemyType == "Up")
        {
            enemyObject = Instantiate(UpEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
        }
        else if (enemyType == "Right")
        {
            enemyObject = Instantiate(RightEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
        }

        // ������ ���� Ŀ�ǵ� ����
        gameController.SetCommand(command);
        gameController.ExecuteCommand();
    }






}
