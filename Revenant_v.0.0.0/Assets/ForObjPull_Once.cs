using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForObjPull_Once : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private float m_Timer = 0f;
    private bool m_isStart = false;

    // Constructors

    // Updates
    private void Update()
    {
        if (m_isStart)
        {
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
