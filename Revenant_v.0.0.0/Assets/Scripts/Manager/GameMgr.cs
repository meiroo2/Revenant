using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    // Visible Member Variables
    public float m_GameTimer { get; private set; } = 0f;


    // Member Variables
    private static GameMgr s_Instance = null;
    public static GameMgr getInstance() { return s_Instance; }
    private bool m_isStartTimer = false;


    // Constructor
    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(this != s_Instance)
            {
                Destroy(this.gameObject);
            }
        }
    }


    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            m_isStartTimer = true;

        if (m_isStartTimer)
            m_GameTimer += Time.unscaledDeltaTime;
    }

    // Functions
    public void resetTimer() 
    {
        m_isStartTimer = false;
        m_GameTimer = 0f;
    }
}