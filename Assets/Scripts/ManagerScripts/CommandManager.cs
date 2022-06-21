using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandManager : MonoBehaviour
{
    public abstract class Command
    {
        public float Ts { get; private set; }
        public Command() {
            Ts = Time.time;
        }

        public abstract void Execute();
        public virtual void Undo() {}
    }

    public static CommandManager Instance { get; private set; }

    private List<Command> commands = new List<Command>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this); 
            return;
        }
        Instance = this;
    }

    public void AddCommand(Command command)
    {
        command.Execute();
        commands.Add(command);
    }

    public void UndoAll()
    {
        var reversedCommands = new List<Command>(commands);
        reversedCommands.Reverse();
        foreach ( var command in reversedCommands )
        {
            command.Undo();
        }
    }

    public void Undo()
    {
        if (commands.Count == 0) return;
        var command = commands[commands.Count - 1];
        command.Undo();
        commands.RemoveAt(commands.Count - 1);
    }

    public void Replay()
    {
        StartCoroutine(replay());
      //  if(comm)
    }

    IEnumerator replay()
    {
        if (commands.Count == 0) yield break;
        UndoAll();
        float lastTs = commands[0].Ts;
        foreach (var command in commands)
        {
            var elapsed = command.Ts - lastTs;
            yield return new WaitForSeconds(elapsed);
            command.Execute();
            lastTs = command.Ts;
        }
        SceneManager.LoadScene("Clear");
    }
}
