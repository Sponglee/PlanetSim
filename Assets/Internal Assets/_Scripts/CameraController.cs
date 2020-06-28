using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1f;  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            transform.Translate(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed  );
        }
    }
}
