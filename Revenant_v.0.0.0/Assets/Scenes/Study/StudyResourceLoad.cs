using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StudyResourceLoad : MonoBehaviour
{
    [Tooltip("불러올 png의 파일명을 지정하세요.")]
    public string p_Filename;
    
    private SpriteRenderer m_Renderer;
    private Sprite m_LoadedSprite;
    
    private void Awake()
    {
        if (GetComponent<SpriteRenderer>())
        {
            m_Renderer = GetComponent<SpriteRenderer>();
            if (Resources.Load<Sprite>(p_Filename))
            {
                m_LoadedSprite = Resources.Load<Sprite>(p_Filename);
                m_Renderer.sprite = m_LoadedSprite;
            }
            else
            {
                Debug.Log("리소스 로드 실패. 경로가 틀렸거나, 파일명이 " + p_Filename+"와 같지 않습니다.");
            }
        }
        else
        {
            Debug.Log("SpriteRenderer component cannnot be found");
        }
    }
}
