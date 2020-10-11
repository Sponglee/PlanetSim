
using System.Collections.Generic;
using UnityEngine;

public class UnitControllerBase : MonoBehaviour
{


    [SerializeField]
    private float speed = 1f;
    private Stack<Node> path;

    public Point GridPosition { get; set; }
    private Vector3 destination;


    private bool mRePath;
    public bool MRePath
    {
        get
        {
            return mRePath;
        }

        set
        {
            mRePath = value;
        }
    }


    private Point currentTilePos;
    public Point CurrentTilePos
    {
        get
        {
            return currentTilePos;
        }

        set
        {
            currentTilePos = value;
        }
    }


    public void RePath()
    {

        //Reset previous path
        path = null;
        //Toggle switch of start to this current tile
        AStar.firstCurrent = true;
        //this current start tile

        //<----
        AStar.NewGoal = false;

        //this.GetComponent<MonsterRange>().Target = null;
        //Clear obstacles
        AStar.Obstacles.Clear();

        path = LevelManager.Instance.GeneratePath(CurrentTilePos);
        //Set it as Path for this Monster
        SetPath(path);


        //Move 
        //Move();

        //<---- wrong

    }


    private void Move()
    {
        if (true)
        {

            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
                else if (path.Count == 0 && GridPosition != LevelManager.Instance.RedSpawn)
                {
                    this.MRePath = true;
                }
            }
        }

    }


    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            path = newPath;
            if (path.Count > 0)
            {
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            }

        }

    }

}
