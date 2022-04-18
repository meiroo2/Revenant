using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptUIMgr : MonoBehaviour
{
    public string[] m_Scripts;
    public GameObject m_TxtObj;
    public GameObject m_LetterBox;

    public float m_PTxtTimer = 2.5f;

    public bool m_isPlaying = false;

    private float m_TxtTimer;
    private bool m_DoTimerCheck = false;

    private Text m_Text;
    private int m_Idx = 0;
    private int m_count = 0;

    private string m_EmptyTxt = "";

    private bool m_isItalic = false;

    private void Update()
    {
        if (m_DoTimerCheck)
        {
            m_TxtTimer -= Time.deltaTime;
            if (m_TxtTimer <= 0f)
            {
                m_TxtTimer = m_PTxtTimer;

                if (m_count > 0)
                {
                    m_count--;
                    NextScript(m_count, m_isItalic);
                }
                else
                {
                    m_DoTimerCheck = false;
                    m_isPlaying = false;
                    m_Text.text = m_EmptyTxt;
                }
            }
        }
    }

    private void Awake()
    {
        m_Text = m_TxtObj.GetComponentInChildren<Text>();
        m_TxtTimer = m_PTxtTimer;
        m_Text.text = m_EmptyTxt;
    }

    public int NextScript(int _count, bool isItalic = false)
    {
        //Debug.Log("호출됨");
        m_count = _count;
        if (m_Idx < m_Scripts.Length)
        {
            m_TxtTimer = m_PTxtTimer;
            m_Text.enabled = true;
            m_Text.text = m_Scripts[m_Idx];

            m_isItalic = isItalic;
            if (m_isItalic)
                m_Text.fontStyle = FontStyle.Italic;
            else
                m_Text.fontStyle = FontStyle.Normal;

            m_Idx++;
            m_DoTimerCheck = true;
            m_isPlaying = true;
        }
        else
            Debug.Log("스크립트 끝");
        return 0;
    }
}