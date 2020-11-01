using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridController : Singleton<GridController>
{
    public GameObject tilePref;
    public Vector2 worldSize;
    public Vector2 tileStep;

    public Transform tileHolder;
    public Transform world;


    public TileType[] tileTypes;
    public Node[,] tiles;






    void Awake()
    {
        InitializeWorld();
    }


    public void InitializeWorld()
    {
        tiles = new Node[(int)worldSize.x, (int)worldSize.y];
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float xPos = x * tileStep.y;
                if (y % 2 == 1)
                    xPos += tileStep.y / 2f;

                int tmpTileType = Random.Range(0, tileTypes.Length);

                GameObject tmpTile = Instantiate(tilePref, tileHolder);
                tmpTile.transform.localPosition = new Vector3(xPos, 0, y * tileStep.x);
                tmpTile.GetComponent<TileScript>().SetUpTileType(tileTypes[tmpTileType]);
                Vector2 tmpGrid = new Vector2(x, y);
                tmpTile.GetComponent<TileScript>().tileNode.GridPosition = tmpGrid;
                tmpTile.GetComponent<TileScript>().tileNode.TileState = tileTypes[tmpTileType].state;

                tiles[x, y] = tmpTile.GetComponent<Node>();
            }
        }
    }


    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = (int)node.GridPosition.x + x;
                int checkY = (int)node.GridPosition.y + y;

                if (checkX >= 0 && checkX < worldSize.x && checkY >= 0 && checkY < worldSize.y)
                {
                    neighbours.Add(tiles[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


}
