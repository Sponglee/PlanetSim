using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private float length, startposX, startposY;
    public GameObject cam;
    public float parallexEffect;
    public int rows = 7;
    public int jumpRow = 4;

    void Start()
    {
        cam = Camera.main.gameObject;
        startposX = transform.position.x;
        startposY = transform.position.z;

        length = 20f;
    }
    void Update()
    {
       if(Input.GetAxis("Horizontal")!= 0)
        {
            float temp = (cam.transform.position.x * (1 - parallexEffect));
            float dist = (cam.transform.position.x * parallexEffect);
            transform.position = new Vector3(startposX + dist, transform.position.y, transform.position.z);

            if (temp > startposX + length * (jumpRow))
                startposX += length * rows;
            else if (temp < startposX - length * (jumpRow))
                startposX -= length * rows;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            float temp = (cam.transform.position.z * (1 - parallexEffect));
            float dist = (cam.transform.position.z * parallexEffect);

            transform.position = new Vector3(transform.position.x,transform.position.y, startposY + dist);

            if (temp > startposY + length * (jumpRow))
                startposY += length * rows;
            else if (temp < startposY - length * (jumpRow))
                startposY -= length * rows;
        }
    }
}
