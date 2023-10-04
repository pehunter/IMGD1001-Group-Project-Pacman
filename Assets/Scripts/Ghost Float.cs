using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFloat : GhostBehavior
{
    private void Start()
    {
        this.aiRefreshRate = 0.02f;
        this.force = true;
    }

    protected override void OnNode(List<Vector2> node)
    {
        //Null-check
        base.OnNode(node);

        if (node.Count == 0) return;

        Vector3 diff = ghost.pacman.transform.position - ghost.transform.position;
        Vector2 dir = new Vector2(diff.x, diff.y).normalized;


        //Assign nextDirection
        ghost.movement.nextDirection = dir;
        ghost.movement.SetDirection(true);

        elapsedTime = 0;
    }
}
