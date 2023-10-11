using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostZoom : GhostBehavior
{
    //How long to pause when wall hit.
    public float chargeTime = 1f;
    public float chargeBuff = 0.7f;

    //Note: Original speed serves as speed!

    private float chargeElapsedTime;
    bool canMove = false;

    private void Start()
    {
        ghost = GetComponent<Ghost>();
        ghost.movement.speed = 0f;
        if (DifficultyManager.hard)
            chargeTime *= chargeBuff;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!ghost.frozen)
        {
            chargeElapsedTime += Time.fixedDeltaTime;
            //Upon charge, set speed to zoom value
            if(chargeElapsedTime > chargeTime)
            {
                ghost.movement.speed = ghost.initialSpeed;
                canMove = true;
            }
        }
    }

    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (!canMove || node.Count == 0 || (!ghost.movement.blocked && ghost.movement.nextDirection != ghost.movement.direction && ghost.movement.nextDirection != Vector2.zero)) return;
        print(gameObject.name);
        float minDist = 1000;
        Vector2 newDirection = Vector2.zero;
        foreach (Vector2 dir in node)
        {
            //Get distance from 1 unit in this direction to pacman
            float dist = (dir + new Vector2(transform.position.x, transform.position.y) - new Vector2(ghost.pacman.transform.position.x, ghost.pacman.transform.position.y)).magnitude;

            //If this direction is closer than the last minimum, make this the new minimum
            //And don't move in reverse!
            if (dist < minDist && dir != -ghost.movement.direction)
            {
                minDist = dist;
                newDirection = dir;
            }
        }


        //Assign nextDirection
        ghost.movement.nextDirection = newDirection;
        chargeElapsedTime = 0f;
        canMove = false;
        elapsedTime = 0;
    }
}
