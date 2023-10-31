using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openArea : MonoBehaviour
{
    [Tooltip("-x, -y, x, y")]
    public float[] setWorldBounds;
    public bool selfActivated;
    public Vector3 newResetPoint;

    private void OnDisable()
    {
        cameraFollow.worldBounds[0] = setWorldBounds[0];
        cameraFollow.worldBounds[1] = setWorldBounds[1];
        cameraFollow.worldBounds[2] = setWorldBounds[2];
        cameraFollow.worldBounds[3] = setWorldBounds[3];
        smackBall.resetPoint = newResetPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (selfActivated)
        {
            if (collision.gameObject.layer == 7)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
