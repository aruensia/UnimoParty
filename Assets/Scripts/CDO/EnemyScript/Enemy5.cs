using System.Collections;
using UnityEngine;

public class Enemy5 : EnemyBase, IDamageable
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
        ExecuteMoveCommand(Vector3.forward, 5f, 0f);
        yield return new WaitForSeconds(2f);
        ExecuteMoveCommand(Vector3.forward, 5f, 2f);
        yield return new WaitForSeconds(3f);

        transform.position = startPos;
    }

    private void ExecuteMoveCommand(Vector3 direction, float speed, float delay)
    {
        ICommand moveCommand = new MoveCommand(this, direction, speed);
        moveCommand.Execute();

        //commandReplay.ExecuteCommand(moveCommand);
    }

    public override void Move(Vector3 direction)
    {
        Debug.Log("move ����");
        transform.Translate(direction * moveSpeed);
    }
}

public class MoveCommand : ICommand
{
    EnemyBase enemyBase;
    Vector3 direction;

    public float DelayTime { get; private set; }

    public MoveCommand(EnemyBase enemyBase, Vector3 direction, float delayTime)
    {
        this.enemyBase = enemyBase;
        this.direction = direction;
        DelayTime = delayTime;
    }

    public void Execute()
    {
        enemyBase.Move(direction);
    }

}

public class Enemy4 : EnemyBase, IDamageable
{
    private void Start()
    {
        ExecuteMoveCommand(Vector3.forward, 5f, 0f);
    }

    public override void Move(Vector3 direction)
    {
        //���������� ��
    }

    //�ߺ��� �Ͼ��?
    private void ExecuteMoveCommand(Vector3 direction, float speed, float delay)
    {
        ICommand moveCommand = new MoveCommand(this, direction, speed);
        moveCommand.Execute();

        //commandReplay.ExecuteCommand(moveCommand);
    }

}