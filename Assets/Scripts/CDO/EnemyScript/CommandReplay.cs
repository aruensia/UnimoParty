using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//invoker
//���÷��̿�
//�غ��� �; �غ�
public class CommandReplay
{
    //��Ͽ�
    private Queue<ICommand> commandHistory = new Queue<ICommand>();


    public void ExecuteCommand(ICommand newCommand)
    {
        commandHistory.Enqueue(newCommand);
        newCommand.Execute();
    }

    public void ReplayCommands()
    {
        Debug.Log(commandHistory.Count);
        foreach (var cmd in commandHistory)
        {
            Debug.Log("2");
            cmd.Execute();
        }
    }

    public IEnumerator ReplayCommandsCoroutine()
    {
        Debug.Log("���÷��� ����");
        foreach (var cmd in commandHistory)
        {
            yield return new WaitForSeconds(cmd.DelayTime); 
            cmd.Execute();
        }
    }

    public void ClearHistory()
    {
        commandHistory.Clear();
    }

    
}
