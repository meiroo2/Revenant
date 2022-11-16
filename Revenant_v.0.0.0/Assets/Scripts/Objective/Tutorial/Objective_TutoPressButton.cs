﻿using Unity.VisualScripting;
using UnityEngine;


public class Objective_TutoPressButton : Objective
{
     /*
     6번쨰 튜토리얼
     1. 구르기
     */
    
    private Player_InputMgr m_InputMgr;

    // Objective Variables
    private int m_Phase = 0;
    private int m_Count = 0;
    private Player m_Player;

	public TutorialStairObject p_tutorialStair;
	public TutorialButtonObject p_tutorialButton;
    public TutorialDoorObject p_tutorialDoor;
	private CameraMgr m_CameraMgr;

	public override void InitObjective(ObjectiveMgr _mgr, ObjectiveUI _ui)
    {
        m_ObjMgr = _mgr;
        m_ObjUI = _ui;

		m_CameraMgr = FindObjectOfType<CameraMgr>();
		m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
        
        m_InputMgr.SetAllLockByBool(true);

		m_Phase = 0;
        m_Count = 0;
		p_tutorialButton.action?.Invoke();
        p_tutorialButton.action += AddCount;

		p_tutorialDoor.Initialize();

		p_tutorialStair.action?.Invoke();
		m_CameraMgr.MoveToTarget(p_tutorialStair.transform, 1);
	}

	public override void UpdateObjective()
    {
		float proceed = 0;
		proceed = Mathf.InverseLerp(p_tutorialStair.p_StairBottomTransform.position.y, p_tutorialStair.p_StairTopTransform.position.y, m_Player.transform.position.y); // 계단 오르기 진행도
		switch (m_Phase)
        {
			case 0: // 카메라 이동
				if (m_CameraMgr.m_IsMoveEnd == true)
				{
					m_Phase++;
					m_InputMgr.p_MousePosLock = false;
					m_InputMgr.p_MoveInputLock = false;
					m_InputMgr.p_FireLock = false;
					m_InputMgr.p_ReloadLock = false;
					m_InputMgr.p_RollLock = false;
					m_InputMgr.p_StairLock = false;
				}
				break;

            case 1: // 계단 오르기
				m_ObjUI.SetObjectiveProgress(0, proceed);
				if (proceed >= 0.99f)
				{
                    m_Phase++;
					m_ObjUI.SetObjectiveFontStyle(0, true);
				}
				break;

            case 2: // 버튼 누르기
                if (m_Count >= 1)
                {
					m_ObjUI.SetObjectiveProgress(1, 1);
					m_ObjUI.SetObjectiveFontStyle(1, true);
					m_CameraMgr.MoveToTarget(p_tutorialDoor.transform, 1);
					m_InputMgr.SetAllLockByBool(true);
					m_Phase++;
				}
				break;
			case 3: // 문쪽으로 카메라 이동
                if(!m_CameraMgr.m_IsFollowTarget)
                {
                    p_tutorialDoor.action?.Invoke();
				}

                if(m_CameraMgr.m_IsMoveEnd)
                {
                    m_Phase = -1;
					m_ObjMgr.SendObjSuccessInfo(m_ObjIdx, true);
					m_InputMgr.p_MousePosLock = false;
					m_InputMgr.p_MoveInputLock = false;
					m_InputMgr.p_FireLock = false;
					m_InputMgr.p_ReloadLock = false;
					m_InputMgr.p_RollLock = false;
					m_InputMgr.p_StairLock = false;
				}
                break;
        }
    }

    public override void ExitObjective()
    {
    }

    private void AddCount()
    {
        m_Count++;
    }
}