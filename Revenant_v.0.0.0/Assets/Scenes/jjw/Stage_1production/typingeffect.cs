using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typingeffect : MonoBehaviour
{
    // Visible Member Variables
    public float p_InitDelay = 1f;
    public float p_TypingDelay = 0.4f;

    [Space(20f)]
    [Header("텍스트 오브젝트 대입")]
    public Text[] p_TextObjs;


    // Member Variables
    private int m_CurIdx = 0;
    private string[] m_TextStringArr;
    private Coroutine m_CurCoroutine;


    // Constructors
    private void Awake()
    {
        m_CurIdx = 0;

        m_TextStringArr = new string[p_TextObjs.Length];
        for (int i = 0; i < p_TextObjs.Length; i++)
        {
            m_TextStringArr[i] = p_TextObjs[i].text;
            p_TextObjs[i].text = "";
        }
    }

    void Start()
    {
        m_CurCoroutine = StartCoroutine(TypingEffectCoroutine());
    }


    private IEnumerator TypingEffectCoroutine() 
    {
        yield return new WaitForSeconds(p_InitDelay);
        for(int i =0; i <= m_TextStringArr[m_CurIdx].Length; i++)
        {
            p_TextObjs[m_CurIdx].text = m_TextStringArr[m_CurIdx].Substring(0,i);

            yield return new WaitForSeconds(p_TypingDelay);
        }

        m_CurIdx++;
        if(m_CurIdx < p_TextObjs.Length)
        {
            m_CurCoroutine = StartCoroutine(TypingEffectCoroutine());
        }
        yield break;
    }
 
}
