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
        var rnd = new Vector2(Mathf.Round(movement.direction.x), Mathf.Round(movement.direction.y));
        if (rnd == Vector2.up)
            spriteRenderer.sprite = up;
        else if (rnd == Vector2.down)
            spriteRenderer.sprite = down;
        else if (rnd == Vector2.left)
            spriteRenderer.sprite = left;
        else if (rnd == Vector2.right)
            spriteRenderer.sprite = right;
    }

}
