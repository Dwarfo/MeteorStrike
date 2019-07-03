using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollsCommand : ConsoleCommand
{
    public override string Name { get; protected set; }
    public override string Command { get ; protected set ; }
    public override string Description { get ; protected set ; }
    public override string Help { get; protected set; }

    public CheckCollsCommand()
    {
        Name = "CheckColls";
        Command = "check";
        Description = "Makes a manual collision check";
        Help = "Use this command to check collisions and verify info about it";
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        if(GameManager.Instance.ColSys != null)
        {
            GameManager.Instance.ColSys.CheckCollisions();
            Console.Instance.AddMessageToConsole(GameManager.Instance.ColSys.ColSysName + " has made "
            + GameManager.Instance.ColSys.CollisionChecks + " collision checks");
        }
        else
            Console.Instance.AddMessageToConsole("There is no collision system chosen to check");
    }

    public static QuitCommand CreateCommand()
    {
        return new QuitCommand();
    }
}