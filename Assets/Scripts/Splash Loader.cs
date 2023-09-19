using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{
    //State of splashes
    private enum SPLASH_STATE
    {
        FADE_IN,
        FADE_OUT,
        DISPLAY
    }
    //Screens to load
    public List<GameObject> splashScreens;

    //How long each splash should display for
    public float displayTime;

    //How long it takes for each splash to fade in/out
    public float fadeTime;

    private int currentSplash = 0;
    private float elapsedTime;
    private SPLASH_STATE state = SPLASH_STATE.FADE_IN;
    void Start()
    {
        foreach(var splash in splashScreens)
            splash.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSplash >= splashScreens.Count)
        {
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject);
        }
        else
        {

            elapsedTime += Time.deltaTime;
            var screen = splashScreens[currentSplash].GetComponent<SpriteRenderer>();
            var c = screen.color;

            //If screen is currently fading in
            if (state == SPLASH_STATE.FADE_IN)
            {
                //Transition states
                if (elapsedTime > fadeTime)
                {
                    elapsedTime = 0;
                    state = SPLASH_STATE.DISPLAY;
                    screen.color = new Color(c.r, c.g, c.b, 1f);
                }
                else
                    screen.color = new Color(c.r, c.g, c.b, elapsedTime / fadeTime);
                screen.enabled = true;
            }
            else if (state == SPLASH_STATE.DISPLAY && elapsedTime > displayTime)
            {
                elapsedTime = 0;
                state = SPLASH_STATE.FADE_OUT;
            }
            else if (state == SPLASH_STATE.FADE_OUT)
            {
                //Transition states
                if (elapsedTime > fadeTime)
                {
                    elapsedTime = 0;
                    state = SPLASH_STATE.FADE_IN;
                    screen.enabled = false;
                    currentSplash++;
                }
                else
                    screen.color = new Color(c.r, c.g, c.b, (fadeTime - elapsedTime) / fadeTime);
            }
        }
    }
}
