using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{

    //Keep track of whether game is muted or unmuted
    public static bool muted = false;

    public Sprite volSprite;
    public Sprite muteSprite;
    public Color volColor;
    public Color muteColor;

    public GameObject button;
    public GameObject icon;

    private void Start()
    {
        setSelf();
    }
    void setSelf()
    {
        var colors = button.GetComponent<Button>().colors;

        if (VolumeManager.muted)
        {
            colors.normalColor = muteColor;
            colors.selectedColor = muteColor;
            icon.GetComponent<Image>().sprite = muteSprite;
        }
        else
        {
            colors.normalColor = volColor;
            colors.selectedColor = volColor;
            icon.GetComponent<Image>().sprite = volSprite;
        }

        button.GetComponent<Button>().colors = colors;
    }
    public void toggleMute()
    {
        //Toggle
        VolumeManager.muted = !VolumeManager.muted;
        setSelf();
    }
}
