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

    public Dictionary<Point, TileScript> Tiles { get; set; }



    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            //if (path == null)
            //{
            //    //Makes a Path between cTMP position and Redspawn
            //    GeneratePath(BlueSpawn);
            //}
            return new Stack<Node>(new Stack<Node>(path));
        }
        set { path = value; }
    }

    //Temporary position
    Point tmp;
    public Point Tmp
    {
        get
        {
            return tmp;
        }

        set
        {
            tmp = value;
        }
    }








    void Start()
    {
        InitializeWorld();
    }


    public void InitializeWorld()
    {
        Tiles = new Dictionary<Point, TileScript>();
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                float xPos = x * worldOffset.y;
                if (y % 2 == 1)
                    xPos += worldOffset.y / 2f;

                GameObject tmpTile = Instantiate(tilePref, tileHolder);
                tmpTile.transform.localPosition = new Vector3(xPos, 0, y * worldOffset.x);
                tmpTile.GetComponent<TileScript>().Setup(new Point(x, y), tmpTile.transform.position, tileHolder);
            }
        }

    }



    // public Stack<Node> GeneratePath(Point spawn)
    // {
    //     if (AStar.Obstacles == null)
    //         AStar.NewGoal = false;

    //     path = AStar.GetPath(spawn, spawn);


    //     if (AStar.NewGoal)
    //     {

    //         //If path to redSpawn is unreachable turn on "NEW GoAL" mode to get to random obstacle
    //         //closest F score 
    //         if (AStar.Obstacles.Count != 0)
    //         {
    //             Node closestEnemy = AStar.Obstacles.OrderBy(n => n.F).First();

    //             Tmp = closestEnemy.GridPosition;
    //         }

    //         //Check if 
    //         for (int x = -1; x <= 1; x++)
    //         {
    //             for (int y = -1; y <= 1; y++)
    //             {
    //                 Point neighbourPos = new Point(Tmp.X - x, Tmp.Y - y);
    //                 // Inbounds checks "offgrid" cases and Walkables
    //                 if (LevelManager.Instance.InBounds(neighbourPos) && neighbourPos != Tmp)
    //                 {
    //                     path = AStar.GetPath(spawn, Tmp);
    //                     return path;
    //                 }
    //             }
    //         }

    //     }
    //     return path;
    // }
}
