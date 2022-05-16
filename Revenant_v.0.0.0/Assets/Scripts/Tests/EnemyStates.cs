using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyStates
{
    public DaggerEnemy m_Enemy { get; set; }

    public void StartState(DaggerEnemy _enemy);
    public void UpdateState();
    public void ExitState();
    public void NextPhase();
}

public class IDLE_Dagger : EnemyStates
{
    public DaggerEnemy m_Enemy { get; set; }

    public void StartState(DaggerEnemy _enemy)
    {
        m_Enemy = _enemy;
    }
    public void UpdateState()
    {
        if (m_Enemy.m_VisionHit.collider != null)
        {
            ExitState();
            m_Enemy.ChangeFSMState(new FOLLOW_Dagger());
        }
    }
    public void ExitState()
    {

    }
    public void NextPhase() { }
}

public class FOLLOW_Dagger : EnemyStates
{
    public DaggerEnemy m_Enemy { get; set; }
    private Transform m_EnemyTransform;
    private Transform m_PlayerTransform;

    public void StartState(DaggerEnemy _enemy)
    {
        Debug.Log("���󰩴ϴ�.");
        m_Enemy = _enemy;
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }
    public void UpdateState()
    {
        m_Enemy.m_DistanceBetweenPlayer = m_EnemyTransform.position.x - m_PlayerTransform.position.x; 
        if(Mathf.Abs(m_Enemy.m_DistanceBetweenPlayer) <= m_Enemy.p_StickDistance)
        {
            // �����Ÿ� ����
            ExitState();
            m_Enemy.ChangeFSMState(new Attack_Dagger());
        }
        else
        {
            if (m_Enemy.m_DistanceBetweenPlayer > 0f)
                m_Enemy.MoveByDirection(-1);
            else
                m_Enemy.MoveByDirection(1);
        }
    }
    public void ExitState()
    {

    }

    public void NextPhase() { }
}

public class Attack_Dagger : EnemyStates
{
    public DaggerEnemy m_Enemy { get; set; }
    private Transform m_EnemyTransform;
    private Transform m_PlayerTransform;

    private int m_Phase = 0;
    private float m_Timer = 0.5f;

    public void StartState(DaggerEnemy _enemy)
    {
        Debug.Log("�����մϴ�.");
        m_Phase = 0;
        m_Enemy = _enemy;
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }
    public void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                Debug.Log("���۾Ƹ� �ߵ�");
                m_Enemy.doSuperArmor();
                m_Phase++;
                break;

            case 1:
                // Empty
                break;

            case 2:
                ExitState();
                m_Enemy.m_DistanceBetweenPlayer = m_EnemyTransform.position.x - m_PlayerTransform.position.x;
                if (Mathf.Abs(m_Enemy.m_DistanceBetweenPlayer) <= m_Enemy.p_StickDistance)
                {
                    Debug.Log("���� �����Ÿ� �̳��Դϴ�.");
                    if (m_Enemy.m_DistanceBetweenPlayer > 0f)
                        m_Enemy.setisRightHeaded(false);
                    else
                        m_Enemy.setisRightHeaded(true);
                    m_Enemy.ChangeFSMState(new Attack_Dagger());
                }
                else 
                {
                    Debug.Log("�����Ÿ� ���");
                    m_Enemy.ChangeFSMState(new FOLLOW_Dagger());
                }
                break;
        }
    }
    public void ExitState()
    {

    }

    public void NextPhase() { m_Phase++; }
}

public class Dead_Dagger : EnemyStates
{
    public DaggerEnemy m_Enemy { get; set; }

    public void StartState(DaggerEnemy _enemy)
    {
        m_Enemy = _enemy;
        m_Enemy.gameObject.SetActive(false);
    }
    public void UpdateState()
    {

    }
    public void ExitState()
    {

    }

    public void NextPhase() { }
}