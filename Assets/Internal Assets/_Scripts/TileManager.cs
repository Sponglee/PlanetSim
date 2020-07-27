
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TileManager : MonoBehaviour
{
    private float startPosX, startPosZ;
    public GameObject cam;
    public float parallexEffect;
    public int rows = 50;
    public int jumpRow = 30;
    public float length = 1.6f;

    [Header("")]
    public GameObject[] contentPrefs;

    private bool isVisible = true;

    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (value != isVisible)
            {
                UpdateTile();
            }
            isVisible = value;
        }


    }

    void Start()
    {
        // RTS_Camera.OnCameraMoved.AddListener(UpdateTile);

        // // GameObject tmpContent = Instantiate(contentPrefs[Random.Range(0, contentPrefs.Length)], transform);

        // // tmpContent.transform.Rotate(Vector3.up,  Random.Range(0f, 360f));


        cam = Camera.main.gameObject;
        startPosX = transform.position.x;
        startPosZ = transform.position.z;


        UpdateTilePosition(cam.transform.position.x, 0);
        UpdateTilePosition(cam.transform.position.z, 1);
    }


    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam.GetComponent<Camera>());
        if (!IsVisible && GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        {
            IsVisible = true;
        }
        else if (IsVisible && !GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        {
            IsVisible = false;
        }

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
        float calculatedStartPos = (axis == 0 ? startPosX : startPosZ);
        float camPos = camProjectionCoord;

        // Debug.Log(camPos + " : " + calculatedStartPos + length * (jumpRow));
        //Move depending on axis

        // if (axis == 0)
        //     transform.position = new Vector3(calculatedStartPos, transform.position.y, transform.position.z);
        // else if (axis == 1)
        //     transform.position = new Vector3(transform.position.x, transform.position.y, calculatedStartPos);

        //Check if needed to jump
        if (Mathf.Abs(camPos) > Mathf.Abs(calculatedStartPos + length * (jumpRow)))
        {
            Debug.Log(transform.name + " = " + camPos + " : " + (calculatedStartPos + length * (jumpRow)));
            calculatedStartPos += length * rows;
        }
        else if (Mathf.Abs(camPos) < Mathf.Abs(calculatedStartPos - length * (jumpRow)))
        {
            Debug.Log(transform.name + " = " + "-- " + camPos + " : " + (calculatedStartPos + length * (jumpRow)));
            calculatedStartPos -= length * rows;
        }

        //Update position projection coordinate depending on axis
        if (axis == 0)
            startPosX = calculatedStartPos;
        else if (axis == 1)
            startPosZ = calculatedStartPos;
    }

}
