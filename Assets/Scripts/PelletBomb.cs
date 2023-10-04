using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents an active Pellet Bomb
public class PelletBomb : MonoBehaviour
{
    //How long it takes for this pellet bomb to detonate
    public float duration;
    protected float elapsedTime;
    protected bool passedHalf;

    //How fast explosion tiles are created
    public float explosionSpeed;

    //How many tiles the explosion should be
    public int length;

    //How long an explosion tile should last
    public float explosionLife;

    protected bool exploding = false;

    //Keep track of explosion tiles
    protected List<GameObject> explosionTiles;

    protected int currentLength = 0;

    public LayerMask obstacleLayer;

    public bool frozen { get; private set; } = false;

    //Prefab for explosion
    public GameObject explosion;

    //Explosion source
    protected Vector3 source = Vector3.zero;

    //Sprites for center, middle, and end of explosion
    public Sprite center;
    public Sprite middle;
    public Sprite tip;

    protected List<Vector2> directions;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        directions = new List<Vector2>() { Vector2.zero };
        explosionTiles = new List<GameObject>();
        passedHalf = false;
    }

    private void Update()
    {
        if (!frozen)
        {
            elapsedTime += Time.deltaTime;
            if (!exploding && elapsedTime > duration)
                Explode();
            else if (!passedHalf && elapsedTime > duration / 2)
                Halftime();
        }
    }

    protected virtual void Halftime()
    {
        var animator = GetComponent<SpriteAnimator>();
        animator.NewDuration(animator.animationSpeed / 2);
        passedHalf = true;
    }

    protected virtual bool PreExplode()
    {
        return true;
    }

    //Explode the bomb
    protected virtual void Explode()
    {
        if (PreExplode())
        {
            if (source == Vector3.zero)
                source = transform.position;
            exploding = true;
            FindObjectOfType<GameManager>().GMPInvoke(length * explosionSpeed + explosionLife);
            //First entry: up/down/left/right are undefined, zero only vector inside.
            List<Vector2> newList = new List<Vector2>();
            GameObject last = null;
            foreach (Vector2 direction in directions)
            {
                if (frozen)
                    break;

                if (direction == Vector2.zero)
                    newList.AddRange(new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right });

                //Calculate position
                Vector2 shift = direction * 0.5f * currentLength;
                Vector3 pos = source + new Vector3(shift.x, shift.y, 0f);

                //If space is unoccupied, insert new explosion.
                if (!Occupied(pos, direction))
                {
                    //Calculate rotation
                    float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    //Calculate sprite
                    Sprite exploSprite = middle;
                    pos.z = -6f;
                    if (currentLength == 0)
                    {
                        exploSprite = center;
                        pos.z = -7f;
                    }
                    else if (currentLength == length)
                        exploSprite = tip;

                    var explo = Instantiate(explosion);
                    explo.GetComponent<Explosion>().explosionLife = explosionLife;
                    explo.GetComponent<Explosion>().Place(exploSprite, pos, rotation);
                    explo.GetComponent<Explosion>().bomb = this;
                    FindObjectOfType<GameManager>().AddExplosion(explo);
                    last = explo;

                    //If not at end of length, add another tile in direction.
                    if (currentLength > 0 && currentLength < length)
                        newList.Add(direction);
                }

            }
            if (!frozen)
            {
                currentLength++;
                directions = newList;
            }
            else
                newList = directions;
            if (directions.Count > 0)
            {
                if(!VolumeManager.muted && last != null)
                    last.GetComponent<AudioSource>().Play();
                Invoke(nameof(Explode), explosionSpeed);
            }
            else
                OnEnd();
        }
    }

    protected virtual void OnEnd()
    {
        exploding = false;
        source = Vector3.zero;

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
        if (cast.collider != null && cast.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle") && cast.collider.gameObject.GetComponent<BreakableWall>() != null)
        {
            cast.collider.gameObject.GetComponent<BreakableWall>().Break();
            cast = Physics2D.BoxCast(position, Vector2.one * 0.5f, 0f, Vector2.zero, 0f, obstacleLayer);
        }
        Debug.Log(cast.collider != null);
        return cast.collider != null;
    }
}
