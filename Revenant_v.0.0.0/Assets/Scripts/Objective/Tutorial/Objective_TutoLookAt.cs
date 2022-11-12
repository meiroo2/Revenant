using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;



public class Objective_TutoLookAt : Objective
{
    /*
     첫 번쨰로 하는 목표
     1. 왼쪽 보기
     2. 오른쪽 보기
     */
    
    private Player_InputMgr m_InputMgr;

    // Objective Variables
    private float m_Timer = 0f;
    private int m_Phase = 0;
    private Player m_Player;
    private bool m_Success = false;
    
    public override void InitObjective(ObjectiveMgr _mgr, ObjectiveUI _ui)
    {
        m_ObjMgr = _mgr;
        m_ObjUI = _ui;
        
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
        
        m_InputMgr.SetAllLockByBool(true);
        m_Timer = 0f;
        m_Phase = 0;
    }

    public override void UpdateObjective()
    {
        switch (m_Phase)
        {
            case 0:
                m_InputMgr.p_MousePosLock = false;
                m_Phase++;
                break;
            
            case 1:
                if (!m_Player.m_IsRightHeaded)
                {
                    m_Timer += Time.deltaTime * 0.5f;
                    m_ObjUI.SetObjectiveProgress(0, m_Timer);

                    if (m_Timer >= 1f)
                    {
                        m_Timer = 0f;
                        m_ObjUI.SetObjectiveFontStyle(0, true);
                        m_Phase++;
                    }
                }
                break;
            
            case 2:
                if (m_Player.m_IsRightHeaded)
                {
                    m_Timer += Time.deltaTime * 0.5f;
                    m_ObjUI.SetObjectiveProgress(1, m_Timer);
                    
                    if (m_Timer >= 1f)
                    {
                        m_Timer = 0f;
                        m_ObjUI.SetObjectiveFontStyle(1, true);
                        m_Phase++;
                    }
                }
                break;
            
            case 3:
                //m_ObjMgr.SelfExit();
                m_ObjMgr.SendObjSuccessInfo(m_ObjIdx, true);
                m_Phase = -1;
                break;
        }
    }

    public override void ExitObjective()
    {
        
    }
}