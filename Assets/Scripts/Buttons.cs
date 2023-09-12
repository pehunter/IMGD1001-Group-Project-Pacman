using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    //scene for unmodified game 
    public void LaunchClassic()
    {
        SceneManager.LoadScene("OriginalGame");
    }

    //scene for modified game
    public void LaunchVariant()
    {
        SceneManager.LoadScene("ModdedGame");
    }

    //scene for credits screen
    public void LaunchCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    //scene for version notes
    public void LaunchVNotes()
    {
        SceneManager.LoadScene("VersionNotes");
    }
}

