using Unity.VisualScripting;
using UnityEngine;


public class Objective_TutoHide : Objective
{
     /*
     5번쨰 튜토리얼
     1. 구르기
     */
    
    private Player_InputMgr m_InputMgr;

    // Objective Variables
    private int m_Phase = 0;
    private int m_Count = 0;
    private Player m_Player;

    public TutorialHideObject p_TutorialHideObject;

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
        m_InputMgr.p_ReloadLock = false;
        m_InputMgr.p_RollLock = false;
        m_InputMgr.p_HideLock = false;
        p_TutorialHideObject.Initialize();
        p_TutorialHideObject.action?.Invoke();

		m_Player.AttachActionOnFSM(PlayerStateName.HIDDEN,() => AddCount(), true);
        m_Phase = 0;
        m_Count = 0;
    }

    public override void UpdateObjective()
    {
        switch (m_Phase)
        {
            case 0:
                if (m_Count >= 1)
                {
                    m_ObjUI.SetObjectiveProgress(0, 1f);
                    m_ObjUI.SetObjectiveFontStyle(0, true);
                    m_ObjMgr.SendObjSuccessInfo(m_ObjIdx, true);

                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitObjective()
    {
       m_Player.RemoveActionOnFSM(PlayerStateName.HIDDEN);
    }

    private void AddCount()
    {
        m_Count++;
    }
}