using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressKey : MonoBehaviour
{
    [SerializeField]
    Fading m_fading;

    void Update()
    {
        if(Input.anyKeyDown)
        {
            //Time.timeScale = 0;
        }
    }
}
