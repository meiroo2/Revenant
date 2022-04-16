using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogo : MonoBehaviour
{
    public Image m_BlackBoxImg;
    public GameObject Title;
    private float timer = 2f;
    private int TitlePhase;

    private Color m_BlackBoxColor;

    private Animator m_TitleAnimator;

    private void Awake()
    {
        m_TitleAnimator = Title.GetComponent<Animator>();
        TitlePhase = 0;
        m_BlackBoxColor = new Color(0, 0, 0, 1);
    }


    private void Update()
    {
        switch (TitlePhase)
        {
            case 0:
                m_BlackBoxImg.color = m_BlackBoxColor;
                m_BlackBoxColor.a -= Time.deltaTime * 0.3f;
                if(m_BlackBoxColor.a <= 0f)
                {
                    TitlePhase++;
                }
                break;
            case 1:
                m_TitleAnimator.SetInteger("isStart", 1);
                TitlePhase++;
                break;
        }
    }
}
