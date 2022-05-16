using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNormal : MonoBehaviour
{
    public Sprite p_RSprite;
    public Sprite p_LSprite;

    private SpriteRenderer m_Renderer;

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(transform.localScale.x > 0)
        {
            m_Renderer.sprite = p_RSprite;
        }
        else
        {
            m_Renderer.sprite = p_LSprite;
        }
    }
}
