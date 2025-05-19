using System.Collections;
using UnityEngine;


//����1. DelayTime�̰Ŵ� ���� �����Ѱ� �´���?
//����2. ���� �ൿ���� �����ߴ�µ� Ʋ�� �������� �ϴٺ��� �̷��Ե�
//       �̰� Ŀ�ǵ����� �³�?  ĸ��ȭ�Ѱ� �´µ� �̻���
//����3. �ǵ���� �ҷ��� ��� �������ϴ°Ű����� �׷��� �򰥸����ȳ�? 
//       ��� ������ �⺻ ������1, ���2�� ���� //��3�� ��ӹ���
//       �ൿ����4, �ൿ�5 �̷��� 5�� �������ϳ�?
//       �߰��� ���������� ��������6, ���ݰ7 �̷��Եų�?
//����4. Ŀ�ǵ� vs ����

/// <summary>
/// //////////////////////////////////////////////////////////////
/// </summary>


//[Invoker] CommandReplay
//    |
//    V
//[ICommand] <interface>
//    | -float DelayTime { get; }
//    | -void Execute()

//[Command] MoveCommand: ICommand
//    | -EnemyBase receiver
//    | -Vector3 direction

//[Receiver] Enemy5: EnemyBase
//    | - public void Move(Vector3 direction)
public class Enemy5 : EnemyBase, IDamageable, ICommand
{
    Vector3 direction;
    Vector3 startPos;
    public float moveSpeed = 5f;

    CommandReplay commandReplay;

    //**�ٸ���ũ��Ʈ���� ��������, �ٵ� ���ο����� ���� ����? ������Ʈ?
    //�׳� �ű��ؼ� ������
    public float DelayTime { get; private set; }

    private void Start()
    {
        startPos = transform.position;

        commandReplay = new CommandReplay();
        StartCoroutine(cor());

    }

    IEnumerator cor()
    {
        move1(0);
        yield return new WaitForSeconds(2f);
        move1(2);
        yield return new WaitForSeconds(3f);

        transform.position = startPos;
        yield return new WaitForSeconds(1f);

        //commandInvoker.ReplayCommands();
        yield return StartCoroutine(commandReplay.ReplayCommandsCoroutine());
    }

    private void move1(float delay)
    {
        Vector3 moveDir = Vector3.forward;
        SetMoveCommand(moveDir, delay);

        //Execute(); //���÷��� �Ⱦ���
        commandReplay.ExecuteCommand(this); //���÷��̿�
    }


    public override void Move(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed);
    }

    public void SetMoveCommand(Vector3 direction, float delay)
    {
        this.direction = direction;
        DelayTime = delay;
    }

    public void Execute()
    {
        Move(direction);
    }
}