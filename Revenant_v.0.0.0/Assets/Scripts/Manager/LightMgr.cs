using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightMgr : MonoBehaviour
{
    public float[] p_LayerLightIntensity = new float[8];
    public float[] p_LayerParallaxIntensity = new float[8];

    [Space (30f)]
    [Header("Don't ∂£¡„")]
    public TempParaBack[] p_BackGrounds;
    public Light2D[] p_LayerLights;

    private void Start()
    {
        setLightValues();
        setParallaxValues();
    }

    public void setLightValues() 
    {
        for(int i = 0; i < p_LayerLightIntensity.Length; i++)
        {
            p_LayerLights[i].intensity = p_LayerLightIntensity[i];
        }
    }
    public void setParallaxValues()
    {
        for (int i = 0; i < p_LayerParallaxIntensity.Length; i++)
        {
            p_BackGrounds[i].m_ParaValue = p_LayerParallaxIntensity[i];
        }
    }
}