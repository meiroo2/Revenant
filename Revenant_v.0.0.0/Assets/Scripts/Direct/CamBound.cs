using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBound : MonoBehaviour
{
    // 0, 1, 2, 3 -> ╩С го аб ©Л
    public bool P_StartAppearSprite = false;

    private void Awake()
    {
        if (P_StartAppearSprite)
            GetComponent<SpriteRenderer>().enabled = true;
        else
            GetComponent<SpriteRenderer>().enabled = false;
    }
}