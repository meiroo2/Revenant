using UnityEngine;


public class Objective_Tuto02 : Objective
{
 /*
     두 번쨰로 하는 목표
     1. A키 누르기
     2. D키 누르기
     */
    
    private Player_InputMgr m_InputMgr;

    // Objective Variables
    private int m_Count = 0;
    private int m_Phase = 0;
    private Player m_Player;


    public override void InitObjective(ObjectiveMgr _mgr, ObjectiveUI _ui)
    {
        m_ObjMgr = _mgr;
        m_ObjUI = _ui;
        
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
        
        m_InputMgr.SetAllLockByBool(true);
        m_InputMgr.p_MousePosLock = false;
        m_InputMgr.p_MoveInputLock = false;
        m_InputMgr.p_FireLock = false;

        m_Phase = 0;
        m_Count = 0;
        
        m_InputMgr.SetAttackAction(AddCount);
    }

    public override void UpdateObjective()
    {
        switch (m_Phase)
        {
            case 1:
                m_ObjMgr.SendObjSuccessInfo(m_ObjIdx, true);
                m_Phase = -1;
                m_InputMgr.ResetAttackAction();
                break;
        }
    }

    public override void ExitObjective()
    {
        
    }

    private void AddCount()
    {
        m_Count++;
        
        m_ObjUI.SetObjectiveProgress(0, m_Count * 0.2f);
        if (m_Count >= 5)
        {
            m_ObjUI.SetObjectiveFontStyle(0, true);
            m_Phase = 1;
        }
    }
}