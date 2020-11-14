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
                tmpTile.GetComponent<Node>().TileState = GetTileStateFromIndex(tmpTileType);
                tiles[x, y] = tmpTile.GetComponent<Node>();
            }
        }
    }

    private TileScript.TileStates GetTileStateFromIndex(int index)
    {
        TileScript.TileStates tmpState;
        switch (index)
        {
            case 0:
                tmpState = TileScript.TileStates.Unwalkable;
                break;
            case 1:
                tmpState = TileScript.TileStates.Free;
                break;
            case 2:
                tmpState = TileScript.TileStates.Trees;
                break;
            case 3:
                tmpState = TileScript.TileStates.Unwalkable;
                break;
            default:
                tmpState = TileScript.TileStates.Free;
                break;
        }

        return tmpState;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        // Debug.Log("======");

        if (node.GridPosition.y % 2 != 0)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Skip one we're standing on
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = (int)node.GridPosition.x + x;
                    int checkY = (int)node.GridPosition.y + y;
                    Vector2Int newCheck = CheckSign(checkX, checkY);
                    neighbours.Add(tiles[newCheck.x, newCheck.y]);
                }
            }
            //Left tile
            Vector2Int newLeftSideCheck = CheckSign((int)node.GridPosition.x - 1, (int)node.GridPosition.y);
            neighbours.Add(tiles[newLeftSideCheck.x, newLeftSideCheck.y]);
        }
        else
        {
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Skip one we're standing on
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = (int)node.GridPosition.x + x;
                    int checkY = (int)node.GridPosition.y + y;
                    Vector2Int newCheck = CheckSign(checkX, checkY);
                    neighbours.Add(tiles[newCheck.x, newCheck.y]);
                }
            }
            //Left tile
            Vector2Int newLeftSideCheck = CheckSign((int)node.GridPosition.x + 1, (int)node.GridPosition.y);
            neighbours.Add(tiles[newLeftSideCheck.x, newLeftSideCheck.y]);
        }


        return neighbours;
    }


    private Vector2Int CheckSign(int checkX, int checkY)
    {
        if (checkX < 0)
            checkX = (int)worldSize.x - 1;

        if (checkY < 0)
            checkY = (int)worldSize.y - 1;

        if (checkX >= (int)worldSize.x)
            checkX = 0;
        if (checkY >= (int)worldSize.y)
            checkY = 0;

        return new Vector2Int(checkX, checkY);
    }
}
