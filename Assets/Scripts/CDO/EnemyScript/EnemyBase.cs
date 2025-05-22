using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    //csv
    //�Ʒ� ���� ����ϼ���~
    public string enemyName;              
    public float spawnStartTime;           
    public float spawnCycle;            
    public int damage;                     
    public float enemyMoveSpeed;                
    public int sizeScale;              
    public int spawnCount;                

    public abstract void Move(Vector3 direction);

    //base
    public abstract void CsvEnemyInfo();

}

