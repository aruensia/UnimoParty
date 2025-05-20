using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//invoker
public class GameController : MonoBehaviour
{
    ICommand command;

    // Ŀ�ǵ� ����
    public void SetCommand(ICommand command)
    {
        this.command = command;
    }

    // Ŀ�ǵ� ����
    public void ExecuteCommand()
    {
        command.Execute();
    }
}
