using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vCamScript : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public CinemachineFramingTransposer vCamBase;
    public float vDistance { get { return vCamBase.m_CameraDistance; } set { vCamBase.m_CameraDistance = Math.Min(Math.Max(value, 10), 50); } }

    public 
    
    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        vCamBase = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            vDistance -= 5;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
           vDistance += 5;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(2))
        {
            //rotate camera around player
            transform.rotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5, Vector3.up) * transform.rotation;
        }
    }
}
