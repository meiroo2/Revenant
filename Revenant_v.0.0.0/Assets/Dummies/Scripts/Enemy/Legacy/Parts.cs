using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public SpriteRenderer[] parts { get; set; }

    private void Awake()
    {
        parts = GetComponentsInChildren<SpriteRenderer>();
        
    }

    public void setSpriteParts(bool _input)
    {
        foreach (var s in parts)
        {
            if (s.enabled != _input)
                s.enabled = _input;
        }
    }
}
