using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall2Command : CommandManager.Command
{
    GameObject wall2;
    Renderer wall2_color;
    
    int hit_num;
    Color[] color = { Color.green, Color.red, new Color(1,1,1,0)};
    public Wall2Command(GameObject wall2)
    {
        this.wall2 = wall2;
        this.wall2_color = wall2.GetComponent<Renderer>();
    }
    public override void Execute()
    {
        CheckColor();
        if (hit_num == 0) wall2_color.material.color = color[0];
        else if (hit_num == 1) wall2_color.material.color = color[1];
        else if (hit_num == 2)
        {
            wall2_color.material.color = color[2];
            wall2.GetComponent<BoxCollider>().gameObject.SetActive(false);
        }
    }

    void CheckColor()
    {
        if(wall2_color.material.color == Color.white)
        {
            hit_num = 0;
        }
        else if (wall2_color.material.color == color[0])
        {
            hit_num = 1;
        }
        else if (wall2_color.material.color == color[1])
        {
            hit_num = 2;
        }
        else if (wall2_color.material.color == color[2])
        {
            hit_num = 3;
        }
    }
    public override void Undo()
    {
        CheckColor();
        if (hit_num == 3)
        {
            wall2_color.material.color = color[1];
            wall2.GetComponent<BoxCollider>().gameObject.SetActive(true);
        }
        else if(hit_num == 2)
        {
           wall2_color.material.color = color[0];
        }
        else if (hit_num == 1)
        {
            wall2_color.material.color = Color.white;
        }
    }
}
