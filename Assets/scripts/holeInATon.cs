using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeInATon : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject textStrokes;

    private bool stupidUnityBool = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            if (stupidUnityBool)
            {
                text1.SetActive(true);
                text2.SetActive(true);
                textStrokes.SetActive(true);
                textStrokes.GetComponent<TextMesh>().text += smackBall.strokes;
                stupidUnityBool = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            text1.SetActive(false);
            text2.SetActive(false);
            textStrokes.GetComponent<TextMesh>().text = "HOLE IN ";
            textStrokes.SetActive(false);
            stupidUnityBool = true;
        }
    }
}
