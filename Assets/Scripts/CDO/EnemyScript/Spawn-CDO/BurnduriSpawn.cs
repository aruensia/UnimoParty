using UnityEngine;

public  class BurnduriSpawn : EnemySpawnBase
{
    public override void Spawn()
    {
        Debug.Log("BurnduriSpawn.spawn�Լ�����");
        //if (����, ��������) { }
        enemySpawnerCommand.SpawnEnemy("Burnduri", isPlayerHere(), 5);
    }

  
}
