using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenLerp : MonoBehaviour
{
    // Visible Member Variables
    public RectTransform m_LeftToRight;
    public RectTransform m_RightToLeft;

    // Member Variables
    private int m_isFirstLerpFinished = -1;
    private float m_Timer = 1f;


    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        if(m_isFirstLerpFinished == -1)
        {
            m_Timer -= Time.deltaTime;
            if(m_Timer <= 0f)
            {
                m_Timer = 1f;
                m_isFirstLerpFinished = 0;
            }
        }
        else if (m_isFirstLerpFinished == 0)
        {
            m_LeftToRight.anchoredPosition = Vector2.Lerp(m_LeftToRight.anchoredPosition, Vector2.zero, Time.deltaTime * 3);
            m_RightToLeft.anchoredPosition = Vector2.Lerp(m_RightToLeft.anchoredPosition, Vector2.zero, Time.deltaTime * 3);

            if(Vector2.Distance(m_LeftToRight.anchoredPosition, Vector2.zero) < 5f)
            {
                m_LeftToRight.anchoredPosition = Vector2.zero;
                m_RightToLeft.anchoredPosition = Vector2.zero;
                m_isFirstLerpFinished = 1;
            }
        }
        else if(m_isFirstLerpFinished == 1)
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0f)
            {
                m_Timer = 1f;
                m_isFirstLerpFinished = 2;
            }
        }
        else if(m_isFirstLerpFinished == 2)
        {
            m_LeftToRight.anchoredPosition = Vector2.Lerp(m_LeftToRight.anchoredPosition, new Vector2(1920, 0), Time.deltaTime * 3);
            m_RightToLeft.anchoredPosition = Vector2.Lerp(m_RightToLeft.anchoredPosition, new Vector2(-1920, 0), Time.deltaTime * 3);

            if (Vector2.Distance(m_LeftToRight.anchoredPosition, new Vector2(1920, 0)) < 5f)
            {
                m_LeftToRight.anchoredPosition = new Vector2(1920, 0);
                m_RightToLeft.anchoredPosition = new Vector2(-1920, 0);
                m_isFirstLerpFinished = 3;
            }
        }
        
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
