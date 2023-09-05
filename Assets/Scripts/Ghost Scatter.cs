using System;
using UnityEngine;

[Serializable]
public class GhostScatter : GhostBehavior
{
    protected override void OnNode(Node node)
    {
        //Null-check
        base.OnNode(node);


        //Pick random direction
        Vector2 newDirection = node.availableDirections[UnityEngine.Random.Range(0, node.availableDirections.Count)];

        //Keep choosing a random direction until it is not equal to current direction
        while (node.availableDirections.Count > 1 && newDirection == -ghost.movement.direction) 
            newDirection = node.availableDirections[UnityEngine.Random.Range(0, node.availableDirections.Count)];

        //Assign nextDirection
        ghost.movement.nextDirection = newDirection;
    }
}
