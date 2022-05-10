using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // Visible Member Variables
    [Header("회전 각도는 총 5단위로 나누어집니다.")]
    public bool m_doRotate = true;
    public float m_rotationHighLimitAngle { get; private set; } = 65f;
    public float m_rotationLowLimitAngle { get; private set; } = -45f;

    [field: SerializeField] public float m_curActualAngle { get; private set; }
    [field: SerializeField] public float m_curAnglewithLimit { get; private set; }
    [field: SerializeField] public int m_curAnglePhase { get; private set; } = 0;


    // Member Variables
    private Camera m_mainCam;
    private Player m_Player;

    private Vector2 mousePos;
    private Vector2 m_Mousedistance;
    private Quaternion toRotation;
    private float m_PhaseAngle = 0;
    private float m_InitHighLimitAngle;
    private float m_InitLowLimitAngle;


    // Constructors
    private void Awake()
    {
        m_InitHighLimitAngle = m_rotationHighLimitAngle;
        m_InitLowLimitAngle = m_rotationLowLimitAngle;
        m_mainCam = Camera.main;
        m_PhaseAngle = (Mathf.Abs(m_rotationHighLimitAngle) + Mathf.Abs(m_rotationLowLimitAngle)) / 5;
    }
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }


    // Updates
    private void Update()
    {
        if (!m_doRotate)
            return;

        if (m_curActualAngle > 90f || m_curActualAngle < -90f)
        {
            if (m_Player.m_isRightHeaded)
                m_Player.setisRightHeaded(false);
            else
                m_Player.setisRightHeaded(true);
        }

        getAngle();
        doRotate();
    }


    // Functions
    private void getAngle()
    {
        mousePos = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
        m_Mousedistance = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        if (m_Player.m_isRightHeaded)
        {
            m_curActualAngle = Mathf.Atan2(m_Mousedistance.y, m_Mousedistance.x) * Mathf.Rad2Deg;
        }
        else if (!m_Player.m_isRightHeaded)
        {
            m_curActualAngle = -(Mathf.Atan2(-m_Mousedistance.y, -m_Mousedistance.x) * Mathf.Rad2Deg);
        }

        // Limit Cut된 Angle 구하기
        m_curAnglewithLimit = m_curActualAngle;
        if (m_curAnglewithLimit > m_rotationHighLimitAngle)
            m_curAnglewithLimit = m_rotationHighLimitAngle;
        else if (m_curAnglewithLimit < m_rotationLowLimitAngle)
            m_curAnglewithLimit = m_rotationLowLimitAngle;


        // 시작 시 각도상한선과 각도하한선을 더한 후 5로 나눠서 각도를 구함.
        // 이 각도를 기준으로 사용가능 각도 범위를 5개로 쪼갬
        // 윗각도는 포함, 아랫각도는 제외로 계산한다. 이 범위 안에 있으면 위쪽부터 0, 1, 2, 3, 4로 m_curAnglePhase에 할당
        // 해당 각도는 Init Angle을 받아와서 계산. -> 만약 Limit가 줄어들어도 초기 값으로 m_curAnglePhase가 바뀜
        for (int i = 0; i < 5; i++)
        {
            if (m_curAnglewithLimit <= m_InitHighLimitAngle - m_PhaseAngle * i &&
                m_curAnglewithLimit > m_InitHighLimitAngle - m_PhaseAngle * (i + 1))
            {
                m_curAnglePhase = i;
                break;
            }
        }
    }
    private void doRotate()
    {
        if (m_Player.m_isRightHeaded)
            toRotation = Quaternion.Euler(0f, 0f, m_curAnglewithLimit);
        else
            toRotation = Quaternion.Euler(0f, 0f, -m_curAnglewithLimit);

        transform.rotation = toRotation;
    }

    public void changeAngleLimit(float _HighLimit, float _LowLimit)
    {
        if (_HighLimit > 0f && _LowLimit < 0f)
        {
            m_rotationHighLimitAngle = _HighLimit;
            m_rotationLowLimitAngle = _LowLimit;

            //m_PhaseAngle = (Mathf.Abs(m_rotationHighLimitAngle) + Mathf.Abs(m_rotationLowLimitAngle)) / 5;
        }
        else
            Debug.Log("PlayerRotation Angle Param Error");
    }
}
