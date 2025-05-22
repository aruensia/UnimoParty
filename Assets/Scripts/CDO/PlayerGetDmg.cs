using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߿� playerƲ ¥�»�� �����ϸ��

public class PlayerGetDmg : MonoBehaviour,IDamageable
{
    [SerializeField]int hp = 10;
    public void TakeDamage(int dmg)
    {
        hp-=dmg;    
        if(hp <= 0)
        {
            //�÷��̾� ����
            //this.gameObject.transform.parent.gameObject.SetActive(false);   
            this.gameObject.transform.gameObject.SetActive(false);   
        }

    }


    [ContextMenu("�׽�Ʈ ������ �ޱ�")]
    public void testTakeDmgBtn()
    {
        TakeDamage(5);
    }
   
}
