using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCommand : ConsoleCommand
{
    public override string Name { get; protected set; }
    public override string Command { get ; protected set ; }
    public override string Description { get ; protected set ; }
    public override string Help { get; protected set; }

    public BuildCommand()
    {
        Name = "Build";
        Command = "build";
        Description = "Makes one manual build of collision system";
        Help = "Use this command to rebuild a collision system objects";
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        if(GameManager.Instance.ColSys != null)
        {
            GameManager.Instance.ColSys.Build();
            Console.Instance.AddMessageToConsole(GameManager.Instance.ColSys.ColSysName + " was build with "
                + GameManager.Instance.ColSys.NumOfObjects + " " + GameManager.Instance.ColSys.GetRoot().NodeType + " nodes.");    
        }
        else
            Console.Instance.AddMessageToConsole("There is no collision system chosen to build");
    }

    public static QuitCommand CreateCommand()
    {
        return new QuitCommand();
    }
}