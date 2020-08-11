﻿
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
    public float jumpRow = 20f;
    public float length = 1.6f;

    public Vector3 difference;

    [Header("")]
    public GameObject[] contentPrefs;

    private bool isVisible = true;

    public Transform contentHolder;

    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (value == false)
            {
                // UpdateTile();
            }
            isVisible = value;
        }


    }

    void Start()
    {
        // RTS_Camera.OnCameraMoved.AddListener(UpdateTile);

        GameObject tmpContent = Instantiate(contentPrefs[Random.Range(0, contentPrefs.Length)], contentHolder);

        tmpContent.transform.Rotate(Vector3.up, Random.Range(0f, 360f));


        int seed = Random.Range(0, 100);

        if (seed < 50)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        cam = Camera.main.gameObject;
        startPosX = transform.position.x;
        startPosZ = transform.position.z;


        UpdateTilePosition(cam.transform.position.x, 0);
        UpdateTilePosition(cam.transform.position.z, 1);
    }


    void Update()
    {
        // Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam.GetComponent<Camera>());
        // if (!IsVisible && GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        // {
        //     IsVisible = true;
        // }
        // else if (IsVisible && !GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        // {
        //     IsVisible = false;
        // }

        difference = (cam.transform.position - transform.position);

        if (Mathf.Abs(difference.x) > 17f)
        {
            UpdateTilePosition(cam.transform.position.x, 0);
        }
        if (Mathf.Abs(difference.z) > 17f)
        {
            UpdateTilePosition(cam.transform.position.z, 1);
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



        //Update position projection coordinate depending on axis
        if (axis == 0)
        {
            calculatedStartPos += length * rows * Mathf.Sign(camProjectionCoord - transform.position.x);
            startPosX = calculatedStartPos;
            transform.position = new Vector3(calculatedStartPos, transform.position.y, transform.position.z);
        }
        else if (axis == 1)
        {
            calculatedStartPos += length * rows * Mathf.Sign(camProjectionCoord - transform.position.z);
            startPosZ = calculatedStartPos;
            transform.position = new Vector3(transform.position.x, transform.position.y, calculatedStartPos);
        }
    }

}
