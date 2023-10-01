using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public AudioSource sound;
    //Play sound
    public void playSound()
    {
        if (!VolumeManager.muted)
            sound.Play();
    }
    //scene for unmodified game 
    public void LaunchClassic()
    {
        SceneManager.LoadScene("OriginalGame");
    }

    //scene for modified game
    public void LaunchVariant()
    {
        SceneManager.LoadScene("LevelSelect");
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

    //launched classic after 0.5 seconds, allowing the audio to play
    public void DelayedClassic()
    {
        Invoke("LaunchClassic", 0.5f);
        playSound();
    }

    //ditto for variant
    public void DelayedVariant()
    {
        Invoke("LaunchVariant", 0.5f);
        playSound();
    }

    //ditto for credits 
    public void DelayedCredits()
    {
        Invoke("LaunchCredits", 0.5f);
        playSound();
    }

    public void DelayedVNotes()
    {
        Invoke("LaunchVNotes", 0.5f);
        playSound();
    }
}

