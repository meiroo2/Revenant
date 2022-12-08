using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;


public class Player_DeadProcess : MonoBehaviour
{
    // Visible Member Variables
    public float p_BlackSpeed = 1f;
    public float p_InvertSpeed = 1f;

    public Material p_InvertMat;
    public SpriteRenderer m_AniRenderer;
    public Animator m_Animator;
    public SpriteRenderer m_BlackRenderer;
    
    // Member Variables
    private int m_Phase = 0;
    
    private Action m_AfterDeadAction = null;
    private Color m_Color;

    private Coroutine m_CheckAniCoroutine = null;
    private readonly int ColorInvertLerp = Shader.PropertyToID("_ColorInvertLerp");


    // Constructor
    private void Awake()
    {
        m_Phase = 0;

        m_Color = Color.black;
        m_Color.a = 0f;
        m_AniRenderer.enabled = false;
        m_Animator.enabled = false;
        m_BlackRenderer.enabled = false;
    }


    private void Update()
    {
        switch (m_Phase)
        {
            case 2:
                if (Input.anyKeyDown)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Title_Ui/Title_Start");
                    m_CheckAniCoroutine = StartCoroutine(ColorInvert());
                }
                break;
        }
    }


    // Functions
    public void StartDeadAni(Action _action)
    {
        if (m_Phase != 0)
            return;
        
        m_Phase = 1;
        m_AfterDeadAction = _action;

        m_AniRenderer.material = p_InvertMat;
        m_AniRenderer.enabled = true;
        m_Animator.enabled = true;

        m_BlackRenderer.material = p_InvertMat;
        m_BlackRenderer.enabled = true;
        m_BlackRenderer.color = m_Color;
        m_CheckAniCoroutine = StartCoroutine(DeadProcess());
    }

    private IEnumerator DeadProcess()
    {
        yield return null;
        Time.timeScale = 0f;
        
        while (true)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
            yield return null;
        }

        while (true)
        {
            m_Color.a += Time.unscaledDeltaTime * p_BlackSpeed;
            m_BlackRenderer.color = m_Color;

            if (m_Color.a >= 1f)
            {
                m_Color.a = 1f;
                m_BlackRenderer.color = m_Color;
                break;
            }
            
            yield return null;
        }

        m_Phase = 2;

        yield break;
    }

    private IEnumerator ColorInvert()
    {
        m_Phase = 3;
        
        float lerpVal = 0f;

        while (true)
        {
            lerpVal += Time.unscaledDeltaTime * p_InvertSpeed;
            Debug.Log(lerpVal);
            
            if (lerpVal < 1f)
            {
                m_AniRenderer.material.SetFloat(ColorInvertLerp, lerpVal);
                m_BlackRenderer.color = Vector4.Lerp(Color.black, Color.white, lerpVal);
            }
            else
            {
                break;
            }

            yield return null;
        }
        
        Time.timeScale = 1f;
        m_AfterDeadAction?.Invoke();
    }
}