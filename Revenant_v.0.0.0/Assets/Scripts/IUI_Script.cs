using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IUI_Script : MonoBehaviour, IUI
{
    public string[] m_Scripts;
    public bool m_isActive { get; set; } = false;

    public float m_PTxtTimer = 2.5f;

    private float m_TxtTimer;
    private bool m_DoTimerCheck = false;

    private Text m_Text;
    private int m_Idx = 0;

    public int[] m_Phases;
    private bool m_DoPhasing = false;
    private int m_curPhase = -1;

    private string m_EmptyTxt = "";

    private void Update()
    {
        if (m_DoTimerCheck)
        {
            m_TxtTimer -= Time.deltaTime;
            if(m_TxtTimer <= 0f)
            {
                m_TxtTimer = m_PTxtTimer;
                m_DoTimerCheck = false;
                m_Text.text = m_EmptyTxt;

                if (m_DoPhasing)
                {
                    if (m_Idx < m_Phases[m_curPhase])
                    {
                        ActivateIUI(new IUIParam(true));
                    }
                    else
                    {
                        m_DoPhasing = false;
                        if (m_curPhase < m_Phases.Length - 1)
                            m_curPhase++;
                        else
                            m_curPhase = -1;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_TxtTimer = m_PTxtTimer;
        if (m_Phases.Length > 0)
            m_curPhase = 0;
    }

    public int ActivateIUI(IUIParam _input)
    {
        if (_input.m_ToActive == true)
        {
            if (m_Idx < m_Scripts.Length)
            {
                if (m_curPhase >= 0)
                {
                    m_DoPhasing = true;
                }

                m_TxtTimer = m_PTxtTimer;
                m_Text.enabled = true;
                m_Text.text = m_Scripts[m_Idx];
                m_Idx++;
                m_DoTimerCheck = true;
            }
            else
                Debug.Log("스크립트 끝");
        }
        return 0;
    }
}
