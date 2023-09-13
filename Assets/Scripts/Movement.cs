using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Required components
[RequireComponent(typeof(Rigidbody2D))]

//Handle movement of pacman & ghost
public class Movement : MonoBehaviour
{
    //Attributes
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    bool frozen;

    //"Internal" variables
    public Rigidbody2D body { get; private set; }
    public new Collider2D collider { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; set; }
    public Vector3 startingPosition { get; private set; }


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        startingPosition = transform.position;
    }

    // Start is called before the first frame update
    private void Start()
    {
        ResetState();
    }

    //Reset script
    public void ResetState()
    {
        //Reset speed
        speedMultiplier = 1.0f;

        //Reset position and direction
        direction = initialDirection;
        nextDirection = Vector3.zero;
        transform.position = startingPosition;
        
        //Disable rigidbody collision
        body.isKinematic = false;

        //Enable script
        enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        //If there is a desired direction, try to go in that direction.
        if (nextDirection != Vector2.zero)
            SetDirection(false);
    }

    //Move body every frame based on direction
    private void FixedUpdate()
    {

        Vector2 position = body.position;
        Vector2 translation = Vector2.zero;
        if(!frozen)
            translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        body.MovePosition(position + translation);
    }

    //Attempt to update the current direction with the desired direction
    public void SetDirection(bool forced)
    {
        //If direction is forced or space in direction is unoccupied, change current direction to next direction.
        if (forced || !Occupied(nextDirection))
        {
            direction = nextDirection;
            nextDirection = Vector2.zero;
        }
    }

    //Check if the spot from the entity to the given direction is occupied
    public bool Occupied(Vector2 direction, bool small = false)
    {
        float shot = 1.5f;
        if (small)
            shot = 0.15f;
        //Check for obstacles from entity, slightly in desired direction. If no obstacles are present, then spot is free.
        RaycastHit2D cast = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, shot, obstacleLayer);
        return cast.collider != null;
    }

    //Enable frozen mode
    public void Freeze()
    {
        frozen = true;
    }

    //Disable frozen mode
    public void Unfreeze()
    {
        frozen = false;
    }
}
