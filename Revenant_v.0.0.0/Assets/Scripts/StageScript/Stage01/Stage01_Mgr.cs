using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stage01_Mgr : StageManager
{
    // Visible Member Variables
    public ToStage2_Door m_Stage2Door;
    
    
    // Member Variables
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(3);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    // Functions
    public override void SendToStageMgr(int _input)
    {
        m_Stage2Door.NowCanColliderToPlayer();
    }
}