using System.Collections;
using System.Collections.Generic;
//using System.Collections.IEnumerable;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorChanger : MonoBehaviour
{
    Tilemap tilemap;
    //private int score;
    private int goal;
    private int currentColor;

    //here's our colors! 
    private Color[] colors;
    Color blue, cyan, green, yellow, red, magenta, white;
    
    public GameManager gameManager;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        goal = 3000;

        blue = new Color(0, 0, 255, 255);
        cyan = new Color(0, 255, 255, 255);
        green = new Color(0, 255, 0, 255);
        magenta = new Color(255, 0, 255, 255);
        white = new Color(255, 255, 255, 255);

        colors = new Color[] {blue, cyan, green, magenta, white};
        
        currentColor = 0;
        //tilemap.color = colors[currentColor];
    }
    void Update()
    {
        if(gameManager.score > goal) 
        {
            goal += 3000;
            //allows the colors to cycle!
            if (currentColor == colors.Length-1)
            {
                currentColor = -1;
            }
            currentColor += 1; 
            tilemap.color = colors[currentColor];
        }

        //my way of checking for a game over
        else if(gameManager.score == 0)
        {
            goal = 3000;
            currentColor = 0;
            tilemap.color = colors[currentColor];
        }
    }
}
