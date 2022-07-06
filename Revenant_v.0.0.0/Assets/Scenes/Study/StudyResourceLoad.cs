using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StudyResourceLoad : MonoBehaviour
{
    [Tooltip("�ҷ��� png�� ���ϸ��� �����ϼ���.")]
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
                Debug.Log("���ҽ� �ε� ����. ��ΰ� Ʋ�Ȱų�, ���ϸ��� " + p_Filename+"�� ���� �ʽ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("SpriteRenderer component cannnot be found");
        }
    }
}
