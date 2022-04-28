using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutScene0Mgr : MonoBehaviour
{
    public Text[] m_Texts;
    public ScriptUIMgr m_ScriptMgr;
    public string m_NextSceneName;

    private Color m_Color;
    private Color m_FadeColor;

    private float m_Timer = 3f;

    private int Phase = 0;


    private int m_Idx = 0;

    private void Awake()
    {
        m_Color = new Color(1, 1, 1, 0);
        m_FadeColor = new Color(1, 1, 1, 1);

        foreach (Text element in m_Texts)
        {
            element.color = m_Color;
        }
    }

    private void Update()
    {
        switch (Phase)
        {
            case 0:
                InitScript();
                break;

            case 1:
                m_ScriptMgr.NextScript(4, true);
                Phase++;
                break;

            case 2:
                if(m_ScriptMgr.m_isPlaying == false)
                {
                    m_ScriptMgr.NextScript(0, false);
                    Phase++;
                }
                break;

            case 3:
                // Whoop Whoop
                if (m_ScriptMgr.m_isPlaying == false)
                {
                    Phase++;
                }
                break;

            case 4:
                m_ScriptMgr.NextScript(2, true);
                Phase++;
                break;

            case 5:
                // Sans
                m_Timer -= Time.deltaTime;
                if(m_Timer <= 0f)
                {
                    Phase++;
                    m_Timer = 3f;
                }
                break;

            case 6:
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                    Phase++;
                break;

            case 7:
                SceneManager.LoadScene(m_NextSceneName);
                break;
        }
    }

    private void InitScript()
    {
        if (m_Idx < m_Texts.Length)
        {
            m_Color.a += Time.deltaTime * 0.5f;
            m_Texts[m_Idx].color = m_Color;

            if (m_Color.a >= 1)
            {
                m_Color.a = 0;
                m_Idx++;
            }
        }
        else
        {
            foreach (Text element in m_Texts)
            {
                element.color = m_FadeColor;
            }
            m_FadeColor.a -= Time.deltaTime * 0.5f;
            if (m_FadeColor.a <= 0f)
                Phase++;
        }
    }
}


