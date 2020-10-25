using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : Singleton<WorldController>
{
    public GameObject tilePref;
    public Vector2 worldSize;
    public Vector2 worldOffset;

    public Transform tileHolder;
    public Transform world;







    void Start()
    {
        InitializeWorld();
    }


    public void InitializeWorld()
    {
        // Tiles = new Dictionary<Point, TileScript>();
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float xPos = x * worldOffset.y;
                if (y % 2 == 1)
                    xPos += worldOffset.y / 2f;

                GameObject tmpTile = Instantiate(tilePref, tileHolder);
                tmpTile.transform.localPosition = new Vector3(xPos, 0, y * worldOffset.x);
                // tmpTile.GetComponent<TileScript>().Setup(new Point(x, y), tmpTile.transform.position, tileHolder);
            }
        }

    }



}
