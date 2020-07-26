
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private float length, startPosX, startPosZ;
    public GameObject cam;
    public float parallexEffect;
    public int rows = 7;
    public int jumpRow = 4;

    [Header("")]
    public GameObject[] contentPrefs;

    void Start()
    {
        RTS_Camera.OnCameraMoved.AddListener(UpdateTile);

        // GameObject tmpContent = Instantiate(contentPrefs[Random.Range(0, contentPrefs.Length)], transform);

        // tmpContent.transform.Rotate(Vector3.up,  Random.Range(0f, 360f));


        cam = Camera.main.gameObject;
        startPosX = transform.position.x;
        startPosZ = transform.position.z;
        length = 20f;

        UpdateTilePosition(cam.transform.position.x,0);
        UpdateTilePosition(cam.transform.position.z,1);
    }


    private void UpdateTile()
    {
        UpdateTilePosition(cam.transform.position.x, 0);
        UpdateTilePosition(cam.transform.position.z, 1);
    }

    //Update tiles depending on what axis is needed (0 - x, 1 - z)
    private void UpdateTilePosition(float camProjectionCoord, int axis)
    {
        //Pick a start position projection coordinate
        float calculatedStartPos = (axis == 0? startPosX:startPosZ);
        float camPos = camProjectionCoord;
       

        //Move depending on axis
        if(axis == 0)
            transform.position = new Vector3(calculatedStartPos, transform.position.y, transform.position.z);
        else if (axis == 1)
            transform.position = new Vector3(transform.position.x, transform.position.y, calculatedStartPos);

        //Check if needed to jump
        if (camPos > calculatedStartPos + length * (jumpRow))
        {
            //Debug.Log(transform.name + " = " + camPos + " : " + (calculatedStartPos + length * (jumpRow)));
            calculatedStartPos += length * rows;
        }
        else if (camPos < calculatedStartPos - length * (jumpRow))
        {
            //Debug.Log(transform.name + " = " + "-- " + camPos + " : " + (calculatedStartPos + length * (jumpRow)));
            calculatedStartPos -= length * rows;       
        }

        //Update position projection coordinate depending on axis
        if (axis == 0)
            startPosX = calculatedStartPos;
        else if (axis == 1)
            startPosZ = calculatedStartPos;
    }

}
