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

    //Break related gameobjects
    public void Break()
    {
        foreach(var wall in walls)
            Destroy(wall);
    }
}
