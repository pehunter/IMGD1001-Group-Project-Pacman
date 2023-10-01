using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

[Serializable]
public class GhostFrightened : GhostBehavior
{


    //Flag if ghost was eaten
    public bool eaten { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //Enter blue state
        ghost.bodySprite.enabled = false;
        ghost.eyesSprite.enabled = false;
        ghost.blueSprite.enabled = true;
        ghost.whiteSprite.enabled = false;

        //Slow down ghost
        ghost.movement.speedMultiplier = 0.5f;

        //Invoke white flashing state after 50% of duration
        Invoke(nameof(Flash), duration * 0.5f);
    }

    //Enter white flashing state, if not eaten.
    private void Flash()
    {
        if (!eaten)
        {
            ghost.blueSprite.enabled = false;
            ghost.whiteSprite.enabled = true;
            ghost.whiteSprite.GetComponent<SpriteAnimator>().Restart();

            //Revert back to regular behavior
            Invoke(nameof(Revert), duration * 0.5f);
        }
    }

    public void Eaten()
    {
        //Mark as eaten
        eaten = true;

        //Set behavior to dead
        ghost.swapBehavior(typeof(GhostDead), ghost.respawnTime);
    }

    //Move away from pacman
    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (node.Count == 0 || (!ghost.movement.blocked && ghost.movement.nextDirection != ghost.movement.direction && ghost.movement.nextDirection != Vector2.zero)) return;

        float maxDist = 0;
        Vector2 newDirection = Vector2.zero;
        foreach (Vector2 dir in node)
        {
            //Get distance from 1 unit in this direction to pacman
            float dist = (dir + new Vector2(transform.position.x, transform.position.y) - new Vector2(ghost.pacman.transform.position.x, ghost.pacman.transform.position.y)).magnitude;

            //Don't move in reverse!
            if (dist > maxDist && dir != -ghost.movement.direction)
            {
                maxDist = dist;
                newDirection = dir;
            }
        }


        //Assign nextDirection
        ghost.movement.nextDirection = newDirection;
    }

    //If not eaten, replace behavior with appropriate behavior.
    private void Revert()
    {
        if (!eaten)
        {
            //Make ghost normal
            ghost.bodySprite.enabled = true;
            ghost.eyesSprite.enabled = true;
            ghost.blueSprite.enabled = false;
            ghost.whiteSprite.enabled = false;
            ghost.movement.speedMultiplier = 1f;

            GhostBehavior.switchBehavior(ghost);
        }
    }
}
