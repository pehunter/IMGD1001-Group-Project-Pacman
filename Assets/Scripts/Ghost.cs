using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Ghost : MonoBehaviour
{
    public int points = 200;
    public Movement movement { get; private set; }

    //Track whether in frightened state or not
    private bool frightened;

    //Starting behavior for this ghost
    public string initialBehavior;
    public float initialDuration;
    public float initialSpeed;

    //How long it takes the ghost to respawn
    public float respawnTime;

    public GameObject test;

    //Currently set behavior
    private GhostBehavior currentBehavior;

    //Next behavior logic
    private float nextDuration;
    private Type nextBehavior;
    public bool needToUpdate;

    //Mark inside and outside points for home
    public Transform inside;
    public Transform outside;

    //Parts of ghost
    public GameObject body;
    public GameObject eyes;
    public GameObject blue;
    public GameObject white;

    public bool frozen;

    //variables for ghost bobbing 
    //bobSpeed -> multiply this by timedelta to get rise-fall speed
    private float bobSpeed = 0.9f;
    private float timer;
    private bool pauseBob;
    private string bobDirection;

    AudioSource source;

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

    //Speed buff for hardmode
    public float speedBuff;

    void Awake()
    {
        frightened = false;
        movement = GetComponent<Movement>();
        source = GetComponent<AudioSource>();

        //Set components
        bodySprite = body.GetComponent<SpriteRenderer>();
        eyesSprite = eyes.GetComponent<SpriteRenderer>();
        blueSprite = blue.GetComponent<SpriteRenderer>();
        whiteSprite = white.GetComponent<SpriteRenderer>();

        bodyAnimation = body.GetComponent<SpriteAnimator>();
        whiteAnimation = white.GetComponent<SpriteAnimator>();
        blueAnimation = blue.GetComponent<SpriteAnimator>();

        timer = 0;
        bobDirection = "up";
        pauseBob = true;
        initialSpeed = movement.speed;
    }

    public void ResetState()
    {
        //If ghost has bomb, reset that.
        if (GetComponent<GhostBomb>() != null)
            GetComponent<GhostBomb>().ResetState(initialBehavior != "GhostHome");

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
        if (DifficultyManager.hard)
            movement.speed += speedBuff;
            
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
            behavior.test = test;
        }
    }

    public void swapBehavior(Type behavior, float duration, bool forced = false)
    {
        GhostBehavior currentBehavior = gameObject.GetComponent<GhostBehavior>();
        //Do not update behavior if in home state still!
        //Remove current behavior
        Destroy(gameObject.GetComponent<GhostBehavior>());
        body.transform.localPosition = Vector3.zero;
        eyes.transform.localPosition = new Vector3(0,0,-1);

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
            {
                if (!VolumeManager.muted)
                {
                    source.pitch = 0.93f + FindObjectOfType<GameManager>().ghostMultiplier / 15f;
                    source.Play();
                }
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
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

    //bob up and down while in Home 
    public void Bob(float timeDelta)
    {
        //counts frames until it's time to move again
        if(bobDirection == "up")
        {
            body.transform.localPosition += new Vector3(0, timeDelta*bobSpeed, 0);
            eyes.transform.localPosition = body.transform.localPosition + new Vector3(0, 0, -1);
            movement.nextDirection = Vector2.up;
            movement.SetDirection(true);
            //this.SetPosition(new Vector3(this.transform.position.x, this.transform.position.y - 0.02f, this.transform.position.z));
            if (body.transform.localPosition.y >= 0.5)
            {
                bobDirection = "down";
                pauseBob = true;
                timer = 0;
            }
        }
        if(bobDirection == "down")
        {
            body.transform.localPosition += new Vector3(0, -timeDelta * bobSpeed, 0);
            eyes.transform.localPosition = body.transform.localPosition + new Vector3(0,0,-1);
            movement.nextDirection = Vector2.down;
            movement.SetDirection(true);
            //this.SetPosition(new Vector3(this.transform.position.x, this.transform.position.y - 0.02f, this.transform.position.z));
            if (body.transform.localPosition.y <= -0.5)
            {
                bobDirection = "up";
                pauseBob = true;
                timer = 0;
            }
        }
    }

    //Freeze the ghost in place
    public void Freeze()
    {
        frozen = true;
        if (GetComponent<GhostBomb>() != null)
            GetComponent<GhostBomb>().Freeze();
        movement.Freeze();
        bodyAnimation.Freeze();
        whiteAnimation.Freeze();
        blueAnimation.Freeze();
    }

    //Unfreeze the ghost
    public void Unfreeze()
    {
        frozen = false;
        if (GetComponent<GhostBomb>() != null)
            GetComponent<GhostBomb>().Unfreeze();
        movement.Unfreeze();
        bodyAnimation.Unfreeze();
        whiteAnimation.Unfreeze();
        blueAnimation.Unfreeze();
    }
}
