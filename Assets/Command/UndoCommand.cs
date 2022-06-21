using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoCommand : CommandManager.Command
{
    public override void Execute()
    {
        CommandManager.Instance.Undo();
    }
}
