using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCutoff : GhostBehavior
{
    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (node.Count == 0 || (ghost.movement.nextDirection != ghost.movement.direction && ghost.movement.nextDirection != Vector2.zero)) return;

        float minDist = 1000;
        Vector2 newDirection = Vector2.zero;
        foreach(Vector2 dir in node) {
            //Target point is a bit infront of pacman
            var infront = Physics2D.Raycast(new Vector2(ghost.pacman.transform.position.x, ghost.pacman.transform.position.y), ghost.pacman.movement.direction, 50f, ghost.movement.obstacleLayer);
            Vector2 target = infront.point;
            //var template = Instantiate(test);
            //template.transform.position = new Vector3(target.x, target.y, -1);
            //Get distance from 1 unit in this direction to target
            float dist = (dir + new Vector2(transform.position.x, transform.position.y) - target).magnitude;

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
        //if (ghost.movement.direction != newDirection)
        //    elapsedTime = -0.4f;
        //else
        elapsedTime = 0;
    }
}
