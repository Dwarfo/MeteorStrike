using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGraphCommand : ConsoleCommand
{
    public override string Name { get; protected set; }
    public override string Command { get; protected set; }
    public override string Description { get; protected set; }
    public override string Help { get; protected set; }

    public DrawGraphCommand()
    {
        Name = "Draw Graph";
        Command = "graph";
        Description = "Draws a graph of current cs hierarchy";
        Help = "Use to inspect a current hierarchy in a graph form";
        args = new string[] { };
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        GameManager.Instance.DrawGraph();
    }

    public static CreateMeteorCommand CreateCommand()
    {
        return new CreateMeteorCommand();
    }
}
