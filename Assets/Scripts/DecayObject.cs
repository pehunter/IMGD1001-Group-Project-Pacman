using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayObject : MonoBehaviour
{
    public float time = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(End), time);
    }

    void End()
    {
        Destroy(gameObject);
    }
}
