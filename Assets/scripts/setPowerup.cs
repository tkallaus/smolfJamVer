using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPowerup : MonoBehaviour
{
    public int setPowerupTo;
    public AudioClip clip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        smackBall.powerUpState = setPowerupTo;
        AudioSource.PlayClipAtPoint(clip, transform.position, 0.4f);
        gameObject.SetActive(false);
    }
}
