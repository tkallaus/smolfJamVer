using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeOut : MonoBehaviour
{
    private SpriteRenderer sr;
    private float fadeOutDelay = 2f;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(fadeOutDelay < 0f)
        {
            if (sr.color.a > 0f)
            {
                sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime);
            }
            else
            {
                smackBall.inEvent = false;
                Destroy(gameObject);
            }
        }
        fadeOutDelay -= Time.deltaTime;
    }
}
