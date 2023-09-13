using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents an active Pellet Bomb
public class PelletBomb : MonoBehaviour
{
    //How long it takes for this pellet bomb to detonate
    public float duration;

    //How fast explosion tiles are created
    public float explosionSpeed;

    //How many tiles the explosion should be
    public int length;

    //Keep track of explosion tiles
    private List<GameObject> explosionTiles;

    int currentLength = 0;

    public LayerMask obstacleLayer;

    public bool frozen { get; private set; } = false;

    //Prefab for explosion
    public GameObject explosion;

    //Sprites for center, middle, and end of explosion
    public Sprite center;
    public Sprite middle;
    public Sprite tip;

    List<Vector2> directions;
    // Start is called before the first frame update
    void Start()
    {
        directions = new List<Vector2>() { Vector2.zero };
        explosionTiles = new List<GameObject>();
        //When duration is half complete, flash sprite faster.
        Invoke(nameof(Halftime), duration / 2);
    }

    void Halftime()
    {
        var animator = GetComponent<SpriteAnimator>();
        animator.NewDuration(animator.animationSpeed / 2);
        Invoke(nameof(Explode), duration / 2);
    }

    //Explode the bomb
    void Explode()
    {
        FindObjectOfType<GameManager>().GMPInvoke(length * duration);
        //First entry: up/down/left/right are undefined, zero only vector inside.
        List<Vector2> newList = new List<Vector2>();
        foreach(Vector2 direction in directions)
        {
            if (frozen)
                break;

            if(direction == Vector2.zero)
                newList.AddRange(new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right });

            //Calculate position
            Vector2 shift = direction * 0.5f * currentLength;
            Vector3 pos = transform.position + new Vector3(shift.x, shift.y, 0f);

            //If space is unoccupied, insert new explosion.
            if (!Occupied(pos, direction))
            {
                //Calculate rotation
                float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //Calculate sprite
                Sprite exploSprite = middle;
                if (currentLength == 0)
                    exploSprite = center;
                else if (currentLength == length)
                    exploSprite = tip;
                
                var explo = Instantiate(explosion);
                explo.GetComponent<Explosion>().Place(exploSprite, pos, rotation);
                explo.GetComponent<Explosion>().bomb = this;
                FindObjectOfType<GameManager>().AddExplosion(explo);

                //If not at end of length, add another tile in direction.
                if (currentLength > 0 && currentLength < length)
                    newList.Add(direction);
            }
                    
        }
        if (!frozen)
        {
            currentLength++;
            Debug.Log(currentLength);
            directions = newList;
        }
        else
            newList = directions;
        if (directions.Count > 0)
            Invoke(nameof(Explode), explosionSpeed);
        else
            //Destroy bomb once all directions exhausted
            Destroy(gameObject);
    }

    public void Freeze()
    {
        frozen = true;
    }

    public void Unfreeze()
    {
        frozen = false;
    }

    //Check if the spot from the entity to the given direction is occupied
    public bool Occupied(Vector3 position, Vector2 direction)
    {
        //Check for obstacles from entity, slightly in desired direction. If no obstacles are present, then spot is free.
        RaycastHit2D cast = Physics2D.BoxCast(position, Vector2.one * 0.5f, 0f, Vector2.zero, 0f, obstacleLayer);
        return cast.collider != null;
    }
}
