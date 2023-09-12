using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scroller : MonoBehaviour
{

    public int floor;
    //this variable is an unnecessary middleman but eh it's not worth ripping out
    private float yFloat;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        yFloat = (float)this.transform.position.y;
        //0 is the highest y the camera is allowed to get to, so when the player gets there they can no longer scroll up. 
        if (Input.GetKey(KeyCode.W) && this.transform.position.y < 0)
        {
            // each frame the player holds W, the camera moves up 0.02. 
            this.transform.position = new Vector3(0, yFloat + 0.02f, -10);
        }
        //ditto here, floor is as low as it goes so once the player gets there they can't scroll down. 
        else if (Input.GetKey(KeyCode.S) && this.transform.position.y > floor)
        {
            // each frame the player holds D, the camera moves down 0.02. 
            this.transform.position = new Vector3(0, yFloat - 0.02f, -10);
        }
    }
}