//���� ������ �߻�Ŭ����
using UnityEngine;

public abstract class SpawnerBase : MonoBehaviour,ISpawnable
{
    public virtual void Spawn()
    {
        //����� enemy�⺻ �޼���
    }
}

//�����Լ� �������̽�
public interface ISpawnable
{
    public void Spawn();

}







//��� ����
public class EnemySpawner1 : SpawnerBase
{
    void Start()
    {
        base.Spawn(); //������� �޼���
        Spawn();      //�� �ڵ��� �޼���
    }
    public override void Spawn()
    {
        //����� enemy1�� �޼���
    }
}