using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForObjPull_Once : MonoBehaviour
{
    // Visible Member Variables
    [HideInInspector] public SoundPlayer m_SoundPlayer;
    protected Animator m_Animator;

    
    // Member Variables
    protected float m_Timer = 0f;
    protected bool m_isStart = false;

    
    // Constructors
    protected void Awake()
    {
        if (GetComponentInChildren<Animator>())
        {
            m_Animator = GetComponentInChildren<Animator>();
        }
    }

    
    // Updates
    private void Update()
    {
        if (m_isStart)
        {
            /*
            if(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
            {
                gameObject.SetActive(false);
                m_isStart = false;
            }
            */
            
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0f)
            {
                m_Timer = 0f;
                gameObject.SetActive(false);
                m_isStart = false;
            }
            
        }
    }

    // Physics


    // Functions
    public virtual void resetTimer(float _inputTime)
    {
        m_Timer = _inputTime;
        m_isStart = true;
    }

    public virtual void InitPulledObj()
    {
    }


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
