using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles on-board Bomb Pellets
public class PelletBombGiver : Pellet
{
    //How many bombs to give Pacman
    public int bombs = 3;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().BombPelletEaten(this);
    }
}
