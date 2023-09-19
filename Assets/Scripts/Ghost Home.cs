using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    //Start coroutine if object is still active
    private float timeSinceStart = 0f;
    private bool finished = false;

    public void Start()
    {
        //Disable frightening
        frightenable = false;
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
        ghost.Bob(); 

        if (!finished && gameObject.activeInHierarchy && timeSinceStart > duration)
        {
            finished = true;
            StartCoroutine(ExitTransition());
        }
    }

    //If a wall is hit, reverse direction.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            ghost.movement.nextDirection = -ghost.movement.direction;
    }

    private IEnumerator ExitTransition()
    {
        //Disable movement during exit animation
        ghost.movement.nextDirection = Vector2.up;
        ghost.movement.SetDirection(true);
        ghost.movement.body.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float elapsed = 0f;
        float animDuration = 0.5f;

        //Explicity set position to animate ghost exiting while ignoring collisions
        while (elapsed < animDuration)
        {
            ghost.SetPosition(Vector3.Lerp(position, ghost.inside.position, elapsed / animDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < animDuration)
        {
            ghost.SetPosition(Vector3.Lerp(ghost.inside.position, ghost.outside.position, elapsed / animDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Re-enable movement after exit animation
        ghost.movement.nextDirection = new Vector2(Random.value < 0.5f ? -1f : 1f, 0f);
        ghost.movement.SetDirection(true);
        ghost.movement.body.isKinematic = false;
        ghost.movement.enabled = true;

        //If ghost contains a bomb, enable that bomb.
        if (gameObject.GetComponent<GhostBomb>() != null)
            gameObject.GetComponent<GhostBomb>().begin = true;

        //Pass behavior
        GhostBehavior.switchBehavior(ghost);
    }
}
