using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funaudio : MonoBehaviour
{
    private AudioSource[] clip;
    public float musicVolume = 1;
    private bool toggleMute = false;
    private void Start()
    {
        clip = GetComponents<AudioSource>();
        clip[1].volume = 0;
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            clip[0].Play();
            clip[0].pitch = Random.Range(0.4f, 1.2f);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!toggleMute)
            {
                toggleMute = true;
            }
            else
            {
                toggleMute = false;
            }
        }
        if (clip[1].volume > 0f && toggleMute)
        {
            clip[1].volume -= musicVolume / 60;
        }
        if (clip[1].volume < musicVolume && !toggleMute)
        {
            clip[1].volume += musicVolume / 60;
        }
    }
}
