
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControllerBase : Singleton<UnitControllerBase>
{

    public enum UnitStates
    {
        Idle,
        Working,
        Walking,
    }

    private struct DestData : IComparable<DestData>
    {
        public Node node;
        public float distance;

        public int CompareTo(DestData obj)
        {
            return distance.CompareTo(obj.distance);
        }
    }

    public UnitStates unitState;
    public Node currentNode;
    public int lookRadius = 5;

    private Stack<Node> unitPath = new Stack<Node>();

    private void Initialize()
    {
        transform.position = GridController.Instance.tiles[0, 0].WorldPosition;
        currentNode = GridController.Instance.tiles[0, 0];

        // transform.parent = GridController.Instance.tiles[0, 0].gameObject.GetComponent<TileScript>().unitHolder;
    }


    private IEnumerator Start()
    {
        Initialize();
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (unitState == UnitStates.Idle)
            {

                if (CheckForTargetDestination())
                {
                    GetToDestination(targetDestos[0].node);
                }
            }
        }
    }

    public void GetToDestination(Node target)
    {
        if (unitState == UnitStates.Walking)
        {
            return;
        }

        unitPath = Pathfinding.Instance.FindPath(currentNode, targetDestos[0].node);

        if (unitPath.Count > 0)
        {
            StartCoroutine(StartDestMove(unitPath));
        }
    }

    private IEnumerator StartDestMove(Stack<Node> targetNodes)
    {
        unitState = UnitStates.Walking;
        while (targetNodes.Count > 0)
        {
            currentNode = targetNodes.Peek();
            float duration = 1f;
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            Vector3 targetPos = targetNodes.Pop().transform.position;

            Debug.Log(Vector3.Magnitude(targetPos - startPos));
            //Check if u should teleport away
            if (Mathf.Abs(Vector3.Magnitude(targetPos - startPos)) > 2f)
            {
                transform.position = targetPos;
                continue;
            }

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPos;
        }

        targetDestos[0].node.TileState = TileScript.TileStates.Free;
        targetDestos[0].node.gameObject.GetComponent<TileScript>().SetUpTileType(GridController.Instance.tileTypes[0]);
        unitState = UnitStates.Idle; /* FOR NOW, CHANGE TO WORKING */
    }

    private List<DestData> targetDestos = new List<DestData>();

    private bool CheckForTargetDestination()
    {
        targetDestos.Clear();

        Vector2Int startCoord = new Vector2Int((int)currentNode.GridPosition.x - lookRadius, (int)currentNode.GridPosition.y - lookRadius);
        Vector2Int endCoord = new Vector2Int((int)currentNode.GridPosition.x + lookRadius, (int)currentNode.GridPosition.y + lookRadius);

        for (int x = startCoord.x; x < endCoord.x; x++)
        {
            for (int y = startCoord.y; y < endCoord.y; y++)
            {
                int tmpX = x;
                int tmpY = y;

                if (x < 0)
                    tmpX = (int)GridController.Instance.worldSize.x - 1 + x;

                if (y < 0)
                    tmpY = (int)GridController.Instance.worldSize.y - 1 + y;

                if (x >= (int)GridController.Instance.worldSize.x)
                    tmpX = x % (int)GridController.Instance.worldSize.x;

                if (y >= (int)GridController.Instance.worldSize.y)
                    tmpY = y % (int)GridController.Instance.worldSize.y;


                // Debug.Log(tmpX + " : " + tmpY);
                Node tmpTile = GridController.Instance.tiles[tmpX, tmpY];
                if (tmpTile.TileState == TileScript.TileStates.Trees)
                {
                    DestData tmpData = new DestData();
                    tmpData.node = tmpTile;
                    tmpData.distance = Vector2.SqrMagnitude(new Vector2(Mathf.Abs(x - currentNode.GridPosition.x), Mathf.Abs(y - currentNode.GridPosition.y)));
                    targetDestos.Add(tmpData);
                }
            }
        }

        if (targetDestos.Count > 0)
        {
            targetDestos.Sort();
            return true;
        }
        else
            return false;
    }
}
