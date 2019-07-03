using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : Singleton_MB<Console> {

    [Header("UI components")]
    public Canvas consoleCanvas;
    public ScrollRect scrollRect;
    public Text consoleText;
    public Text inputText;
    public InputField consoleInput;

    public Dictionary<string, ConsoleCommand> Commands;

	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);

            if (consoleCanvas.gameObject.activeInHierarchy)
                consoleInput.ActivateInputField();
            else
                consoleInput.DeactivateInputField();
            consoleInput.text = "";
        }

        if (consoleCanvas.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (inputText.text != "")
                {
                    AddMessageToConsole(inputText.text);
                    ParseInput(inputText.text);
                }
            }
        }
        //consoleInput.text = "";
    }

    void Start()
    {
        consoleCanvas.gameObject.SetActive(false);
        Commands = new Dictionary<string, ConsoleCommand>();
        CreateCommands();
    }

    public void CreateCommands()
    {
        AddNewCommand(new QuitCommand());
        AddNewCommand(new CreateMeteorCommand());
        AddNewCommand(new BuildCommand());
        AddNewCommand(new CheckCollsCommand());
        AddNewCommand(new NearestNeighbourCommand());
        AddNewCommand(new WriteDebugCommand());
    }

    private void AddNewCommand(ConsoleCommand cc)
    {
        Commands.Add(cc.Command, cc);
    }

    public void AddCommandToConsole(ConsoleCommand command)
    {
        if (!Commands.ContainsKey(command.Command))
        {
            Commands.Add(name, command);
        }

    }

    public void AddMessageToConsole(string msg)
    {
        consoleText.text += msg + "\n";
        //scrollRect.verticalNormalizedPosition = 0f;
    }

    private void ParseInput(string input)
    {
        string[] inputs = input.Split(null);
        if (inputs.Length == 0 || inputs == null)
        {
            AddMessageToConsole("Unknown Command");
        }

        if (!Commands.ContainsKey(inputs[0]))
        {
            AddMessageToConsole("Unknown Command");
        }
        else
        {
            Commands[inputs[0]].RunCommand(inputs);
        }

        consoleInput.DeactivateInputField();
        consoleInput.text = "";
        consoleInput.ActivateInputField();
    }




}

public abstract class ConsoleCommand
{
    public abstract string Name { get; protected set; }
    public abstract string Command { get; protected set; }
    public abstract string Description { get; protected set; }
    public abstract string Help { get; protected set; }
    public string[] args;

    public void AddCommandToConsole(Console console)
    {
        console.AddCommandToConsole(this);
        string addMesage = " command has been added to the console.";

    }

    public abstract void RunCommand(string[] args);





}