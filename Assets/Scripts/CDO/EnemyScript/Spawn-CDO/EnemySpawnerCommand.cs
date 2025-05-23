using UnityEngine;

//invoke
//���⼭ ������ �� �־ ����
public class EnemySpawnerCommand : MonoBehaviour
{
    public EnemyBase burnduriEnemyPrefab;
    public EnemyBase pewpewEnemyPrefab;
    private GameController gameController;

    // �� ���� �� Ŀ�ǵ� �Ҵ�
    public EnemyBase SpawnEnemy(string enemyType, Vector3 direction, float speed)
    {
        EnemyBase enemyObject = null;
        ICommand command = null;

        // ���� ������ ���� �����հ� Ŀ�ǵ� ����
        if (enemyType == "Burnduri")
        {
            enemyObject = Instantiate(burnduriEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }
        else if (enemyType == "Pewpew")
        {
            enemyObject = Instantiate(pewpewEnemyPrefab, transform.position, Quaternion.identity);
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }





        if (enemyObject == null)
        {
            Debug.Log("SpawnEnemy null��");
            return null;
        }

        return enemyObject;
        // ������ ���� Ŀ�ǵ� ����
        //gameController.SetCommand(command);
        //gameController.ExecuteCommand();
    }






}
