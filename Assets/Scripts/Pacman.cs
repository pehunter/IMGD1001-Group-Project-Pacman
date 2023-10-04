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

    //Start time
    public float startTime;

    //Pellet Bomb prefab
    public GameObject pelletBomb;
    public AudioClip bombLay;

    //Components
    public Movement movement { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public SpriteAnimator animator { get; private set; }
    public SpriteRenderer deathRenderer { get; private set; }
    public SpriteAnimator deathAnimator { get; private set; }
    
    //audio stuff
    //public AudioSource source;
    public AudioClip youDied;


    public bool frozen { get; private set; } = false;

    //Set components
    void Awake()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = normal.GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = normal.GetComponent<SpriteAnimator>();
        //source = GetComponent<AudioSource>(); 

        deathAnimator = dead.GetComponent<SpriteAnimator>();
        deathRenderer = dead.GetComponent <SpriteRenderer>();

        //start waka loop 
        //source.Play(); 
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
            {
                animator.Freeze();
                //if the animator stops the waka loop pauses.. 
                //source.Pause(); 
            }
            else
            {
                animator.Unfreeze();
                //and if the animator unstops the waka loop unpauses! 
                //source.UnPause();
            }
        }
    }

    //Place a Pellet Bomb at Pacman's current location.
    public void PlaceBomb()
    {
        if (bombs > 0 && !frozen)
        {
            //Get position that bomb will snap to.
            var roundedPosition = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, transform.position.z + 1);
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
            var rot = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

            //Insert bomb
            var bomb = Instantiate(pelletBomb, roundedPosition, Quaternion.identity);
            if (bomb.GetComponent<Movement>() != null)
            {
                bomb.transform.rotation = rot;
                bomb.GetComponent<Movement>().initialDirection = movement.direction;
            }

            //Add to GameManager's active bombs
            FindObjectOfType<GameManager>().AddBomb(bomb.GetComponent<PelletBomb>());

            bombs--;
            FindObjectOfType<GameManager>().SetBombs(bombs);

            //Play sound
            bool shouldPlay = !VolumeManager.muted;
            if (shouldPlay)
                GetComponent<AudioSource>().PlayOneShot(bombLay);
        }
    }

    //Add bombs to Pacman's bombs
    public void AddBomb(int bombs)
    {
        this.bombs += bombs;
        FindObjectOfType<GameManager>().SetBombs(this.bombs);
    }

    //Disable/enable sprite renderer
    public void SetSprite(bool state)
    {
        spriteRenderer.enabled = state;
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
        //source.Play();
    }

    public void Die()
    {
       // source.Stop();
        //Remain in frozen state for a second before death animation
        Invoke(nameof(ActuallyDie), 1f);
    }

    private void ActuallyDie()
    {
        //play death noise 
        if (!VolumeManager.muted)
            GetComponent<AudioSource>().PlayOneShot(youDied, 0.7f);

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
