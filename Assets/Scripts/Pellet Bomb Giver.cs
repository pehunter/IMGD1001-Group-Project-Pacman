using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles on-board Bomb Pellets
public class PelletBombGiver : Pellet
{
    protected override void Eat()
    {
        FindObjectOfType<GameManager>().BombPelletEaten(this);
    }
}
