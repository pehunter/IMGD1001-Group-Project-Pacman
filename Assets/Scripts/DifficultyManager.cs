using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Save difficulty state
public class DifficultyManager : MonoBehaviour
{

    //Keep track of whether game is in hardmode or not
    public static bool hard = false;

    public Color normalColor;
    public Color hardColor;

    public GameObject lvls;

    public void setDifficulty(bool hard)
    {
        DifficultyManager.hard = hard;

        for (int i = 0; i < lvls.transform.childCount; i++)
        {
            var block = lvls.transform.GetChild(i).GetComponent<Button>().colors;
            block.normalColor = hard ? hardColor : normalColor;
            lvls.transform.GetChild(i).GetComponent<Button>().colors = block;
        }
    }
}
