using Photon.Pun;
using UnityEngine;

//invoker
//���⼭ ������ �� �־ ����
public class EnemySpawnerCommand : MonoBehaviour
{
    public EnemyBase burnduriEnemyPrefab;
    public EnemyBase pewpewEnemyPrefab;
    public EnemyBase shookshookEnemyPrefab;
    private GameController gameController;

    // �� ���� �� Ŀ�ǵ� �Ҵ�
    public EnemyBase SpawnEnemy(string enemyType, Vector3 direction, float speed)
    {
        EnemyBase enemyObject = null;
        ICommand command = null;

        // ���� ������ ���� �����հ� Ŀ�ǵ� ����
        if (enemyType == "Burnduri")
        {
            enemyObject = PhotonNetwork.Instantiate("BurnduriTest", direction, Quaternion.identity).GetComponent<EnemyBase>();
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }
        else if (enemyType == "Pewpew")
        {
            enemyObject = PhotonNetwork.Instantiate("PewPew", direction, Quaternion.identity).GetComponent<EnemyBase>();
            command = new MoveCommand(enemyObject, direction, speed);
            command.Execute();
        }
        else if(enemyType == "Shookshook")
        {
            enemyObject = PhotonNetwork.Instantiate("ShookShookTest", direction, Quaternion.identity).GetComponent<EnemyBase>();
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
