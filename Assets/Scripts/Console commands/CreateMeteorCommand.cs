using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CreateMeteorCommand : ConsoleCommand {

    public override string Name { get; protected set; }
    public override string Command { get; protected set; }
    public override string Description { get; protected set; }
    public override string Help { get; protected set; }

    public CreateMeteorCommand()
    {
        Name = "Create Meteor";
        Command = "make_meteors";
        Description = "Creates 1 or more meteors";
        Help = "use this command to make meteors for debugging";
        args = new string[] { "-num", "-stop" };
        AddCommandToConsole(Console.Instance);
    }

    public override void RunCommand(string[] args)
    {
        List<string> arguments = new List<string>(args);
        if(arguments.Contains(this.args[0]))
        {
            string result = arguments.Find(x => x.StartsWith("="));
            int num;
            /*if(Int32.TryParse(args, out num))
            {
                for(int i = 0; i < num ; i++)
                    GameManager.Instance.MakeTestMeteor();
            }
            else
                Console.Instance.AddMessageToConsole("Make sure that number of meteors is written correctly");*/
        }
        else
            GameManager.Instance.MakeTestMeteor();
    }

    public static CreateMeteorCommand CreateCommand()
    {
        return new CreateMeteorCommand();
    }
}
