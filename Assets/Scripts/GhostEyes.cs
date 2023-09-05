using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    //Sprites for up, down, left, and right.
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    //SpriteRenderer for the eyes
    public SpriteRenderer spriteRenderer { get; private set; }

    //Movement script
    public Movement movement { get; private set; }

    private void Awake()
    {
        //Set renderer and movement component
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();
    }

    //Set sprite to corresponding movement direction
    private void Update()
    {
        if (movement.direction == Vector2.up)
            spriteRenderer.sprite = up;
        else if (movement.direction == Vector2.down)
            spriteRenderer.sprite = down;
        else if (movement.direction == Vector2.left)
            spriteRenderer.sprite = left;
        else if (movement.direction == Vector2.right)
            spriteRenderer.sprite = right;
    }

}
