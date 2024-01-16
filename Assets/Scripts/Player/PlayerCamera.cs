using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private bool cameraMoving = false;
    public GameObject player;
    [Range(60, 300)]
    public int camera_distance = 60;
    [Range(1, 20)]
    public int camera_zoom = 5;
    private bool cameraRotating = false;
    private Vector3 offset;
    private int _camera_distance //private property to get and set camera distance, and limit the value while setting
    {
        get { return camera_distance; }
        set { camera_distance = Math.Min(Math.Max(value, 60), 300); }
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, camera_distance / (float)Math.Sqrt(2), camera_distance / (float)Math.Sqrt(2));
    }

    // Update is called once per frame
    void Update()
    {
        //if is holding middle mouse button, then the camera will rotate

        if (Input.GetMouseButtonDown(2))
        {
            cameraRotating = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            cameraRotating = false;
        }

        
        // if plyaer is scrolling the mouse wheel, then the camera will zoom in or out

        if (Input.mouseScrollDelta.y > 0)
        {
            _camera_distance -= camera_zoom;
            offset = -transform.forward * camera_distance;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            _camera_distance += camera_zoom;
        }


        offset = -transform.forward * camera_distance;
    }

    private void FixedUpdate()
    {
        //This fixed update tick is for the camera movement
        
        if (cameraRotating)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5, Vector3.up) * offset;
        }
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);

    }
}
