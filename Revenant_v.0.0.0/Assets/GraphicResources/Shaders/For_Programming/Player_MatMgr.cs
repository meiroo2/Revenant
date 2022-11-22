using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class Player_MatMgr : MonoBehaviour
{
    // Visible Member Variables
    public Material p_RotateMat;
    public float p_MaxWhiteIntensity = 1f;
    public float p_FadeSpeed= 2f;


    // Member Variables
    private SpriteRenderer[] m_Renderers;
    
    private Coroutine m_Coroutine;

    private MaterialPropertyBlock m_Matblock;
    private bool m_IsWhite = false;
    private float m_InternalWhiteIntensity = 0f;
    
    
    // Cashing
    private readonly int FlipVal_Cashed = Shader.PropertyToID("_FlipVal");
    private readonly int WhiteIntensity_Cashed = Shader.PropertyToID("_WhiteIntensity");
    
    
    // Constructors
    private void Awake()
    {
        m_Renderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < m_Renderers.Length; i++)
        {
            if(m_Renderers[i].CompareTag("Unlit"))
                continue;
            
            m_Renderers[i].material = p_RotateMat;
        }
        
        m_Matblock = new MaterialPropertyBlock();
        SetMatblock(FlipVal_Cashed, 1);
        SetMatblock(WhiteIntensity_Cashed, 0f);
        
        InitErrCheck();
    }
    
    
    // Functions
    public void FlipAllNormalsToRight(bool _isTrue)
    {
        SetMatblock(FlipVal_Cashed, _isTrue ? 1 : -1);
    }
    public void ActivateBlink(bool _doBlink)
    {
        if(m_Coroutine != null)
            StopCoroutine(m_Coroutine);
        
        if (_doBlink)
        {
            m_IsWhite = true;
            m_InternalWhiteIntensity = p_MaxWhiteIntensity;
            SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
            
            m_Coroutine = StartCoroutine(Blinking());
        }
        else
        {
            m_IsWhite = false;
            m_InternalWhiteIntensity = 0f;
            SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
        }
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            if (m_IsWhite)
            {
                if (m_InternalWhiteIntensity <= 0f)
                {
                    m_InternalWhiteIntensity = 0f;
                    SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
                    m_IsWhite = false;
                    break;
                }
                
                m_InternalWhiteIntensity -= Time.deltaTime * p_FadeSpeed;
                SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
            }
            else
            {
                if (m_InternalWhiteIntensity >= p_MaxWhiteIntensity)
                {
                    m_InternalWhiteIntensity = p_MaxWhiteIntensity;
                    SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
                    m_IsWhite = true;
                    break;
                }
                
                m_InternalWhiteIntensity += Time.deltaTime * p_FadeSpeed;
                SetMatblock(WhiteIntensity_Cashed, m_InternalWhiteIntensity);
            }

            yield return null;
        }

        m_Coroutine = StartCoroutine(Blinking());
    }
    
    private void SetMatblock(int _nameId, int _value)
    {
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            m_Renderers[i].GetPropertyBlock(m_Matblock);
            m_Matblock.SetInt(_nameId, _value);
            m_Renderers[i].SetPropertyBlock(m_Matblock);
        }
    }
    private void SetMatblock(int _nameId, float _value)
    {
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            m_Renderers[i].GetPropertyBlock(m_Matblock);
            m_Matblock.SetFloat(_nameId, _value);
            m_Renderers[i].SetPropertyBlock(m_Matblock);
        }
    }

    private void InitErrCheck()
    {
        if (m_Renderers.Length <= 0)
            Debug.Log("Player_MatMgr Awake Assignment Err");
    }
}