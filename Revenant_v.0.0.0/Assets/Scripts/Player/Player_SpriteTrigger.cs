using System;
using UnityEngine;


public class Player_SpriteTrigger : MonoBehaviour
{
    // Visible Member Variables
    public SpriteController p_Controller;
    public BoxCollider2D p_BoxCollider;
    public bool p_Arr00IsOn = false;
    public bool p_Arr01IsOn = false;

    // Member Variables
    private Transform m_PlayerTransform;
    private float m_LeftPosX;
    private float m_RightPosX;

    private float m_FullDistance;
    private float m_Distance;
    private float m_NormalizedDistance;
    

    // Constructors
    private void Awake()
    {
        m_LeftPosX = transform.position.x - (p_BoxCollider.size.x / 2f);
        m_RightPosX = transform.position.x + (p_BoxCollider.size.x / 2f);
        m_FullDistance = m_RightPosX - m_LeftPosX;
    }

    private void Start()
    {
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
    }
    
    
    // Updates


    // Physics
    private void OnTriggerStay2D(Collider2D other)
    {
        m_Distance = m_RightPosX - m_PlayerTransform.position.x;
        m_NormalizedDistance = m_Distance / m_FullDistance;

        p_Controller.SetSpriteAlpha(0, p_Arr00IsOn ? (1f - m_NormalizedDistance) : m_NormalizedDistance);
        p_Controller.SetSpriteAlpha(1, p_Arr01IsOn ? (1f - m_NormalizedDistance) : m_NormalizedDistance);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_PlayerTransform.position.x < transform.position.x)
        {
            p_Controller.SetSpriteAlpha(0, p_Arr00IsOn ? 0f : 1f);
            p_Controller.SetSpriteAlpha(1, p_Arr01IsOn ? 0f : 1f);
        }
        else
        {
            p_Controller.SetSpriteAlpha(0, p_Arr00IsOn ? 1f : 0f);
            p_Controller.SetSpriteAlpha(1, p_Arr01IsOn ? 1f : 0f);
        }
    }

    // Functions
    
    
}