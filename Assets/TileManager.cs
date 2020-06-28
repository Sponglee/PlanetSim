using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallexEffect;
    public int rows = 7;

    void Start()
    {
        cam = Camera.main.gameObject;
        startpos = transform.position.x;
        length = 20f;
    }
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallexEffect));
        float dist = (cam.transform.position.x * parallexEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length*(rows-2)) startpos += length*rows;
        else if (temp < startpos - length * (rows - 2)) startpos -= length*rows;
    }
}
