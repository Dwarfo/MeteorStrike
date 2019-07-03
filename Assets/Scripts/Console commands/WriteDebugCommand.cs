using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteDebugCommand : ConsoleCommand {

    public override string Name { get; protected set; }
    public override string Command { get; protected set; }
    public override string Description { get; protected set; }
    public override string Help { get; protected set; }

    public WriteDebugCommand()
    {
        Name = "Write Debug";
        Command = "showdebug";
        Description = "Reveals all debug info gathered at the moment";
        Help = "Use to show debug info";
        args = new string[] { };
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        Console.Instance.AddMessageToConsole(GameManager.Instance.GetDebugInfo());
    }

    public static CreateMeteorCommand CreateCommand()
    {
        return new CreateMeteorCommand();
    }
}