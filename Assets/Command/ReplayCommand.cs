using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCommand : CommandManager.Command
{
    public override void Execute()
    {
        CommandManager.Instance.Replay();
    }
}
