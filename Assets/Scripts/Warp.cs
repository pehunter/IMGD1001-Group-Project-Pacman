using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Required components
[RequireComponent(typeof(BoxCollider2D))]

//Warp Pacman to the receiving end of the warp
public class Warp : MonoBehaviour
{
    //Attributes
    public GameObject receiver;

    //Components
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //New pos will be the receiver's x and y with the collider's z
        Vector3 newPos = receiver.transform.position;
        newPos.z = collision.transform.position.z;

        //Set collider to receiver pos
        collision.transform.position = newPos;
    }
}
