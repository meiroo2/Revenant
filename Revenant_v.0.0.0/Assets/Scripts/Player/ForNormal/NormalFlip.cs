using System;
using UnityEngine;




public class NormalFlip : MonoBehaviour
{
    // Visible Member Variables
    public Sprite m_RSprite;
    public Sprite m_LSprite;
    
    // Member Variables
    private Player m_Player;
    private SpriteRenderer m_Renderer;
    private bool m_isRight = true;
    
    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }
    
    // Updates
    private void Update()
    {
        if (m_Player.m_IsRightHeaded == true && !m_isRight)
        {
            m_isRight = true;
            m_Renderer.sprite = m_RSprite;
        }
        else if(m_Player.m_IsRightHeaded != true && m_isRight)
        {
            m_isRight = false;
            m_Renderer.sprite = m_LSprite;
        }
    }

    // Functions
    //private void 
}