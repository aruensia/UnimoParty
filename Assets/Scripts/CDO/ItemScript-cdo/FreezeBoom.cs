using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeBoom : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            photonView.RPC("Explode", RpcTarget.All);
            //Explode();
        }
    }

    [PunRPC]
    void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 30, Vector3.up, 100f, LayerMask.GetMask("Player", "Enemy", "Water"));
        foreach (var hitobj in hits)
        {
            Debug.Log("���� ��ź");

            GameObject target = hitobj.collider.gameObject;

            //�÷��̾� 
            //�޼� : ������(���̽�ƽ), ���
            //������ : �׷�, Ʈ����, ������
            if (target.layer == LayerMask.NameToLayer("Player"))
            {
                JoystickController PlayerMove;
                HeadDash headDash;

                if ((PlayerMove = target.GetComponentInChildren<JoystickController>()) != null)
                {
                    PlayerMove.Freeze(true);
                    Debug.Log("�÷��̾� ������ �");
                }

                if ((headDash = target.GetComponentInChildren<HeadDash>()) != null)
                {
                    headDash.Freeze(true);

                    Debug.Log("�÷��̾� ��� �");
                }

                if (target.TryGetComponent<HandHarvest>(out HandHarvest PlayerHarvest))
                {
                    PlayerHarvest.Freeze(true);

                    Debug.Log("�÷��̾� ä�� �");
                }

                //����
                //IFreeze[] IFreezeInterface = GetComponentsInChildren<IFreeze>();
                //foreach (IFreeze temp in IFreezeInterface)
                //{
                //    temp.Freeze(true);

                //}

            }

            if (target.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (target.TryGetComponent<EnemyBase>(out EnemyBase Enemy))
                {
                    ICommand command = null;
                    command = new FreezeCommand(Enemy, transform.position, true);
                    command.Execute();



                    Debug.Log("enemy �");
                }
                if (target.TryGetComponent<TestEnemybase>(out TestEnemybase TestEnemy))
                {
                    TestEnemy.Freeze(transform.position,true);


                    Debug.Log("TestEnemy �");
                }

            }



        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////






}
