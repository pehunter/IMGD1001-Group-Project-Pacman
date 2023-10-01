using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GhostScatter : GhostBehavior
{
    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (node.Count == 0 || (!ghost.movement.blocked && ghost.movement.nextDirection != ghost.movement.direction && ghost.movement.nextDirection != Vector2.zero)) return;

        //Pick random direction
        Vector2 newDirection = node[UnityEngine.Random.Range(0, node.Count)];

        //Keep choosing a random direction until it is not equal to current direction
        while (node.Count > 1 && newDirection == -ghost.movement.direction) 
            newDirection = node[UnityEngine.Random.Range(0, node.Count)];

        //Assign nextDirection
        ghost.movement.nextDirection = newDirection;
    }
}
