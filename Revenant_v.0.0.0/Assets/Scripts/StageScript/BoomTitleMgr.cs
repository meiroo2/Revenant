using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoomTitleMgr : MonoBehaviour
{
    public float m_Timer = 5f;

    private float timer;
    public string m_NextSceneName;

    private void Awake()
    {
        timer = m_Timer;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            SceneManager.LoadScene(m_NextSceneName);
        }
    }
}
