using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayerScript : MonoBehaviour {

    GridManager gm = null;

    CommandManager.Command replayCommand = null;
    CommandManager.Command undoCommand = null;

    public event Action myEvent;

    private int isClear;
    // Use this for initialization
    void Start ()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        gm.BuildWorld(30, 30);

        isClear = 0;
        replayCommand = new ReplayCommand();
        undoCommand = new UndoCommand();
    }

    // Update is called once per frame
    void Update ()
    {
        myEvent?.Invoke();
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
                if (hit.transform.tag == "Plane")
                {
                    var command = new CreateWallCommand(gm, hit.point);
                    CommandManager.Instance.AddCommand(command);
                }
            }
        }

        else if (Input.GetMouseButtonDown(2))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "enemy")
                {
                    Debug.Log("enemy");
                    var bullet = ObjectPool.GetObject();
                    var direction = hit.point - this.gameObject.transform.position;
                    bullet.Shoot(direction);

                    Destroy(hit.transform.gameObject);
                }
            }    
      
        }
      
        else if (Input.GetKeyDown(KeyCode.U))
        {
            undoCommand.Execute();
        }

        else if (isClear == 1)
        {
            replayCommand.Execute();
            isClear = 2;
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Success"))
        {
            isClear = 1;
        }
    }
}
