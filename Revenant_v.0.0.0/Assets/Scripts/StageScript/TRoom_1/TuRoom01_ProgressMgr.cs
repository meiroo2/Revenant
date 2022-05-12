using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuRoom01_ProgressMgr : ProgressMgr
{
    private bool m_isPushA = false;
    private bool m_isPushD = false;
    private float m_KeyPushTimer = 1f;
    // Visible Member Variables
    public WorldUIMgr m_worldUIMgr;

    public Transform m_TargetTransform;

    // Member Variables
    public ScriptUIMgr m_ScriptUIMgr;

    private void Start()
    {
        NextProgress();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            m_isPushA = true;
        else if (Input.GetKeyUp(KeyCode.A))
            m_isPushA = false;

        if (Input.GetKeyDown(KeyCode.D))
            m_isPushD = true;
        else if (Input.GetKeyUp(KeyCode.D))
            m_isPushD = false;

        if (Input.GetKeyDown(KeyCode.M))
            NextProgress();
    }
    private void FixedUpdate()
    {
        switch (m_ProgressValue)
        {
            case 0:
                if (m_isPushA)
                {
                    m_KeyPushTimer -= Time.deltaTime;
                    if (m_KeyPushTimer <= 0f)
                    {
                        m_KeyPushTimer = 1f;
                        NextProgress();
                    }
                }
                else
                    m_KeyPushTimer = 1f;
                break;

            case 1:
                if (m_isPushD)
                {
                    m_KeyPushTimer -= Time.deltaTime;
                    if (m_KeyPushTimer <= 0f)
                    {
                        m_KeyPushTimer = 1f;
                        NextProgress();
                    }
                }
                else
                    m_KeyPushTimer = 1f;
                break;
        }
    }
    public override void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
            case 0:
                m_worldUIMgr.getWorldUI(3).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(5).ActivateIUI(new IUIParam(true));


                m_ScriptUIMgr.NextScript(2, true);
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(0).PosSetIUI(new IUIParam(InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform));
                break;

            case 1:
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(1).PosSetIUI(new IUIParam(InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform));
                break;

            case 2:
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(false));
                break;

            case 3:
                // Active F UI over CenterDoor
                m_worldUIMgr.getWorldUI(2).ActivateIUI(new IUIParam(true));
                break;

            case 4:
                // Deactive F UI over CenterDoor
                m_worldUIMgr.getWorldUI(2).ActivateIUI(new IUIParam(false));
                break;

            case 5:
                // Active F UI over Button, Animation Play(Button Appear)
                m_worldUIMgr.getWorldUI(4).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(5).AniSetIUI(new IUIParam("isAppear", 1));
                break;

            case 6:
                // Animation Play(Stair Appear)
                m_ScriptUIMgr.NextScript(2, true);
                m_worldUIMgr.getWorldUI(4).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(6).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(7).ActivateIUI(new IUIParam(true));

                m_worldUIMgr.getWorldUI(5).AniSetIUI(new IUIParam("isAppear", 2));
                break;
        }
    }
}