using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 마우스 위치에 따라 해당 스크립트가 붙은 오브젝트를 회전시킵니다.
/// </summary>
public class PlayerRotation : MonoBehaviour
{
    // Visible Member Variables
    public int p_PhaseCount = 13;
    public bool m_doRotate = true;
    
    [field : SerializeField]
    public float m_rotationHighLimitAngle { get; private set; } = 60f;
    
    [field : SerializeField]
    public float m_rotationLowLimitAngle { get; private set; } = -60f;

    [field: SerializeField] public float m_curActualAngle { get; private set; }
    [field: SerializeField] public float m_curAnglewithLimit { get; private set; }
    [field: SerializeField] public int m_curAnglePhase { get; private set; } = 0;


    // Member Variables
    private Transform m_AimCursor;
    
    private Player m_Player;

    private Camera m_MainCam;
    private Player_InputMgr m_InputMgr;

    public bool m_BanFlip = false;
    private Vector2 m_mousePos;
    private Vector2 m_MouseDistance;
    private Quaternion toRotation;
    private float m_PhaseAngle = 0f;
    private float m_InitHighLimitAngle;
    private float m_InitLowLimitAngle;

    // AnglePhase가 바뀔 떄 호출하는 Action, 그런데 안 써요~
    public Action<int> m_AnglePhaseAction; 

    // Constructors
    private void Awake()
    {
        m_InitHighLimitAngle = m_rotationHighLimitAngle;
        m_InitLowLimitAngle = m_rotationLowLimitAngle;
        m_PhaseAngle = (Mathf.Abs(m_rotationHighLimitAngle) + Mathf.Abs(m_rotationLowLimitAngle)) / p_PhaseCount;
    }
    private void Start()
    {
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_MainCam = Camera.main;
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
        m_AimCursor = InstanceMgr.GetInstance().GetComponentInChildren<AimCursor>().transform;
    }


    // Updates
    private void Update()
    {
        GetAngle();

        if (!m_doRotate)
            return;
        
        //DoRotate();

        if (m_BanFlip)
            return;
        
        if (m_curActualAngle is > 90f or < -90f)
        {
            m_Player.setisRightHeaded(!m_Player.m_IsRightHeaded);
        }
    }


    // Functions
    private void GetAngle()
    {
        m_mousePos = m_MainCam.ScreenToWorldPoint(m_InputMgr.m_MousePos); 
        m_MouseDistance = new Vector2(m_mousePos.x - transform.position.x, m_mousePos.y - transform.position.y);
        
        // 현재 바라보는 방향에 따라 양수, 음수를 결정
        if (m_Player.m_IsRightHeaded)
        {
            m_curActualAngle = Mathf.Atan2(m_MouseDistance.y, m_MouseDistance.x) * Mathf.Rad2Deg;
        }
        else if (!m_Player.m_IsRightHeaded)
        {
            m_curActualAngle = -(Mathf.Atan2(-m_MouseDistance.y, -m_MouseDistance.x) * Mathf.Rad2Deg);
        }
        
        // 각도 제한 보정 (딱 Limit까지는 각도 인정)
        m_curAnglewithLimit = m_curActualAngle;
        if (m_curAnglewithLimit > m_rotationHighLimitAngle)
            m_curAnglewithLimit = m_rotationHighLimitAngle;
        else if (m_curAnglewithLimit < m_rotationLowLimitAngle)
            m_curAnglewithLimit = m_rotationLowLimitAngle;



        int originPhase = m_curAnglePhase;
        // 현재 페이즈 계산
        for (int i = 0; i < p_PhaseCount; i++)
        {
            if (m_curAnglewithLimit <= m_InitHighLimitAngle - m_PhaseAngle * i && 
                m_curAnglewithLimit > m_InitHighLimitAngle - m_PhaseAngle * (i + 1))
            {
                m_curAnglePhase = i;
                break;
            }
        }

        if (originPhase != m_curAnglePhase)
        {
            m_AnglePhaseAction?.Invoke(m_curAnglePhase);
        }
    }
    
    public void DoRotate()
    {
        toRotation = 
            m_Player.m_IsRightHeaded ? Quaternion.Euler(0f, 0f, m_curAnglewithLimit) : 
                Quaternion.Euler(0f, 0f, -m_curAnglewithLimit);

        transform.rotation = toRotation;
    }

    public void ChangeAngleLimit(float _HighLimit, float _LowLimit)
    {
        if (_HighLimit > 0f && _LowLimit < 0f)
        {
            m_rotationHighLimitAngle = _HighLimit;
            m_rotationLowLimitAngle = _LowLimit;
            GetAngle();
            DoRotate();
        }
        else
            Debug.Log("PlayerRotation Angle Param Error");
    }

    public bool GetIsMouseRight()
    {
        return m_AimCursor.position.x > m_Player.transform.position.x;
    }
}
