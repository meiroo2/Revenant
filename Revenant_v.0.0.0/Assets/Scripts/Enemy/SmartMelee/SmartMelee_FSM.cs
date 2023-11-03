

using System.Collections.Generic;
using UnityEngine;

public class SmartMelee_FSM : Enemy_FSM
{
    protected SmartMelee m_Enemy;
    protected Animator m_EnemyAnimator;
    
    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }
}

public class IDLE_SmartMelee : SmartMelee_FSM
{
     // Member Variables
    private int m_PatrolIdx = 0;
    private float m_InternalTimer = 0f;
    private Transform m_EnemyTransform;
    private int m_Phase = 0;
    private float m_Timer = 0;
    private bool m_IsFirst = true;
    
    private readonly int Turn = Animator.StringToHash("Turn");
    private readonly int Walk = Animator.StringToHash("Walk");

    // Constructor
    public IDLE_SmartMelee(SmartMelee _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_InternalTimer = m_Enemy.p_LookAroundDelay;

        m_PatrolIdx = 0;
        m_Phase = 0;
        m_Timer = 0f;
    }

    public override void UpdateState()
    {
        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null) && !m_Enemy.m_PlayerCognition)
        {
            m_Enemy.StartPlayerCognition();
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetInteger(Turn, 0);
    }

    public override void NextPhase()
    {
        
    }
}

public class FOLLOW_SmartMelee : SmartMelee_FSM
{
    // Member Variables
    private SectionManager m_SectionMgr;
    private SectionSensor m_SectionSensor;
    
    
    private Transform m_EnemyTransform;
    private Transform m_PlayerTransform;
    
    private int m_Phase = 0;
    private int m_InteractIdx = 0;

    private List<List<SectionDoor>> m_PathLists = new();
    private List<SectionDoor> m_TargetList = new();

    // Constructor
    public FOLLOW_SmartMelee(SmartMelee _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_Player.transform; 
        
        m_InteractIdx = 0;
        m_Phase = 0;
        m_PathLists.Clear();
        
        m_SectionMgr = m_Enemy.p_SectionManager;
        m_SectionSensor = m_Enemy.p_SectionSensor;
    }

    public override void UpdateState()
    {
        if (m_SectionMgr.IsSameSectionWithPlayer(m_SectionSensor))
        {
            // 플레이어랑 같은 위치
            Debug.DrawLine(m_EnemyTransform.position, m_PlayerTransform.position,
                GetColor(2));
        }
        else
        {
            switch (m_Phase)
            {
                case 0:
                    m_PathLists = m_Enemy.p_SectionManager.GetPathtoPlayer(
                        m_Enemy.p_SectionSensor,
                        GameMgr.GetInstance().p_PlayerMgr.GetPlayer().m_PlayerSectionSensor);
                    
                    m_Phase = 1;
                    break;
                
                case 1:
                    switch (m_Enemy.p_TraceType)
                    {
                        case TraceType.Fastest:
                            m_TargetList = m_Enemy.p_SectionManager.GetTargetPath(m_PathLists, TraceType.Fastest);
                            m_InteractIdx = 0;
                            m_Phase = 2;
                            break;
                        
                        case TraceType.BackDoor:
                            m_TargetList = m_Enemy.p_SectionManager.GetTargetPath(m_PathLists, TraceType.BackDoor);
                            m_InteractIdx = 0;
                            m_Phase = 2;
                            break;
                    }
                    break;
                
                case 2:
                    if (m_InteractIdx == m_TargetList.Count)
                    {
                        m_Enemy.ResetMovePoint(m_PlayerTransform.position);
                        m_Enemy.SetRigidToPoint();
                    }
                    
                    if (Mathf.Abs(m_EnemyTransform.position.x - m_TargetList[m_InteractIdx].transform.position.x) > 0.05f)
                    {
                        m_Enemy.ResetMovePoint( m_TargetList[m_InteractIdx].transform.position);
                        m_Enemy.SetRigidToPoint();
                    }
                    else
                    {
                        if (m_Enemy.p_SmartUseRange.CanInteract())
                        {
                            m_Enemy.p_SmartUseRange.GetUseableObjbyIDX(0).useObj(new IUseableObjParam(m_EnemyTransform,false,m_Enemy.GetInstanceID()));
                            m_InteractIdx++;
                        }
                    }
                    break;
            }
            
            /*
            m_PathLists = m_Enemy.p_SectionManager.GetPathtoPlayer(
                m_Enemy.p_SectionSensor,
                GameMgr.GetInstance().p_PlayerMgr.GetPlayer().m_PlayerSectionSensor);

            int colorNum = 0;
            foreach (var element in m_PathLists)
            {
                for (int i = 0; i < element.Count; i++)
                {
                    if (i == 0)
                    {
                        Debug.DrawLine(element[i].transform.position, element[i].OtherSideDoor.transform.position,
                            GetColor(colorNum));
                            
                        Debug.DrawLine(element[i].transform.position, m_EnemyTransform.position,GetColor(colorNum));
                    }
                    else
                    {
                        Debug.DrawLine(element[i].transform.position, element[i - 1].OtherSideDoor.transform.position,
                            GetColor(colorNum));
                            
                        Debug.DrawLine(element[i].OtherSideDoor.transform.position, element[i].transform.position,
                            GetColor(colorNum));
                    }

                    if (i == element.Count - 1)
                    {
                        Debug.DrawLine(GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform.position, 
                            element[i].OtherSideDoor.transform.position, GetColor(colorNum));
                    }
                }
                    
                    
                colorNum++;
            }
            */
        }
    }

    private Color GetColor(int _num)
    {
        switch (_num)
        {
            case 0:
                return Color.magenta;
                break;
            
            case 1:
                return Color.green;
                break;
            
            case 2:
                return Color.cyan;
                break;
            
            case 3:
                break;
        }
        
        return Color.black;
    }

    public override void ExitState()
    {
        m_Enemy.ResetRigid();
    }

    public override void NextPhase()
    {
        
    }
}