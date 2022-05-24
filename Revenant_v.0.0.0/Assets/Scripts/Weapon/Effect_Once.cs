using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effect_Once : MonoBehaviour
{
    float Timer = 1f;

    private void Start()
    {
        StartCoroutine(ToDestroy());
    }

    IEnumerator ToDestroy()
    {
        yield return new WaitForSeconds(Timer);
        Destroy(gameObject);
    }
}