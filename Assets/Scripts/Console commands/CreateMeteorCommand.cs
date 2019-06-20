using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeteorCommand : ConsoleCommand {

    public override string Name { get; protected set; }
    public override string Command { get; protected set; }
    public override string Description { get; protected set; }
    public override string Help { get; protected set; }
    public string[] args;

    public CreateMeteorCommand()
    {
        Name = "Create Meteor";
        Command = "make_meteors";
        Description = "Creates 1 or more meteors";
        Help = "use this command to mkae meteors for debugging";
        args = new string[] { "-num", "-stop" };
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand()
    {
        GameManager.Instance.MakeTestMeteor();
    }

    public static CreateMeteorCommand CreateCommand()
    {
        return new CreateMeteorCommand();
    }
}
