using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : Singleton<WorldController>
{
    public GameObject tilePref;
    public Vector2 worldSize;
    public Vector2 tileStep;

    public Transform tileHolder;
    public Transform world;


    public TileType[] tileTypes;
    int[,] tiles;






    void Start()
    {
        InitializeWorld();
    }


    public void InitializeWorld()
    {
        tiles = new int[(int)worldSize.x, (int)worldSize.y];

        // Tiles = new Dictionary<Point, TileScript>();
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float xPos = x * tileStep.y;
                if (y % 2 == 1)
                    xPos += tileStep.y / 2f;

                tiles[x, y] = Random.Range(0, tileTypes.Length);


                GameObject tmpTile = Instantiate(tilePref, tileHolder);
                tmpTile.transform.localPosition = new Vector3(xPos, 0, y * tileStep.x);
                tmpTile.GetComponent<TileScript>().SetUpTileType(tileTypes[tiles[x, y]]);

                // tmpTile.GetComponent<TileScript>().Setup(new Point(x, y), tmpTile.transform.position, tileHolder);
            }
        }

    }



}
