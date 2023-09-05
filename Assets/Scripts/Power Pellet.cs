using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles power pellets
public class PowerPellet : Pellet
{
    //Length of ghost frightened state for power pellet
    public float duration = 8f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
