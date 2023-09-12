using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Required components
[RequireComponent(typeof(Movement))]

//Controls Pacman
public class Pacman : MonoBehaviour
{
    //Public components
    public GameObject normal;
    public GameObject dead;

    //How many bombs Pacman has
    public int bombs { get; private set; } = 0;

    //Pellet Bomb prefab
    public GameObject pelletBomb;

    //Components
    public Movement movement { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public SpriteAnimator animator { get; private set; }
    public SpriteRenderer deathRenderer { get; private set; }
    public SpriteAnimator deathAnimator { get; private set; }


    public bool frozen { get; private set; } = false;

    //Set components
    void Awake()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = normal.GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = normal.GetComponent<SpriteAnimator>();

        deathAnimator = dead.GetComponent<SpriteAnimator>();
        deathRenderer = dead.GetComponent <SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Capture key input and update direction based on currently pressed keys
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            movement.nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            movement.nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            movement.nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            movement.nextDirection = Vector2.right;
        else if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");
        if (Input.GetKeyDown(KeyCode.Space))
            PlaceBomb();

        //Rotate Pacman from current direction
        if (!frozen)
        {
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

            //Check if wall hit
            if (movement.Occupied(movement.direction, true))
                animator.Freeze();
            else
                animator.Unfreeze();
        }
    }

    //Place a Pellet Bomb at Pacman's current location.
    public void PlaceBomb()
    {
        if (bombs > 0 && !frozen)
        {
            //Get position that bomb will snap to.
            var roundedPosition = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, transform.position.z + 1);

            //Insert bomb
            var bomb = Instantiate(pelletBomb);
            bomb.transform.position = roundedPosition;

            //Add to GameManager's active bombs
            FindObjectOfType<GameManager>().AddBomb(bomb.GetComponent<PelletBomb>());

            bombs--;
        }
    }

    //Add bombs to Pacman's bombs
    public void AddBomb(int bombs)
    {
        this.bombs += bombs;
    }

    public void ResetState()
    {
        deathRenderer.enabled = false;
        deathAnimator.Restart();
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void Die()
    {
        //Remain in frozen state for a second before death animation
        Invoke(nameof(ActuallyDie), 1f);
    }

    private void ActuallyDie()
    {
        //Rotate before dying
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(movement.direction.y, movement.direction.x) * Mathf.Rad2Deg - 90f, Vector3.forward);

        //Disable default sprite
        spriteRenderer.enabled = false;

        //Enable death renderer
        deathRenderer.enabled = true;
    }
    

    public void Freeze()
    {
        frozen = true;
        movement.Freeze();
        animator.Freeze();
    }

    public void Unfreeze()
    {
        frozen = false;
        movement.Unfreeze();
        animator.Unfreeze();
    }
}
