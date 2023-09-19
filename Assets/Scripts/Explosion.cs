using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents an Explosion from a Pellet Bomb
public class Explosion : MonoBehaviour
{
    //SpriteRenderer for this explosion piece
    SpriteRenderer sprite;

    //Sound effects for this explosion piece
    public AudioSource source;
    public AudioClip bonkSound;
    public AudioClip dieSound;

    //Bomb this Explosion came from
    public PelletBomb bomb;

    //Lifetime for explosion tile
    public float explosionLife = 3f;

    //Current life
    float life = 0f;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        //Once past life, destroy explosion.
        if(!bomb.frozen)
            life += Time.deltaTime;
        if (life > explosionLife)
            Destroy(gameObject);
    }

    //Set the explosion with the given Sprite at the given Position and Rotation.
    public void Place(Sprite sprite, Vector3 position, float rotation)
    {
        source.Play(); 
        this.sprite.sprite = sprite;
        this.transform.position = position;
        this.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    //When something collides with the explosion...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        source.pitch = 1;
        //If Pacman touches this explosion, it will die.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            source.PlayOneShot(dieSound, 0.7f);
            FindObjectOfType<GameManager>().PacmanEaten();
        }
        //If a Ghost touches this explosion, it will die.
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            if (collision.gameObject.GetComponent<GhostDead>() == null)
            {
                if (collision.gameObject.GetComponent<GhostBomb>() != null)
                    collision.gameObject.GetComponent<GhostBomb>().PreventExplosion();
                source.pitch = 0.93f + FindObjectOfType<GameManager>().ghostMultiplier / 15f;
                source.PlayOneShot(bonkSound, 0.7f);
                FindObjectOfType<GameManager>().GhostEaten(collision.gameObject.GetComponent<Ghost>());
            }
        }
    }
}
