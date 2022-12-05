using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader_Signal : MonoBehaviour
{
    // Visible Member Variables
    public int m_LoadSceneIdx;

    // Member Variables
    private bool m_IsTriggered = false;

    // Constructor

    
    // Functions
    public void LoadScene()
    {
        Player_InputMgr inputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
        inputMgr.SetAllInputLock(false);
        
        GameMgr.GetInstance().p_SceneChangeMgr.InitSceneEndWithSmooth(m_LoadSceneIdx, 3f);
    }
}