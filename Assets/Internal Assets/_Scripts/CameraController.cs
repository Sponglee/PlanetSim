using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform secondCam;
    public Material skyboxMat;
    public RTS_Camera cameraRef;
    float curRot = 0;

    // Update is called once per frame
    void Update()
    {
        if(cameraRef.lastInput != Vector3.zero)
        {
           
            secondCam.Rotate(new Vector3(cameraRef.lastInput.y,cameraRef.lastInput.y,0));
            //curRot += 0.1f * Time.deltaTime;
            //curRot %= 360;
            //RenderSettings.skybox.SetFloat("_Rotation", curRot);
        }
        else
        {
             secondCam.Rotate(new Vector3(Input.GetAxis("Vertical"),-1*Input.GetAxis("Horizontal"),0));
            
        }

    }
}
