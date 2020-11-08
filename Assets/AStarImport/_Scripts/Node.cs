using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileScript;

public class Node : MonoBehaviour
{
    public Node nodeParent;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }


    public TileStates TileState = TileStates.Free;

    [SerializeField] private Vector2 gridPosition;
    public Vector2 GridPosition
    {
        get
        {
            return gridPosition;
        }
        set
        {
            gridPosition = value;
        }
    }


    public Vector3 WorldPosition
    {
        get { return new Vector3(transform.position.x, transform.position.y, transform.position.z); }
    }

}
