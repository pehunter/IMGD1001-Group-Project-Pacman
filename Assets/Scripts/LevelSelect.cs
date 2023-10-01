using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private string nextLevel;
    public AudioSource sound;
    public void LaunchLevel(int level)
    {
        nextLevel = level.ToString();
        if (!VolumeManager.muted)
            sound.Play();

        Invoke("LoadLevel", 0.5f);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevel);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

