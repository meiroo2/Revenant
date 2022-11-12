using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Objective_TutoBulletTime : Objective
{
    
    private Player_InputMgr m_InputMgr;

    // Objective Variables
    private int m_Count = 0;
    private int m_Phase = 0;

    private int m_UseBulletTimeCount = 0;

    private Player m_Player;

    public List<BasicEnemy> tutorialEnemies = new();
    private int enemyCount = 0;

    public TutorialHatchObject p_TutorialHatchObject;

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
        m_InputMgr.p_BulletTimeLock = false;

		m_Phase = 0;
        m_Count = 0;
		p_TutorialHatchObject.Initialize();
		m_Player.AttachActionOnFSM(PlayerStateName.BULLET_TIME, () => AddBulletTimeCountCount(), true);
		foreach (var enemy in tutorialEnemies)
		{
            enemy.gameObject.SetActive(true);
		}
		enemyCount = tutorialEnemies.Count;
	}

    public override void UpdateObjective()
    {
        m_Player.m_RageGauge.ChangeGaugeValue(150);

		switch (m_Phase)
        {
            case 0:
                if(m_UseBulletTimeCount == 1)
                {
					m_ObjMgr.SendObjSuccessInfo(0, true);
					m_ObjUI.SetObjectiveFontStyle(0, true);
					m_ObjUI.SetObjectiveProgress(0, 1);
				}

                if (m_UseBulletTimeCount >= 1)
                {
					float proceed = 0;
					proceed = Mathf.InverseLerp(enemyCount, 0, GetActiveEnemyCount());
					m_ObjUI.SetObjectiveProgress(1, proceed);

                    if(proceed >= 1)
                    {
						m_ObjUI.SetObjectiveFontStyle(1, true);
						m_Phase = 1;
                    }
				}


				break;
            case 1:
				m_ObjMgr.SendObjSuccessInfo(m_ObjIdx, true);
				m_Phase = -1;
				p_TutorialHatchObject.action?.Invoke();
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

    private void AddBulletTimeCountCount() => m_UseBulletTimeCount++;
}