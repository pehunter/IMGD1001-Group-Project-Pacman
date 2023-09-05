using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    //Properties
    public Sprite[] sprites;
    public float animationSpeed = 0.125f;
    public bool loop = true;
    bool frozen = false;

    //Components
    SpriteRenderer render;

    //Internal variables
    int currentFrame = 0;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(Advance), animationSpeed, animationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Advance animated sprite by one frame
    void Advance()
    {
        //Do not run if renderer is not enabled.
        if (frozen || !render.enabled) return;

        //Advance frame.
        currentFrame++;

        //Move back to first frame if looping is enabled and at end of frames.
        if(currentFrame >= sprites.Length && loop)
            currentFrame = 0;

        //Set sprite
        if(currentFrame < sprites.Length)
            render.sprite = sprites[currentFrame];
    }

    //Restart animation
    public void Restart()
    {
        //Set currentFrame to -1 so the advance function will set it to 0.
        currentFrame = -1;
        Advance();
    }


    public void NewDuration(float newTime) {
        CancelInvoke();
        InvokeRepeating(nameof(Advance), newTime, newTime);
    }
    public void Freeze()
    {
        frozen = true;
    }

    public void Unfreeze()
    {
        frozen = false;
    }
}
