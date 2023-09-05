using System;
using UnityEngine;

[Serializable]
public abstract class GhostBehavior : MonoBehaviour
{
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

    //Ran when the ghost collides with a node
    protected virtual void OnNode(Node node)
    {
        if(node == null) return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If a node is hit, run node hit function.
        if(collision.gameObject.layer == LayerMask.NameToLayer("Node"))
            OnNode(collision.gameObject.GetComponent<Node>());
    }

    //Load the appropriate ghost behavior based on type of ghost.
    public static void switchBehavior(Ghost ghost)
    {
        //Cringe
        switch (ghost.gameObject.name)
        {
            case "Cyan":
            case "Orange":
                ghost.swapBehavior(typeof(GhostScatter), 0);
                break;
            default:
                ghost.swapBehavior(typeof(GhostChase), 0);
                break;
        }
    }
}
