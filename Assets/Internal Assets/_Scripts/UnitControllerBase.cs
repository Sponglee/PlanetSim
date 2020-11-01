
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControllerBase : Singleton<UnitControllerBase>
{
    public Node currentNode;


    private void Start()
    {
        transform.position = GridController.Instance.tiles[0, 0].WorldPosition;
        currentNode = GridController.Instance.tiles[0, 0];

        transform.parent = GridController.Instance.tiles[0, 0].gameObject.GetComponent<TileScript>().unitHolder;
    }
    public void MoveToTile(TileScript targetTile)
    {
        Pathfinding.Instance.target = targetTile.tileNode;
    }

}
