using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //Check all four directions and add to availability
        availableDirections = new List<Vector2>();
        CheckDirection(Vector2.up);
        CheckDirection(Vector2.down);
        CheckDirection(Vector2.left);
        CheckDirection(Vector2.right);
    }

    //Performs a Boxcast to see if direction from node is free
    private void CheckDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        if (hit.collider == null)
            availableDirections.Add(direction);
            
    }
}
