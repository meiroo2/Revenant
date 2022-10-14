using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using FixedUpdate = UnityEngine.PlayerLoop.FixedUpdate;


public class DynamicOutline : MonoBehaviour
{
    // Visible Member Variables
    public Sprite[] m_Sprites;
    [Space(20f)]
    public Transform m_CenterPos;
    public float p_DetectDistance = 0.6f;
    
    [ReadOnly] public bool m_IsActivating = true;

    
    // Member Variables
    public Animator m_Animator { get; private set; } = null;
    private SpriteRenderer m_Renderer;
    private Transform m_PlayerRealTransform;
    private float m_Distance;
    private float m_Phase;


    // Constructors
    private void Awake()
    {
        if (GetComponentInChildren<Animator>())
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Animator.enabled = false;
        }
        else
        {
            m_Animator = null;
        }
        
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_Phase = (p_DetectDistance * 2f) / m_Sprites.Length;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_PlayerRealTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
    }
    
    
    // Updates
    private void OnTriggerStay2D(Collider2D other)
    {
        m_Distance = m_CenterPos.position.x - m_PlayerRealTransform.position.x;

        if ((m_Distance < 0 ? m_Distance * -1 : m_Distance) > p_DetectDistance)
            return;

        if (m_IsActivating)
            CalculateDynamicOutline();
    }


    private void CalculateDynamicOutline()
    {
        // 양수면 플레이어가 더 왼쪽임
        // 기본은 0 = 가장 왼쪽 아웃라인
        int spriteIdx = 0;

        for (int i = 0; i < m_Sprites.Length; i++)
        {
            if (m_Distance < ( p_DetectDistance - (m_Phase * i) ) &&
                m_Distance >= ( p_DetectDistance - (m_Phase * (i + 1)) ))
            {
                spriteIdx = i;
            }
        }

        m_Renderer.sprite = m_Sprites[spriteIdx];
    }
}