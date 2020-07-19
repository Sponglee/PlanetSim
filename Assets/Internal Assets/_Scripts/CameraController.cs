using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   
    public Material skyboxMat;
    public RTS_Camera cameraRef;
    float curRot = 0;

    // Update is called once per frame
    void Update()
    {
        if(cameraRef.lastInput != Vector3.zero)
        {
            //curRot += 0.1f * Time.deltaTime;
            //curRot %= 360;
            //RenderSettings.skybox.SetFloat("_Rotation", curRot);
        }

    }
}
