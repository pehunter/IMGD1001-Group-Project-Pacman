using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private string nextLevel;
    public void LaunchLevel(int level)
    {
        nextLevel = level.ToString();
        Invoke("LoadLevel", 0.5f);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevel);
    }
}

