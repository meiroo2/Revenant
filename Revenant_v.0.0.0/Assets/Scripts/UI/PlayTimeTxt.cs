
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeTxt : MonoBehaviour
{
    private Text m_Text;
    private GameMgr m_Gm;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    private void Start()
    {
        m_Gm = GameMgr.GetInstance();
    }

    private void Update()
    {
        m_Text.text = "Time : " + m_Gm.m_GameTimer.ToString();
        if (Input.GetKeyDown(KeyCode.F5))
            m_Gm.resetTimer();
    }
}
