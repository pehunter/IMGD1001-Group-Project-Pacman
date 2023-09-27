using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBomb : PelletBomb
{
    //private SpriteRenderer ticked;
    //private SpriteRenderer def;
    private Ghost ghost;

    private bool onFlash = false;
    private float initialFlash;
    public bool begin = false;
    private bool running = false;
    private void Update()
    {
        if(begin)
        {
            //When duration is half complete, flash sprite faster.
            Invoke(nameof(Halftime), duration / 2);
            elapsedTime = 0;
            begin = false;
            running = true;
        }
        //Must hard code flash speed?
        if (running && ((elapsedTime > 0.5f) || (passedHalf && elapsedTime > 0.25f)))
        {
            elapsedTime = 0f;
            onFlash = !onFlash;
            if(onFlash) {
                ghost.bodySprite.enabled = false;
                ghost.eyesSprite.enabled = false;
                ghost.whiteSprite.enabled = true;
            } else
            {
                ghost.bodySprite.enabled = true;
                ghost.eyesSprite.enabled = true;
                ghost.whiteSprite.enabled = false;
            }
        }
        if(!ghost.frozen)
            elapsedTime += Time.deltaTime;
    }
    protected override void Start()
    {
        passedHalf = false;
        ghost = GetComponent<Ghost>();
        ghost.blueSprite.enabled = false;
        directions = new List<Vector2>() { Vector2.zero };
        explosionTiles = new List<GameObject>();
    }

    protected override void Halftime()
    {
        passedHalf = true;
        Invoke(nameof(Explode), duration / 2);
    }

    protected override bool PreExplode()
    {
        var roundedPosition = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, transform.position.z + 1);
        //ghost.movement.Freeze();
        ghost.movement.body.position = roundedPosition;
        return running || exploding;
    }

    public void PreventExplosion()
    {
        running = false;
    }

    public void ResetState()
    {
        CancelInvoke(nameof(Halftime));
        CancelInvoke(nameof(Explode));
        source = Vector3.zero;
        passedHalf = false;
        currentLength = 0;
        running = false;
        directions = new List<Vector2>() { Vector2.zero };
        exploding = false;
    }

    protected override void OnEnd()
    {
        //ghost.movement.Unfreeze();
        ResetState();
    }
}
