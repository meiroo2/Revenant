using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;


public class Objective_TutoAttack : Objective
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

    private int m_UseLeftClickCount = 0;
    private int m_UseRightClickCount = 0;   

    private Player m_Player;

    public List<BasicEnemy> tutorialEnemies = new();
    private int enemyCount = 0;

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
        m_InputMgr.p_SideAttackLock = false;    
		m_InputMgr.p_ReloadLock = false;
		m_InputMgr.p_RollLock = false;
		m_InputMgr.p_HideLock = false;

		m_Phase = 0;
        m_Count = 0;

        m_InputMgr.SetAttackAction(AddLeftClickCount);
		m_Player.AttachActionOnFSM(PlayerStateName.MELEE, () => AddRightClickCount(), true);
		foreach (var enemy in tutorialEnemies)
		{
            enemy.gameObject.SetActive(true);
		}
		enemyCount = tutorialEnemies.Count;
	}

    public override void UpdateObjective()
    {
        switch (m_Phase)
        {
            case 0:
                if(m_UseLeftClickCount == 1)
                {
					m_ObjUI.SetObjectiveProgress(0, 1);
					m_ObjUI.SetObjectiveFontStyle(0, true);
				}
				if (m_UseRightClickCount == 1)
                {
					m_ObjUI.SetObjectiveProgress(1, 1);
					m_ObjUI.SetObjectiveFontStyle(1, true);
				}



				float proceed = 0;
				proceed = Mathf.InverseLerp(enemyCount, 0, GetActiveEnemyCount());

				m_ObjUI.SetObjectiveProgress(2, proceed);
                if(proceed >= 1)
                {
					m_ObjUI.SetObjectiveProgress(2, 1);
					m_ObjUI.SetObjectiveFontStyle(2, true);
                }

                if(proceed >= 1 && m_UseLeftClickCount >= 1 && m_UseRightClickCount >= 1)
                {
                    m_Phase = 1;
                }
				break;
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

    private int GetActiveEnemyCount()
    {
        int count = 0;
        foreach(var enemy in tutorialEnemies)
        {
            if(enemy.gameObject.activeSelf)
                count++;    
        }
        return count;
    }

    private void AddLeftClickCount() => m_UseLeftClickCount++;
	private void AddRightClickCount() => m_UseRightClickCount++;
}