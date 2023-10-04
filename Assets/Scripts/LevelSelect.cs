using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    //each difficulty is a different scene. 
    private string nextLevel;
    private string nextDiff;
    public AudioSource sound;
    public int diffSelected = 0;

    public void LaunchLevel(int level)
    {
        nextLevel = level.ToString();
        nextDiff = diffSelected.ToString(); 
        if (!VolumeManager.muted)
            sound.Play();

        Invoke("LoadLevel", 0.5f);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(nextDiff + "Level_" + nextLevel);
    }

    //public void ChangeDifficulty(int diff)
    //{
    //    FindObjectOfType<GameManager>().difficulty = diff;
    //}

    public void SelectDiff(int diff)
    {
        diffSelected = diff;
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

