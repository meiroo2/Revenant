using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    public Animator _animator;
    
    [Header("Lighting List")] 
    public Light2D[] Lights;

    private float[] LightsIntensity;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        LightsIntensity = new float[Lights.Length];
        for(int i = 0; i < Lights.Length; i++)
        {
            LightsIntensity[i] = Lights[i].intensity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
