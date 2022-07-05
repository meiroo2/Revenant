using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;


[ExecuteInEditMode]
public class RotationMatScript : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private readonly int FlipVal = Shader.PropertyToID("_FlipVal");


    public void SetNormalToRight(bool _isTrue)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        m_SpriteRenderer.GetPropertyBlock(mpb);
        
        if (_isTrue)
        {
            mpb.SetInt(FlipVal, 1);
        }
        else
        {
            mpb.SetInt(FlipVal, -1);
        }
        
        
        m_SpriteRenderer.SetPropertyBlock(mpb);
    }
    
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
}