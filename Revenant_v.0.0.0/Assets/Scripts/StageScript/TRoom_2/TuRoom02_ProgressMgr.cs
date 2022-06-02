using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuRoom02_ProgressMgr : ProgressMgr
{
    /*
    
    // Visible Member Variables
    public WorldUIMgr m_worldUIMgr;

    public Transform m_TargetTransform;
    public TutoEnemyMgr m_TutoEnemyMgr;

    // Member Variables
    public ScriptUIMgr m_ScriptUIMgr;

    // Constructors
    private void Start()
    {
        NextProgress();
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            NextProgress();
    }

    // Physics


    // Functions
    public override void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
            case 0: // S + D WorldIUI Print
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(4).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(2).ActivateIUI(new IUIParam(true));

                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(true));
                break;

            case 1: // S + D WorldIUI Erase + Collider off
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOpen", 1));
                // Auto Collider Off
                break;

            case 2:
                // GunBox Ani Start
                m_worldUIMgr.getWorldUI(2).AniSetIUI(new IUIParam("isOpen", 1));
                break;

            case 3: // F WorldIUI Print
                m_worldUIMgr.getWorldUI(3).ActivateIUI(new IUIParam(true));
                break;

            case 4: // F WorldIUI Erase
                m_worldUIMgr.getWorldUI(2).AniSetIUI(new IUIParam("isOpen", 2));
                m_worldUIMgr.getWorldUI(3).ActivateIUI(new IUIParam(false));
                m_ScriptUIMgr.NextScript(0, true);

                m_TutoEnemyMgr.TargetBoardToggle(true);
                // Player Gets Gun
                // Ingame UI Print
                // Gen Target
                // Print Script('Eliminate Target')

                // V - Mouse Icon Print - 
                // m_worldUIMgr.getWorldUI(2).ActivateIUI(new IUIParam(true));

                // V - Mouse Position set
                //m_worldUIMgr.getWorldUI(3).PosSetIUI(new IUIParam(m_TargetTransform));
                break;

            case 5:
                // Speaker Ani
                // Print Script('Eliminate Drone')
                m_ScriptUIMgr.NextScript(1, true);
                m_TutoEnemyMgr.DroneToggle(true);
                break;

            case 6:
                // Spawn Drone
                m_TutoEnemyMgr.RespawnTarget();
                m_worldUIMgr.getWorldUI(4).AniSetIUI(new IUIParam("isOpen", 1));
                m_worldUIMgr.getWorldUI(5).ActivateIUI(new IUIParam(true));
                break;

            case 7:
                // Open Right Door
                //m_worldUIMgr.getWorldUI(4).AniSetIUI(new IUIParam("isOpen", 1));
                break;
        }
    }

    private void InitProgress()
    {

    }

    // 기타 분류하고 싶은 것이 있을 경우
    
    */
}
