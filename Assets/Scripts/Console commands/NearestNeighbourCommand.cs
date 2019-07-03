using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestNeighbourCommand : ConsoleCommand {

    public override string Name { get; protected set; }
    public override string Command { get; protected set; }
    public override string Description { get; protected set; }
    public override string Help { get; protected set; }

    public NearestNeighbourCommand()
    {
        Name = "Nearest neighbour";
        Command = "neighbour";
        Description = "Finds a neighbour for specified object";
        Help = "Use to get distance and name of nearest object";
        args = new string[] { };
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        List<string> arguments = new List<string>(args);
        GameObject goToFind = GameObject.Find(args[1]);

        if(goToFind != null)
        {
            var objAndDist = GameManager.Instance.ColSys.GetNearestNeighbour(goToFind);
            Console.Instance.AddMessageToConsole("Nearest Neighbour: " + objAndDist.Key + " Distance: " + objAndDist.Value);
        }
        else
            Console.Instance.AddMessageToConsole("Cannot find an object for checking");
    }

    public static CreateMeteorCommand CreateCommand()
    {
        return new CreateMeteorCommand();
    }
}
