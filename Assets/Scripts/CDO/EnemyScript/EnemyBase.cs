using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//�� ���� ���� ���� ����� ���ÿ� ����� �� ���� (�̵� �� ���� ��)
public abstract class EnemyBase : MonoBehaviour
{
    public float health = 100f;

    public abstract void Move(Vector3 direction);
}

