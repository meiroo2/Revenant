using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FadeOutTrigger : MonoBehaviour
{
    // Visible Member Variables
    public string p_LoadSceneName;
    
    
    // Member Variables
    private bool m_Triggered = false;
    private InGame_UI m_IngameUI;
    
    
    // Constructors
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_IngameUI = instance.m_MainCanvas.GetComponentInChildren<InGame_UI>();
    }


    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_Triggered)
            return;

        //m_IngameUI.SetCallback(SceneLoad);
        //_IngameUI.DoBlackFadeIn();
    }


    private void SceneLoad()
    {
        SceneManager.LoadScene(p_LoadSceneName);
    }
}