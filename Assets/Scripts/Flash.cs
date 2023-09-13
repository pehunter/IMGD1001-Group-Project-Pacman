using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //How much the sprite should flash
    public float flashPd = 1.0f;

    //Text to flash
    public TextMeshProUGUI flasher;

    //Or.. Sprite to flash
    public SpriteRenderer spriteFlasher;

    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > flashPd)
        {
            if (flasher != null)
                flasher.enabled = !flasher.enabled;
            else
                spriteFlasher.enabled = !spriteFlasher.enabled;
            timeElapsed = 0f;
        }
    }
}
