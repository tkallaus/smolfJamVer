using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetScript : MonoBehaviour
{
    public GameObject[] toReset;

    public void factoryReset()
    {
        for(int i = 0; i < toReset.Length; i++)
        {
            toReset[i].SetActive(true);
        }
    }
}
