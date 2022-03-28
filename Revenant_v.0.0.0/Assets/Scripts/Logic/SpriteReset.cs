using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReset : MonoBehaviour
{
    private Sprite m_FirstSprite;
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_FirstSprite = m_SpriteRenderer.sprite;

        if (GetComponent<Animator>())
        {
            m_Animator = GetComponent<Animator>();
        }
    }
    private void OnEnable()
    {
        if (m_Animator)
        {
            m_Animator.enabled = false;
            m_SpriteRenderer.sprite = m_FirstSprite;
            m_Animator.enabled = true;
            m_SpriteRenderer.sprite = m_FirstSprite;
        }

    }
}
