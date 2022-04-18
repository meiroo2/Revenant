using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuRoom03_ProgressMgr : ProgressMgr
{
    // Visible Member Variables
    public WorldUIMgr m_worldUIMgr;

    public Transform m_TargetTransform;

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
            case 0:
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(true));
                break;

            case 1:
                m_ScriptUIMgr.NextScript(0, true);
                // Turret Init Animation
                break;

            case 2:
                m_ScriptUIMgr.NextScript(0, true);
                // Turret Fire Once
                break;

            case 3:
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 1));
                break;

            case 4:
                // If(PlayerState == HIDE)
                // Turret Fires toward Cover
                break;

            case 5:
                // Script("엄폐 해제해")
                break;

            case 6:
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 2));
                break;

            case 7:
                // Script("공겨그이 허점")
                break;

            case 8:
                // Turret Fires toward Player
                break;

            case 9:
                // bool Timescale else.... 
                // if length 0.2f < Timescale 0.001f else
                // Print Space UI
                break;

            case 10:
                // if hit space
                // timescale reload
                break;

            case 11:
                // turret goes off
                break;

            case 12:
                // Go to CutScene
                break;
        }
    }

    private void InitProgress()
    {

    }

    // 기타 분류하고 싶은 것이 있을 경우
}