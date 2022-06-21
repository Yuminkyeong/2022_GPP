using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    //�ӵ�, hp �ۿ��� Ȯ��, �����ϱ� ���� �켱 public����
    public float moveSpeed = 10.0f;
    public int hp = 5;
    public Transform myTransform;
    private Rigidbody playerRigidbody;

  
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
    }

    public Transform GetPos()
    {
        return myTransform;
    }
    private void Update()
    {
        
        float hAxis = Input.GetAxisRaw("Horizontal");
            float vAxis = Input.GetAxisRaw("Vertical");

            Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        playerRigidbody.velocity = inputDir * moveSpeed;

            transform.LookAt(transform.position + inputDir);
        myTransform = GetComponent<Transform>();
    }
}
