using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : Singleton<Pathfinding>
{
    public GridController grid;


    public Node seeker;
    public Node target;


    private void Update()
    {
        if (target != null)
            FindPath(UnitControllerBase.Instance.currentNode, target);
    }


    public void FindPath(Node startNode, Node targetNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (neighbour.TileState != TileScript.TileStates.Free || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.nodeParent = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    public void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.nodeParent;
        }

        path.Reverse();
        ///////////////
        testPath = path;
    }

    public List<Node> testPath;


    private void OnDrawGizmos()
    {
        if (grid.tiles == null)
            return;

        foreach (Node n in grid.tiles)
        {
            Gizmos.color = Color.green;

            if (n.GridPosition == Vector2.zero)
                Gizmos.color = Color.red;

            if (testPath != null)
                if (testPath.Contains(n))
                {
                    Gizmos.color = Color.black;
                    // Gizmos.DrawIcon(n.GridPosition, n.name, true);
                }
            Gizmos.DrawCube(n.WorldPosition + Vector3.up, Vector3.one * 1f);
        }
    }

    //Get closest distance in 4 "mirrors" check which side is closer to move to
    int GetDistance(Node nodeA, Node nodeB)
    {

        int closestX = 99;
        int closestY = 99;

        for (int i = -1; i < 1; i++)
        {
            for (int j = -1; j < 1; j++)
            {
                if (Mathf.Abs((int)nodeA.GridPosition.x - (nodeB.GridPosition.x + (int)i * grid.worldSize.x)) < closestX)
                    closestX = Mathf.Abs((int)nodeB.GridPosition.x + i * (int)grid.worldSize.x);
                if (Mathf.Abs((int)nodeA.GridPosition.y - (nodeB.GridPosition.y + (int)j * grid.worldSize.y)) < closestY)
                    closestX = Mathf.Abs((int)nodeB.GridPosition.y + i * (int)grid.worldSize.y);
            }
        }




        int distX = Mathf.Abs((int)nodeA.GridPosition.x - (int)closestX);
        int distY = Mathf.Abs((int)nodeA.GridPosition.y - (int)closestY);






        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);

    }
}

