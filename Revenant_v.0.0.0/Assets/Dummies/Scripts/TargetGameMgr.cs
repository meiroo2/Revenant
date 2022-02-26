using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetGameMgr : MonoBehaviour
{
    public float Timer = 30f;
    public bool isStart = false;

    public SoundMgr m_soundMgr;
    public TextMeshProUGUI meshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            meshProUGUI.text = "Time Left : " + Timer.ToString();
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0f)
        {
            Timer = 0f;
            meshProUGUI.text = "Time Left : " + Timer.ToString();
            isStart = false;
            m_soundMgr.endGame();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            m_soundMgr.startGame();
            isStart = true;
            Timer = 30f;
        }
    }

    public void getScore(bool isHead)
    {
        if (isHead)
            m_soundMgr.m_level += 0.1f;
        else
            m_soundMgr.m_level += 0.05f;
    }
}
