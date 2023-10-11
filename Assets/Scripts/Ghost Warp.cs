using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GhostWarp : MonoBehaviour
{
    public float timeToWarp = 5f;
    private float elapsedTime = 0f;
    public float dist = 5f;
    public float warpSpeed = 2f;
    public float warpBuff = 0.7f;
    public GameObject tl;
    public GameObject br;
    Ghost ghost;
    public AudioClip warpSound;
    AudioClip defaultSound;
    AudioSource asrc;
    private void Start()
    {
        asrc = GetComponent<AudioSource>();
        defaultSound = asrc.clip;
        ghost = GetComponent<Ghost>();

        if(DifficultyManager.hard)
        {
            timeToWarp *= warpBuff;
            warpSpeed *= warpBuff;
        }

    }

    private void Update()
    {
        if(!ghost.frozen && ghost.GetComponent<GhostBehavior>() != null)
        {
            var type = ghost.GetComponent<GhostBehavior>().GetType();
            if (type != typeof(GhostDead) && type != typeof(GhostHome))
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > timeToWarp)
                {
                    startWarp();
                    elapsedTime = -warpSpeed;
                }
            }
        }
    }
    private Vector2 warpDest()
    {
        Vector2 dest = Vector2.zero;
        while(dest == Vector2.zero)
        {
            var test = new Vector2(ghost.pacman.transform.position.x, ghost.pacman.transform.position.y) + new Vector2(UnityEngine.Random.Range(-dist, dist), UnityEngine.Random.Range(-dist, dist));
            test = new Vector2(Mathf.Round(test.x + 0.5f) - 0.5f, Mathf.Round(test.y + 0.5f) - 0.5f);
            if (test.x > tl.transform.position.x && test.x < br.transform.position.x && test.y < tl.transform.position.y && test.y > br.transform.position.y)
            {
                RaycastHit2D cast = Physics2D.BoxCast(new Vector3(test.x, test.y, -1), Vector2.one * 0.75f, 0f, Vector2.zero, 0f, ghost.movement.obstacleLayer);
                if (cast.collider == null)
                    dest = test;
            }
        }
        return dest;
    }
    void startWarp()
    {
        ghost.movement.enabled = false;
        //If ghost is a bomb layer, disable that.
        if (gameObject.GetComponent<BombLayer>() != null)
            gameObject.GetComponent<BombLayer>().begin = false;

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

        //Audio
        asrc.loop = true;
        asrc.clip = warpSound;
        if(!VolumeManager.muted)
            asrc.Play();

        //Find spot to warp to
        var dest = warpDest();

        //Begin warp animation
        StartCoroutine(Warp(dest));
    }

    private IEnumerator Warp(Vector2 dest)
    {
        var d3 = new Vector3(dest.x, dest.y, -1);

        //Disable movement during exit animation
        ghost.movement.nextDirection = Vector2.up;
        ghost.movement.SetDirection(true);

        Vector3 position = transform.position;

        float elapsed = 0f;
        float animDuration = warpSpeed;

        //Explicity set position to animate ghost exiting while ignoring collisions
        while (elapsed < animDuration)
        {
            ghost.SetPosition(Vector3.Lerp(position, d3, elapsed / animDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Make ghost normal
        ghost.bodySprite.enabled = true;
        ghost.eyesSprite.enabled = true;
        ghost.blueSprite.enabled = false;
        ghost.whiteSprite.enabled = false;

        //Re-enable movement after exit animation
        //ghost.movement.nextDirection = new Vector2(UnityEngine.Random.value < 0.5f ? -1f : 1f, 0f);
        //host.movement.SetDirection(true);
        ghost.movement.body.isKinematic = false;
        ghost.movement.enabled = true;
        ghost.movement.collider.enabled = true;

        //If ghost is a bomb layer, enable that.
        if (gameObject.GetComponent<BombLayer>() != null)
            gameObject.GetComponent<BombLayer>().begin = true;

        //Audio
        asrc.loop = false;
        asrc.clip = defaultSound;
        asrc.Stop();
    }
}
