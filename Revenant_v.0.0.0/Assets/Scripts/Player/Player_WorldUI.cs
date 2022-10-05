using System;
using UnityEngine;


public class Player_WorldUI : MonoBehaviour
{
    public Sprite[] p_SpriteArr;
    private SpriteRenderer m_Renderer;

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Renderer.enabled = false;
    }

    public void PrintSprite(int _idx)
    {
        if (_idx == -1)
        {
            m_Renderer.enabled = false;
        }
        else
        {
            m_Renderer.enabled = true;
            m_Renderer.sprite = p_SpriteArr[_idx];
        }
    }
}