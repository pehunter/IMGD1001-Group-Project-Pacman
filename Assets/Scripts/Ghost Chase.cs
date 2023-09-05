using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    protected override void OnNode(Node node)
    {
        //Null-check
        base.OnNode(node);


        float minDist = 1000;
        Vector2 newDirection = Vector2.zero;
        foreach(Vector2 dir in node.availableDirections) {
            //Get distance from 1 unit in this direction to pacman
            float dist = (new Vector3(dir.x, dir.y, 0) + transform.position - ghost.pacman.transform.position).sqrMagnitude;

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
    }
}
