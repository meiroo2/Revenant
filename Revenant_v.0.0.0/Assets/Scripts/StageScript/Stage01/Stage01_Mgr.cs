using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stage01_Mgr : StageManager
{
    // Visible Member Variables
    public ToStage2_Door m_Stage2Door;
    
    
    // Member Variables

    // Functions
    public override void SendToStageMgr(int _input)
    {
        m_Stage2Door.NowCanColliderToPlayer();
    }
}