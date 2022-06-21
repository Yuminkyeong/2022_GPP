using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    GridManager gm = null;

    CommandManager.Command replayCommand = null;
    CommandManager.Command undoCommand = null;

   
	// Use this for initialization
	void Start ()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        gm.BuildWorld(30, 30);

        replayCommand = new ReplayCommand();
        undoCommand = new UndoCommand();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               if (hit.transform.tag == "Wall2")
                {
                   var command = new Wall2Command(hit.transform.gameObject);
                    CommandManager.Instance.AddCommand(command);
                    return;
                }
                var moveCommand = new MoveCommand(gm, gameObject, gameObject.transform.position, hit.point);
                CommandManager.Instance.AddCommand(moveCommand);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //print(hit.transform.tag);
                if (hit.transform.tag == "Plane")
                {
                    var command = new CreateWallCommand(gm, hit.point);
                    CommandManager.Instance.AddCommand(command);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            undoCommand.Execute();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            replayCommand.Execute();
        }
    }    
}
