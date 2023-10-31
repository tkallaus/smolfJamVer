using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invalidateShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7 && smackBall.powerUpState != 3)
        {
            smackBall.invalidShot = true;
            //Debug.Log("touched the bad");
        }
    }
}
