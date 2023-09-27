using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public List<GameObject> walls;

    private void Awake()
    {
        walls.Add(gameObject);
    }

    public void ResetState()
    {
        foreach (var wall in walls)
            wall.SetActive(true);
    }

    //Break related gameobjects
    public void Break()
    {
        foreach (var wall in walls)
        {
            wall.SetActive(false);
            FindObjectOfType<GameManager>().AddDestroyedWall(wall.GetComponent<BreakableWall>());
        }
    }
}
