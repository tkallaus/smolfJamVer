using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridApprox : MonoBehaviour
{
    public Transform sprite;
    public Grid spriteLock;
    public float tilemapAnchorOffset = 0.25f;
    public bool isCam = false;

    private void LateUpdate()
    {
        Vector3Int temp = spriteLock.WorldToCell(transform.position);
        sprite.position = spriteLock.CellToWorld(temp) + new Vector3(tilemapAnchorOffset, tilemapAnchorOffset) + (isCam ? Vector3.back*10 : Vector3.zero);
    }
}
