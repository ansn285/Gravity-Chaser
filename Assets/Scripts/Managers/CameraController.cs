using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // Transform = -.13, -35.18, -436.64
    // Rotation = 50, 0, 0

    public CinemachineVirtualCamera cvm;

    public void GroundedCam()
    {
        //cvm.Follow = null;
        //transform.position = new Vector3(-.13f, -35.18f, -436.64f);
        //transform.rotation = Quaternion.Euler(50, 0, 0);
    }

}
