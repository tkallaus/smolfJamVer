using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireRemoval : MonoBehaviour
{
    private UnityEngine.Tilemaps.Tilemap tileColorRef;
    private float colorChangeRate = 0;
    private bool burning = false;
    private void Start()
    {
        tileColorRef = GetComponent<UnityEngine.Tilemaps.Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(smackBall.powerUpState == 1)
        {
            burning = true;
        }
    }
    private void Update()
    {
        if (burning)
        {
            tileColorRef.color = Color.Lerp(tileColorRef.color, Color.black, colorChangeRate);
            colorChangeRate += Time.deltaTime;
            if (colorChangeRate >= 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        burning = false;
        tileColorRef.color = new Color(78 / 255f, 43 / 255f, 20 / 255f);
        colorChangeRate = 0;
    }
}
