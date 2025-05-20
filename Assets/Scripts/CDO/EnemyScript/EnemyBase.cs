using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IAttackable
{
    int Hp;
    public void TakeDamage(int Dmg)
    {
        Hp -= Dmg;
        if (Hp <= 0)
        {
            //DIE
        }
    }
}


public interface IAttackable
{
    void TakeDamage(int Dmg);
}






//����
public class Enemy1 : EnemyBase
{
    int Power;

    //�÷��̾�� ��ȣ�ۿ������� ����
    private void OnCollisionEnter(Collision collision)
    {
        //player.TakeDamage(Power);
    }


}