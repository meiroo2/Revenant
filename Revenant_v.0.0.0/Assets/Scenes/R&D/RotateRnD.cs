using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRnD : MonoBehaviour
{
    // Visible Member Variables
    public bool m_doRotate = true;
    public int p_RotationPhaseNum = 5;
    
    public float m_rotationHighLimitAngle = 65f;
    public float m_rotationLowLimitAngle = -45f;

    [field: SerializeField] public float m_curActualAngle { get; private set; }
    [field: SerializeField] public float m_curAnglewithLimit { get; private set; }
    [field: SerializeField] public int m_curAnglePhase { get; private set; } = 0;


    // Member Variables
    private Camera m_mainCam;

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
        m_PhaseAngle = (Mathf.Abs(m_rotationHighLimitAngle) + Mathf.Abs(m_rotationLowLimitAngle)) / p_RotationPhaseNum;
    }


    // Updates
    private void Update()
    {
        if (!m_doRotate)
            return;

        getAngle();
        doRotate();
    }


    // Functions
    private void getAngle()
    {
        mousePos = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
        m_Mousedistance = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        m_curActualAngle = Mathf.Atan2(m_Mousedistance.y, m_Mousedistance.x) * Mathf.Rad2Deg;


        // Limit Cut�� Angle ���ϱ�
        m_curAnglewithLimit = m_curActualAngle;
        if (m_curAnglewithLimit > m_rotationHighLimitAngle)
            m_curAnglewithLimit = m_rotationHighLimitAngle;
        else if (m_curAnglewithLimit < m_rotationLowLimitAngle)
            m_curAnglewithLimit = m_rotationLowLimitAngle;


        // ���� �� �������Ѽ��� �������Ѽ��� ���� �� 5�� ������ ������ ����.
        // �� ������ �������� ��밡�� ���� ������ 5���� �ɰ�
        // �������� ����, �Ʒ������� ���ܷ� ����Ѵ�. �� ���� �ȿ� ������ ���ʺ��� 0, 1, 2, 3, 4�� m_curAnglePhase�� �Ҵ�
        // �ش� ������ Init Angle�� �޾ƿͼ� ���. -> ���� Limit�� �پ�� �ʱ� ������ m_curAnglePhase�� �ٲ�
        for (int i = 0; i < p_RotationPhaseNum; i++)
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
        //toRotation = Quaternion.Euler(0f, 0f, m_curAnglewithLimit);

        float angle = m_rotationHighLimitAngle;
        angle -= m_curAnglePhase * m_PhaseAngle;
        toRotation = Quaternion.Euler(0f, 0f, angle);
        
        transform.rotation = toRotation;
    }
}
