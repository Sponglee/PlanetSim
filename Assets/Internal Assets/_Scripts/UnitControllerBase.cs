
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControllerBase : Singleton<UnitControllerBase>
{
    public Node currentNode;

    private void Initialize()
    {
        transform.position = GridController.Instance.tiles[0, 0].WorldPosition;
        currentNode = GridController.Instance.tiles[0, 0];

        transform.parent = GridController.Instance.tiles[0, 0].gameObject.GetComponent<TileScript>().unitHolder;
    }

    private bool DestinationActive = false;

    private void Start()
    {
        Initialize();
        // while (true)
        // {
        //     yield return new WaitForSeconds(1f);

        // }


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!DestinationActive)
                GetToDestination();
        }
    }

    public void GetToDestination()
    {


        if (DestinationActive && Pathfinding.Instance.testPath.Count == 0)
        {
            return;
        }


        if (Pathfinding.Instance.testPath != null)
        {

            Stack<Node> tmpDest = new Stack<Node>(Pathfinding.Instance.testPath);
            StartCoroutine(StartDestMove(tmpDest));

        }
    }

    private IEnumerator StartDestMove(Stack<Node> targetNodes)
    {
        DestinationActive = true;
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

        DestinationActive = false;
    }
    public void MoveToTile(TileScript targetTile)
    {
        Pathfinding.Instance.target = targetTile.tileNode;
    }

}
