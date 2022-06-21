using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : CommandManager.Command
{
    GameObject  actor;
    Vector3     start, end;
    List<int>   path;
    GridManager gm = null;

    public MoveCommand(GridManager gm, GameObject actor, Vector3 start, Vector3 end )
    {
        this.gm = gm;
        this.actor = actor;
        this.start = start;
        this.end = end;
    }

    public override void Execute()
    {
        Vector3 current = Vector3.zero;
        gm.Move(actor, end);
        
    }

    public override void Undo()
    {
        gm.Move(actor, start, true);
    }
}
