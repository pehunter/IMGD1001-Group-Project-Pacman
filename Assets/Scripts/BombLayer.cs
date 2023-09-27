using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLayer : MonoBehaviour
{
    //How long it takes to lay a bomb
    public float bombTime;
    public float bombDetonationTime;
    public GameObject pelletBomb;

    private Ghost ghost;
    private float elapsedTime;
    private Vector3 bombPos;
    public bool begin = false;

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    //Place a Pellet Bomb at the ghost's current location.
    public void PlaceBomb()
    {
        //Get position that bomb will snap to.
        var roundedPosition = new Vector3(Mathf.Round(bombPos.x + 0.5f) - 0.5f, Mathf.Round(bombPos.y + 0.5f) - 0.5f, bombPos.z + 1);

        //Insert bomb
        var bomb = Instantiate(pelletBomb);
        bomb.GetComponent<PelletBomb>().duration = bombDetonationTime;
        bomb.transform.position = roundedPosition;

        //Add to GameManager's active bombs
        FindObjectOfType<GameManager>().AddBomb(bomb.GetComponent<PelletBomb>());
    }

    // Update is called once per frame
    void Update()
    {
        if(begin && !ghost.frozen)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= bombTime)
            {
                PlaceBomb();
                elapsedTime = 0;
            }
            else if (elapsedTime < bombTime - 0.1f)
                bombPos = transform.position;
        }
    }
}
