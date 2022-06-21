using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    public GameObject BoxPrefab;

    PlayerScript player;
    BoxCollider boxCollider;
    GameObject parent;

    int childCount=0;

    void DropBoxes()
    {
        if (childCount<=3 && isNear())
        {
            Instantiate(BoxPrefab, parent.transform);
        }
        else
        {
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        boxCollider = GetComponent<BoxCollider>();
        parent = GameObject.Find("BoxSpawn");
        player.myEvent += DropBoxes;
    }

    bool isNear()
    {
        childCount = parent.transform.childCount;
        Debug.Log((player.transform.position - boxCollider.transform.position).magnitude);
        return (player.transform.position - boxCollider.transform.position).magnitude <=2;
    }

}
