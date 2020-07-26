using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject tilePref;
    public Vector2 worldSize;
    public Vector2 worldOffset;



    void Start()
    {
        InitializeWorld();
    }


    public void InitializeWorld()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float xPos = x * worldOffset.y;
                if (y % 2 == 1)
                    xPos += worldOffset.y / 2f;

                GameObject tmpTile = Instantiate(tilePref, transform);
                tmpTile.transform.localPosition = new Vector3(xPos, 0, y * worldOffset.x);
            }
        }

    }
}
