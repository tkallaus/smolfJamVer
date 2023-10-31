using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugScript : MonoBehaviour
{
    public bool setTimescale = false;
    public float ts = 1;

    void Update()
    {
        if (setTimescale)
        {
            Time.timeScale = ts;
            setTimescale = false;
        }
    }
}
