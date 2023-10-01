using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (node.Count == 0 || (!ghost.movement.blocked && ghost.movement.nextDirection != ghost.movement.direction && ghost.movement.nextDirection != Vector2.zero)) return;

        float minDist = 1000;
        Vector2 newDirection = Vector2.zero;
        foreach(Vector2 dir in node) {
            //Get distance from 1 unit in this direction to pacman
            float dist = (dir + new Vector2(transform.position.x, transform.position.y) - new Vector2(ghost.pacman.transform.position.x, ghost.pacman.transform.position.y)).magnitude;

            //If this direction is closer than the last minimum, make this the new minimum
            //And don't move in reverse!
            if(dist < minDist && dir != -ghost.movement.direction)
            {
                minDist = dist;
                newDirection = dir;
            }
        }


        //Assign nextDirection
        ghost.movement.nextDirection = newDirection;

        elapsedTime = 0;
    }
}
