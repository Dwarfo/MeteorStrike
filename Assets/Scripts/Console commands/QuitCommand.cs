using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitCommand : ConsoleCommand
{
    public override string Name { get; protected set; }
    public override string Command { get ; protected set ; }
    public override string Description { get ; protected set ; }
    public override string Help { get; protected set; }

    public QuitCommand()
    {
        Name = "Quit";
        Command = "quit";
        Description = "Quits the application";
        Help = "use this command to quit from unity";
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public static QuitCommand CreateCommand()
    {
        return new QuitCommand();
    }
}
