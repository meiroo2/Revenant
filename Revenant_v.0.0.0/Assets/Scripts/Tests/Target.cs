using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Target : MonoBehaviour
{
    public Transform p_Head;
    public Transform p_Body;
    
    private Transform m_AimCursorTransform;

    [Space(20f)] public TextMeshProUGUI[] m_Txts;
    
    private int m_TxtIdx = 0;
    private Coroutine[] m_TxtCoroutineArr;
    private Color whilteColor = Color.clear;

    private Coroutine m_ResetTxtIdxCoroutine;

    private float m_CursorToHeadDistance;
    private float m_CursorToBodyDistance;

    private bool m_IsColliding = false;

    private void Awake()
    {
        m_TxtCoroutineArr = new Coroutine[m_Txts.Length];

        foreach (var VARIABLE in m_Txts)
        {
            VARIABLE.color = whilteColor;
        }
    }

    private void Start()
    {
        m_AimCursorTransform = InstanceMgr.GetInstance().GetComponentInChildren<AimCursor>().transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        m_IsColliding = true;
    }


    public void PrintTxt(string _input)
    {
        m_Txts[m_TxtIdx].text = _input;
        
        if(!ReferenceEquals(m_TxtCoroutineArr[m_TxtIdx], null))
            StopCoroutine(m_TxtCoroutineArr[m_TxtIdx]);
        
        m_TxtCoroutineArr[m_TxtIdx] = StartCoroutine(FadeOutTxt(m_Txts[m_TxtIdx]));

        m_TxtIdx++;
        if (m_TxtIdx >= m_Txts.Length)
            m_TxtIdx = 0;
        
        if(!ReferenceEquals(m_ResetTxtIdxCoroutine, null))
            StopCoroutine(m_ResetTxtIdxCoroutine);
        m_ResetTxtIdxCoroutine = StartCoroutine(ResetTxtIdx());
    }

    private IEnumerator ResetTxtIdx()
    {
        float Timer = 0.5f;
        while (true)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
                break;
            yield return null;
        }

        m_TxtIdx = 0;
    }

    private IEnumerator FadeOutTxt(TextMeshProUGUI _txt)
    {
        
        Color txtColor = Color.white;
        _txt.color = txtColor;
        while (true)
        {
            _txt.color = txtColor;
            txtColor.a -= Time.deltaTime;

            if (txtColor.a <= 0f)
                break;
            yield return null;
        }
    }
}