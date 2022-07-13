using System;
using System.Collections;
using UnityEngine;


public class Player_MatReplacer : MatReplacer
{
    // Member Variables
    public float p_WhiteIntensity = 1f;
    public float p_BlinkTime = 0.5f;

    private Coroutine m_Coroutine;
    private bool m_IsOriginMat = true;

    
    // Constructors
    private new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        int whiteIntensity_Cashed = Shader.PropertyToID("_WhiteIntensity");
        MaterialPropertyBlock matBlock = new MaterialPropertyBlock();
        matBlock.SetFloat(whiteIntensity_Cashed, p_WhiteIntensity);

        for (int i = 0; i < m_Renderers.Length; i++)
        {
            m_Renderers[i].material = p_MatForReplace;
            m_Renderers[i].SetPropertyBlock(matBlock);
            m_Renderers[i].material = m_OriginalMat;
        }
    }


    // Functions
    public void ActivateBlinking(bool _isOn)
    {
        if (_isOn)
        {
            if (m_Coroutine != null)
                StopCoroutine(m_Coroutine);

            m_Coroutine = StartCoroutine(Blinking_Routine());
        }
        else
        {
            if (m_Coroutine != null)
                StopCoroutine(m_Coroutine);
            
            ChangeMat(true);
        }
    }

    private IEnumerator Blinking_Routine()
    {
        yield return new WaitForSeconds(p_BlinkTime);
        
        if (m_IsOriginMat)
        {
            ChangeMat(false);
            m_IsOriginMat = false;
        }
        else
        {
            ChangeMat(true);
            m_IsOriginMat = true;
        }

        m_Coroutine = StartCoroutine(Blinking_Routine());
    }
}