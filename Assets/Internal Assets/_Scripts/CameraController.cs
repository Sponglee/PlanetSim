﻿using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1f;
    public float scrollSpeed = 1f;

    public Material[] skyboxes;


    public CinemachineVirtualCamera vcam;
    CinemachineFramingTransposer transposer;

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            transform.Translate(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed  );
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
           

            transposer.m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            transposer.m_CameraDistance = Mathf.Clamp(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance, 8f, 90f);


            vcam.transform.rotation = Quaternion.Euler(Mathf.Clamp(vcam.transform.eulerAngles.x -+ Input.GetAxis("Mouse ScrollWheel")*(Mathf.Exp(0.05f*(90f - transposer.m_CameraDistance))),60f,90f), vcam.transform.eulerAngles.y, vcam.transform.eulerAngles.z);
            Debug.Log(Mathf.Exp(0.05f * (90f - transposer.m_CameraDistance)));


            if(transposer.m_CameraDistance >= 15f)
            {
                RenderSettings.skybox = skyboxes[1];
            }
            else
            {
                RenderSettings.skybox = skyboxes[0];
            }
        }

    }
}
