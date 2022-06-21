using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWallCommand : CommandManager.Command
{
    GridManager gm;
    GameObject  wall;
    Vector3     pos;
    bool        markWallFlag = true;

    public CreateWallCommand(GridManager gm, Vector3 pos, bool markWallFlag = true)
    {
        wall = null;
        this.pos = pos;
        this.markWallFlag = markWallFlag;
        this.gm = gm;
    }

    public override void Execute()
    {
        wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.GetComponent<MeshRenderer>().material.color = Color.cyan;
        wall.tag = "Wall";

        if (markWallFlag == true)
        {
            wall.transform.position = gm.pos2center(pos);
            gm.SetAsWall(wall.transform.position);
        }
        else
        {
            wall.transform.position = gm.pos2center(pos) + new Vector3(0f, pos.y-0.5f, 0f) + Vector3.up;
        }
    }

    public override void Undo()
    {
        if (markWallFlag == true)
        {
            gm.SetAsWall(wall.transform.position, GridManager.TileType.Plain);
        }
        GameObject.DestroyImmediate(wall);
    }
}
