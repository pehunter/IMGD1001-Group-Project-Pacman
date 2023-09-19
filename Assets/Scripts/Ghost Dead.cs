using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GhostDead : GhostBehavior
{
    private void Start()
    {
        //Disable frightening
        frightenable = false;

        //Freeze ghosts, then unfreeze after 3 seconds.
        ghost.eyesSprite.enabled = false;
        ghost.bodySprite.enabled = false;
        ghost.blueSprite.enabled = false;
        ghost.whiteSprite.enabled = false;
        ghost.pacman.SetSprite(false);
        FindObjectOfType<GameManager>().FreezeAll();

        Invoke(nameof(EyesMode), 1f);
    }
    void EyesMode()
    {
        FindObjectOfType<GameManager>().UnfreezeAll();
        ghost.pacman.SetSprite(true);

        //Disable hitbox
        ghost.movement.collider.enabled = false;
        ghost.movement.body.isKinematic = true;
        ghost.movement.enabled = false;

        //Enter eye state
        ghost.bodySprite.enabled = false;
        ghost.eyesSprite.enabled = true;
        ghost.blueSprite.enabled = false;
        ghost.whiteSprite.enabled = false;
        ghost.movement.speedMultiplier = 1f;

        //Begin home animation
        StartCoroutine(GoHomeTransition());
    }

    private IEnumerator GoHomeTransition()
    {
        //Disable movement during exit animation
        ghost.movement.nextDirection = Vector2.up;
        ghost.movement.SetDirection(true);

        Vector3 position = transform.position;

        float elapsed = 0f;
        float animDuration = duration;

        //Explicity set position to animate ghost exiting while ignoring collisions
        while (elapsed < animDuration)
        {
            ghost.SetPosition(Vector3.Lerp(position, ghost.inside.position, elapsed / animDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Make ghost normal
        ghost.bodySprite.enabled = true;
        ghost.eyesSprite.enabled = true;
        ghost.blueSprite.enabled = false;
        ghost.whiteSprite.enabled = false;

        //elapsed = 0f;

        //while (elapsed < animDuration)
        //{
        //    ghost.SetPosition(Vector3.Lerp(ghost.inside.position, ghost.outside.position, elapsed / animDuration));
        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}

        //Re-enable movement after exit animation
        ghost.movement.nextDirection = new Vector2(UnityEngine.Random.value < 0.5f ? -1f : 1f, 0f);
        ghost.movement.SetDirection(true);
        ghost.movement.body.isKinematic = false;
        ghost.movement.enabled = true;
        ghost.movement.collider.enabled = true;

        //Switch out behavior for home behavior
        ghost.swapBehavior(typeof(GhostHome), 0);
    }
}
