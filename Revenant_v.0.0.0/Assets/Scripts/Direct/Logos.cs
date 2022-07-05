using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

public class Logos : MonoBehaviour
{
    // 로드해야 하는 신의 IDX는 1입니다.
    
    
    // Member Variables
    public Sprite[] p_Logos;
    
    
    // Visible Member Variables
    private SpriteRenderer m_Renderer;
    private readonly Color m_ClearColor = new Color(1, 1, 1, 0);
    private Color m_SpriteColor = new Color(1, 1, 1, 0);
    private int m_Idx = 0;


    // Constructors
    private void Awake()
    {
        if (p_Logos.Length == 0)
        {
            Debug.Log("출력해야 하는 이미지가 없습니다. 다음 신을 바로 로드합니다.");
            SceneManager.LoadScene(1);
        }
        
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Renderer.enabled = true;
        m_Renderer.sprite = p_Logos[m_Idx];
        m_Renderer.color = m_ClearColor;

        StartCoroutine(FadeInnOut());
    }
    
    
    // Updates
   
    
    // Functions
    private IEnumerator FadeInnOut()
    {
        // Appear
        while (true)
        {
            if (m_SpriteColor.a >= 1f)
            {
                m_SpriteColor.a = 1f;
                break;
            }
            m_Renderer.color = m_SpriteColor;
            m_SpriteColor.a += Time.deltaTime * 0.5f;
            yield return null;
        }

        // Disappear
        while (true)
        {
            if (m_SpriteColor.a <= 0f)
            {
                m_SpriteColor.a = 0f;
                break;
            }
            m_Renderer.color = m_SpriteColor;
            m_SpriteColor.a -= Time.deltaTime * 0.5f;
            yield return null;
        }

        m_Idx++;
        if (m_Idx < p_Logos.Length)
        {
            m_Renderer.sprite = p_Logos[m_Idx];
            StartCoroutine(FadeInnOut());
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}