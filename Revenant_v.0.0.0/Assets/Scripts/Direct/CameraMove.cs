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

    private bool m_isCamStuck = false;
    private bool m_FollowMouse = false;


    // Member Variables
    private Vector3 m_cameraPos;
    private Vector2 m_MousePos;

    // Constructors
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.gameObject;
        m_cameraPos = m_Player.transform.position;

        if (m_InitFollow)
            transform.position = m_Player.transform.position;
        else
            m_cameraPos = transform.position;

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void FixedUpdate()
    {
        if (m_FollowMouse)
        {
            m_MousePos = Input.mousePosition;
            m_cameraPos.x += (m_MousePos.x - 960) / 1100f;
            m_cameraPos.y += (m_MousePos.y - 540) / 1100f;

            m_cameraPos.y += m_yOffSet;
            m_cameraPos.z = -10f;
        }
        
        if (!m_isCamStuck)
        {
            transform.position = Vector3.Lerp(transform.position, m_cameraPos, Time.deltaTime * 4f);
            transform.position = StaticMethods.getPixelPerfectPos(transform.position);
        }
    }
    private void Update()
    {
        m_cameraPos = m_Player.transform.position;

        if (m_EffectedByCamBound)
        {
            if (m_CamBoundMgr.canCamMove(m_cameraPos) == true)
                m_isCamStuck = false;
            else
                m_isCamStuck = true;
        }

        if (!m_FollowMouse)
        {
            m_cameraPos.z = -10f;
            //transform.position = m_cameraPos;
        }


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
