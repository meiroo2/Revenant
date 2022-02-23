using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempEffect : MonoBehaviour
{
    float Timer = 1f;
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
            Destroy(gameObject);
    }
}
