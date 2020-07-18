using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1f;
    public float scrollSpeed = 1f;

    public Material[] skyboxes;

    public Transform cameraPivotTransform;

    public CinemachineVirtualCamera vcam;
    public CinemachineVirtualCamera vcamZoomed;

    CinemachineFramingTransposer transposer;

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    // Update is called once per frame
    void Update()
    {
       

        //if(Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
           

        //    transposer.m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        //    transposer.m_CameraDistance = Mathf.Clamp(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance, 8f, 90f);


        //    //vcam.transform.rotation = Quaternion.Euler(Mathf.Clamp(vcam.transform.eulerAngles.x -+ Input.GetAxis("Mouse ScrollWheel")*(Mathf.Exp(0.05f*(90f - transposer.m_CameraDistance))),60f,90f), vcam.transform.eulerAngles.y, vcam.transform.eulerAngles.z);
        //    //Debug.Log(Mathf.Exp(0.05f * (90f - transposer.m_CameraDistance)));


        //    if (transposer.m_CameraDistance >= 30f)
        //    {
        //        vcamZoomed.Priority = 0;
        //        vcam.Priority = 10;
        //        RenderSettings.skybox = skyboxes[1];
        //    }
        //    else
        //    {
        //        vcamZoomed.Priority = 10;
        //        vcam.Priority = 0;
        //        RenderSettings.skybox = skyboxes[0];
        //    }
        //}

    }
}
