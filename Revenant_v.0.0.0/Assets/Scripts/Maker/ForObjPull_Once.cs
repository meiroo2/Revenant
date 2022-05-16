using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForObjPull_Once : MonoBehaviour
{
    // Visible Member Variables
    public SoundMgr_SFX m_SoundSFXMgr;
    private Animator m_Animator;

    // Member Variables
    private float m_Timer = 0f;
    private bool m_isStart = false;

    // Constructors
    private void Awake()
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
    public void resetTimer(float _inputTime)
    {
        m_Timer = _inputTime;
        m_isStart = true;
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
