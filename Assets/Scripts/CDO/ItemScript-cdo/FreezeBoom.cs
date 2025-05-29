using System.Collections;
using UnityEngine;

public class FreezeBoom : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        Explode();
    }

    void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3, Vector3.up, 10f, LayerMask.GetMask("Player", "Enemy", "Water"));
        foreach (var hitobj in hits)
        {
            Debug.Log("���� ��ź");

            GameObject target = hitobj.collider.gameObject;

            //�÷��̾� 
            //�޼� : ������(���̽�ƽ)
            //������ : �׷�, Ʈ����, ������
            if (target.layer == LayerMask.NameToLayer("Player"))
            {
                //�޼�
                if (target.TryGetComponent<JoystickController>(out JoystickController PlayerMove))
                {
                    PlayerMove.moveSpeed = 0;
                    Debug.Log("�÷��̾� ������ �");
                }


                if (target.TryGetComponent<HandHarvest>(out HandHarvest PlayerHarvest))
                {
                    PlayerHarvest.Freeze(false);

                    Debug.Log("�÷��̾� ä�� �");
                }


            }

            if (target.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (target.TryGetComponent<EnemyBase>(out EnemyBase Enemy))
                {
                    ICommand command = null;
                    command = new FreezeCommand(Enemy, transform.position);
                    command.Execute();



                    Debug.Log("enemy �");
                }
                if (target.TryGetComponent<TestEnemybase>(out TestEnemybase TestEnemy))
                {
                    TestEnemy.Freeze(transform.position);


                    Debug.Log("TestEnemy �");
                }

            }



        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////
    





}
