using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public static float[] worldBounds; // -x, -y, x, y

    private const float followDistance = 8;
    private const float camDimensions = 32;

    private void Start()
    {
        worldBounds = new float[4];
        worldBounds[0] = -16;
        worldBounds[1] = -16;
        worldBounds[2] = 16;
        worldBounds[3] = 24;
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, toFollow.position) > followDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position + Vector3.forward * 10, toFollow.position, 16 * Time.fixedDeltaTime) + Vector3.back * 10;
        }
        if ((transform.position.x + camDimensions / 2) > worldBounds[2])
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(worldBounds[2] - camDimensions / 2, transform.position.y, -10), 16 * Time.fixedDeltaTime);
        }
        if ((transform.position.x - camDimensions / 2) < worldBounds[0])
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(worldBounds[0] + camDimensions / 2, transform.position.y, -10), 16 * Time.fixedDeltaTime);
        }
        if ((transform.position.y + camDimensions / 2) > worldBounds[3])
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, worldBounds[3] - camDimensions / 2, -10), 16 * Time.fixedDeltaTime);
        }
        if ((transform.position.y - camDimensions/2) < worldBounds[1])
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, worldBounds[1] + camDimensions / 2, -10), 16 * Time.fixedDeltaTime);
        }
    }
}
