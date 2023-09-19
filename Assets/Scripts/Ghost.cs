using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int points = 200;
    public Movement movement { get; private set; }

    //Track whether in frightened state or not
    private bool frightened;

    //Starting behavior for this ghost
    public string initialBehavior;
    public float initialDuration;

    //How long it takes the ghost to respawn
    public float respawnTime;

    //Currently set behavior
    private GhostBehavior currentBehavior;

    //Next behavior logic
    private float nextDuration;
    private Type nextBehavior;
    bool needToUpdate;

    //Mark inside and outside points for home
    public Transform inside;
    public Transform outside;

    //Parts of ghost
    public GameObject body;
    public GameObject eyes;
    public GameObject blue;
    public GameObject white;

    public bool frozen;

    //Renderers for each part of the ghost
    public SpriteRenderer bodySprite { get; private set; }
    public SpriteRenderer eyesSprite {get; private set;}
    public SpriteRenderer blueSprite {get; private set;}
    public SpriteRenderer whiteSprite {get; private set;}


    //Sprite animators for each part of the ghost
    public SpriteAnimator bodyAnimation { get; private set; }
    public SpriteAnimator blueAnimation { get; private set; }
    public SpriteAnimator whiteAnimation { get; private set; }

    //Pacman
    public Pacman pacman;

    void Awake()
    {
        frightened = false;
        movement = GetComponent<Movement>();

        //Set components
        bodySprite = body.GetComponent<SpriteRenderer>();
        eyesSprite = eyes.GetComponent<SpriteRenderer>();
        blueSprite = blue.GetComponent<SpriteRenderer>();
        whiteSprite = white.GetComponent<SpriteRenderer>();

        bodyAnimation = body.GetComponent<SpriteAnimator>();
        whiteAnimation = white.GetComponent<SpriteAnimator>();
        blueAnimation = blue.GetComponent<SpriteAnimator>();
    }

    public void ResetState()
    {
        //If ghost has bomb, reset that.
        if (GetComponent<GhostBomb>() != null)
            GetComponent<GhostBomb>().ResetState();

        //Reset renderers
        bodySprite.enabled = true;
        eyesSprite.enabled = true;
        blueSprite.enabled = false;
        whiteSprite.enabled = false;

        //Reactivate gameObject, re-enable colliders and reset movement
        gameObject.SetActive(true);
        movement.body.isKinematic = false;
        movement.collider.enabled = true;
        movement.ResetState();

        swapBehavior(Type.GetType(initialBehavior), initialDuration, true);
    }

    private void Start()
    {
        //Add initial behavior
        swapBehavior(Type.GetType(initialBehavior), initialDuration);
    }

    private void FixedUpdate()
    {
        if(needToUpdate)
        {
            needToUpdate = false;
            var behavior = (GhostBehavior)gameObject.AddComponent(nextBehavior);
            behavior.duration = nextDuration;
        }
    }

    public void swapBehavior(Type behavior, float duration, bool forced = false)
    {
        GhostBehavior currentBehavior = gameObject.GetComponent<GhostBehavior>();
        //Do not update behavior if in home state still!
        //Remove current behavior
        Destroy(gameObject.GetComponent<GhostBehavior>());

        //Add new behavior if it is a GhostBehavior, and set duration.
        if (behavior.BaseType == typeof(GhostBehavior))
        {
            nextBehavior = behavior;
            nextDuration = duration;

            //Set frightened state
            frightened = behavior == typeof(GhostFrightened);

            needToUpdate = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If ghost has collided with pacman, either eat the ghost or eat pacman depending on frightened state.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman") && GetComponent<GhostBehavior>().GetType() != typeof(GhostDead))
            if (frightened)
                FindObjectOfType<GameManager>().GhostEaten(this);
            else
                FindObjectOfType<GameManager>().PacmanEaten();
    }

    //Set ghost position while preserving z position.
    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    //Freeze the ghost in place
    public void Freeze()
    {
        frozen = true;
        movement.Freeze();
        bodyAnimation.Freeze();
        whiteAnimation.Freeze();
        blueAnimation.Freeze();
    }

    //Unfreeze the ghost
    public void Unfreeze()
    {
        frozen = false;
        movement.Unfreeze();
        bodyAnimation.Unfreeze();
        whiteAnimation.Unfreeze();
        blueAnimation.Unfreeze();
    }
}
