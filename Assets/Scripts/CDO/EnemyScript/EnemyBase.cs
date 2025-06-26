//���ʹ� ���̽�
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EnemyBase : MonoBehaviourPunCallbacks
{
    ////csv
    ////�Ʒ� ���� ����ϼ���~
    //public string enemyName;              
    //public float spawnStartTime;           
    //public float spawnCycle;            
    //public float enemyMoveSpeed;                
    //public int sizeScale;              
    //public int spawnCount;

    public int damage;
    public bool ImFreeze { get; set; }

    public abstract void Move();
    public abstract void Freeze(Vector3 direction, bool isFreeze);
    //base
    //public abstract void CsvEnemyInfo();

}

