using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonusFruit : MonoBehaviour
{
    //Sprite and score data for fruit
    public List<Sprite> sprites;
    public List<int> scores;
    public float cycleTime;
    public AudioClip fruitSound;

    private float elapsedTime;
    private int currentFruit = 0;

    private List<BreakableWall> surroundingWalls;
    // Start is called before the first frame update
    void Start()
    {
        surroundingWalls = new List<BreakableWall>();
        for(int c = 0; c < transform.childCount; c++)
            surroundingWalls.Add(transform.GetChild(c).GetComponent<BreakableWall>());
    }

    //Reset the BonusFruit, which re-enables its walls.
    public void ResetState()
    {
        currentFruit = 0;
        GetComponent<SpriteRenderer>().sprite = sprites[currentFruit];
        recycle();
    }


    private void recycle()
    {
        gameObject.SetActive(true);
        foreach (BreakableWall wall in surroundingWalls)
            wall.ResetState();
    }

    //Move to next fruit
    private void cycle()
    {
        //Award points based on fruit
        FindObjectOfType<GameManager>().BonusFruitEaten(transform.position, scores[currentFruit]);

        //Gives pac-man a little speed boost for the bonus fruit. this speed boost is lost on death. 
        FindObjectOfType<Pacman>().GetComponent<Movement>().speedMultiplier += 0.12f;

        //Move to next fruit
        currentFruit = Mathf.Min(currentFruit + 1, scores.Count - 1);
        GetComponent<SpriteRenderer>().sprite = sprites[currentFruit];

        //Invoke reset
        Invoke(nameof(recycle), cycleTime);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (!VolumeManager.muted)
                collision.GetComponent<AudioSource>().PlayOneShot(fruitSound);
            cycle();
        }
    }
}
