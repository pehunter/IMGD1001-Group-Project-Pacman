using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents an active Pellet Rocket
public class PelletRocket : PelletBomb
{
    bool hit = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hit && collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Explode();
            hit = true;
        }
    }

    protected override void Explode()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        base.Explode();
    }
}
