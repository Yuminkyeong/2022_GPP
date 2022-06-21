using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : MonoBehaviour
{
    public GameObject target;
    public float CameraSpeed = 1f;

    //camera position x,y,z -> ó�� ���� ��ġ ��
    private float offsetx = 0f;
    private float offsety = 20f;
    private float offsetz = 0f;

   
    private void Update()
    {
        //Ÿ���� ���� ī�޶� ��ġ�� ����
        Vector3 changedPos =
            new Vector3(
                target.transform.position.x + offsetx,
                offsety,
                target.transform.position.z + offsetz
                );

        transform.position = Vector3.Lerp(transform.position, changedPos, Time.deltaTime * CameraSpeed);
    }

}
