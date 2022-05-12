using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Visible Member Variables
    public CamBoundMgr m_CamBoundMgr;
    private GameObject m_Player;
    public bool m_EffectedByCamBound = false;
    public bool m_InitFollow = true;
    public float m_yOffSet = 0.03f;

    private bool m_CamStuckOnce = false;
    private bool m_FollowMouse = false;


    // Member Variables
    private Vector3 m_CameraPos;
    private Vector3 m_CameraTempPos;
    private Vector2 m_MousePos;

    // Constructors
    private void Start()
    {
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.gameObject;

        m_CameraPos = transform.position;

        if (m_InitFollow)
            m_CameraPos = m_Player.transform.position;

        transform.position = m_CameraPos;
    }

    // Updates
    private void FixedUpdate()
    {
        if (m_EffectedByCamBound)
        {
            if (!m_FollowMouse)
            {
                m_CameraPos.z = -10f;
                if (m_CamStuckOnce)
                {
                    m_CamStuckOnce = false;
                    m_CameraPos = m_CameraTempPos;
                }
                m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 4f);
                if (m_CamBoundMgr.canCamMove(m_CameraPos))
                {
                    transform.position = m_CameraPos;
                }
                else
                {
                    transform.position = m_CamBoundMgr.getNearCamPos(m_CameraPos);
                }
            }
            else
            {
                m_CameraPos.z = -10f;
                m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 6f);

                m_MousePos = Input.mousePosition;
                m_CameraPos.x += (m_MousePos.x - 960) / 6000f;
                m_CameraPos.y += (m_MousePos.y - 540) / 6000f;
                m_CameraPos.y += m_yOffSet;

                if (m_CamBoundMgr.canCamMove(m_CameraPos))
                {
                    transform.position = m_CameraPos;
                }
                else
                {
                    transform.position = m_CamBoundMgr.getNearCamPos(m_CameraPos);
                    m_CameraTempPos = transform.position;
                    m_CamStuckOnce = true;
                }
            }
        }
        else
        {
            if (!m_FollowMouse)
            {
                m_CameraPos.z = -10f;
                m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 4f);
            }
            else
            {
                m_CameraPos.z = -10f;

                m_MousePos = Input.mousePosition;
                m_CameraPos.x += (m_MousePos.x - 960) / 6000f;
                m_CameraPos.y += (m_MousePos.y - 540) / 6000f;
                m_CameraPos.y += m_yOffSet;

                m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 6f);
            }
            transform.position = m_CameraPos;
        }

        transform.position = StaticMethods.getPixelPerfectPos(transform.position);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            PreciseMode(1);
        else if (Input.GetMouseButtonUp(1))
            PreciseMode(0);
    }

    // Physics


    // Functions
    private void PreciseMode(int _input)
    {
        switch (_input)
        {
            case 0:
                m_FollowMouse = false;
                break;

            case 1:
                m_FollowMouse = true;
                break;
        }
    }
    public void InstantMoveToPlayer(Vector2 _playerOriginPos, Vector2 _PlayerMovePos)
    {
        float Relativex = _playerOriginPos.x - transform.position.x;
        float Relativey = _playerOriginPos.y - transform.position.y;

        m_CameraPos = _PlayerMovePos;
        m_CameraPos.z = -10f;

        m_CameraPos.x -= Relativex;
        m_CameraPos.y -= Relativey;

        m_CameraTempPos = m_CameraPos;
        transform.position = m_CameraPos;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
