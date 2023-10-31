using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonEvent : MonoBehaviour
{
    private bool startEvent = false;
    private int eventclock = 0;
    public smackBall playerReference;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(smackBall.powerUpState > 0)
        {
            startEvent = true;
        }
    }

    private void FixedUpdate()
    {
        if (startEvent)
        {
            if (eventclock == 0)
            {
                smackBall.inEvent = true;
                smackBall.rig.gravityScale = 0;
                smackBall.rig.velocity = Vector2.zero;
                playerReference.transform.Rotate(0, 0, 90);
                playerReference.transform.position = transform.position + (Vector3)Vector2.right;
                playerReference.golfBallColorRef.enabled = false;
            }
            if (eventclock == 20)
            {
                playerReference.golfBallColorRef.enabled = true;
                smackBall.rig.velocity = Vector2.right * 20;
                GetComponent<AudioSource>().Play();
            }
            if (playerReference.grounded && eventclock > 30)
            {
                smackBall.inEvent = false;
                smackBall.rig.gravityScale = 1;
                playerReference.transform.Rotate(0, 0, -90);

                startEvent = false;
            }
            eventclock++;
        }
        else
        {
            eventclock = 0;
        }
    }
}
