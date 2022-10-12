using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Hologram : MonoBehaviour
{
    // Member Variables
    public float p_AppearDistance = 0f;
    
    private Transform m_PlayerTransform;
    
    private Light2D m_HoloLight;
    private Animator m_Animator;

    private AnimatorStateInfo m_AniState;

    private int m_Phase = 0;
    private int m_Count = 0;

    private bool m_IsAppear = false;
    
    private readonly int Select = Animator.StringToHash("Select");

    // Constructors
    private void Awake()
    {
        m_Phase = 0;
        m_Animator = GetComponent<Animator>();
        m_HoloLight = GetComponentInChildren<Light2D>();
    }
    private void Start()
    {
        m_Animator.SetInteger(Select, -1);
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
    }

    // Updates
    private void Update()
    {
        if ((m_PlayerTransform.position - transform.position).magnitude < p_AppearDistance)
        {
            if (!m_IsAppear)
            {
                m_IsAppear = true;
            }
            
            switch (m_Phase)
            {
                case 0:     // Idle
                    m_Phase = Random.Range(1, 3);
                    m_Animator.SetInteger(Select, m_Phase);
                    break;
            
                case 1:     // Phase 1 Ani
                    m_AniState = m_Animator.GetCurrentAnimatorStateInfo(0);
                    if (m_AniState.IsName("Idle1"))
                        m_Phase = 0;
                    break;
            
                case 2:     // Phase 2 Ani
                    m_AniState = m_Animator.GetCurrentAnimatorStateInfo(0);
                    if (m_AniState.IsName("Idle1"))
                        m_Phase = 0;
                    break;
            }
        }
        else
        {
            if (m_IsAppear)
            {
                m_IsAppear = false;
                m_Animator.SetInteger(Select, -1);
                m_Phase = 0;
            }
        }
        
       
    }
}