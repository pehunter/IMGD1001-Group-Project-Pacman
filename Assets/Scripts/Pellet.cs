using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    //How many points the pellet is worth
    public int points = 10;


    //Ran whenever the pellet is eaten
    protected virtual void Eat()
    {
        //Signal to GameManager that this pellet was eaten.
        FindObjectOfType<GameManager>().PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collider is Pacman, eat this pellet.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
