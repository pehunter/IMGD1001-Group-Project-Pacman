using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public abstract class GhostBehavior : MonoBehaviour
{
    //How often to check for new directions
    public float aiRefreshRate = 0.1f;

    protected float elapsedTime;

    public GameObject test;

    //Ghost this is attached to
    protected Ghost ghost;

    //Duration this state will last for, if it is used.
    public float duration;

    //Whether or not this ghost can enter the frightened state
    public bool frightenable { get; protected set; }

    //Set ghost on wake
    private void Awake()
    {
        //Find Ghost behavior and find Pacman's position.
        ghost = gameObject.GetComponent<Ghost>();

        //Auto-set frightenable to true
        frightenable = true;
    }

    protected void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > aiRefreshRate)
        {
            Vector3 pos = ghost.transform.position + new Vector3(ghost.movement.direction.x, ghost.movement.direction.y, 0) * 2;
            if (!checkMovement(pos))
                checkMovement(ghost.transform.position);
            //elapsedTime = 0f;
        }
    }

    //Check which directions can be entered from given pos
    protected bool checkMovement(Vector3 pos)
    {
        pos = new Vector3(Mathf.Round(pos.x + 0.5f) - 0.5f, Mathf.Round(pos.y + 0.5f) - 0.5f, pos.z);
        List<Vector2> freeDirections = new List<Vector2>();
        foreach (var dir in new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
        {
            var d = CheckDirection(pos, dir);
            if (d != Vector2.zero)
            {
                freeDirections.Add(d);
            }
        }
        OnNode(freeDirections);
        return freeDirections.Count > 0;
    }

    //Performs a Boxcast to see if direction from node is free
    protected Vector2 CheckDirection(Vector3 checkPos, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(checkPos, Vector2.one * 0.5f, 0f, direction, 1f, ghost.movement.obstacleLayer);

        if (hit.collider == null)
            return direction;
        return Vector2.zero;

    }

    //Ran when the ghost collides with a node
    protected virtual void OnNode(List<Vector2> node)
    {
        if(node.Count == 0) return;
    }

    //Load the appropriate ghost behavior based on type of ghost.
    public static void switchBehavior(Ghost ghost)
    {
        //Cringe
        switch (ghost.gameObject.name)
        {
            case "Cyan":
            case "Orange":
            case "Orange - Bomber":
                ghost.swapBehavior(typeof(GhostScatter), 0);
                break;
            case "Pink":
                ghost.swapBehavior(typeof(GhostCutoff), 0);
                break;
            default:
                ghost.swapBehavior(typeof(GhostChase), 0);
                break;
        }
    }
}
