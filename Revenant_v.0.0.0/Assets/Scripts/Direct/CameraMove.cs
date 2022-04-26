using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Visible Member Variables
    public float m_cameraSpeed = 1f;
    private GameObject m_Player;
    public bool m_FollowMouse = true;
    public float m_OffSet;

    public bool m_InitFollow = true;

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

            m_cameraPos.y += m_OffSet;
            m_cameraPos.z = -10f;

            transform.position = Vector3.Lerp(transform.position, m_cameraPos, Time.deltaTime * 4f);
            transform.position = StaticMethods.getPixelPerfectPos(transform.position);
        }
    }
    private void Update()
    {
        m_cameraPos = m_Player.transform.position;

        if(!m_FollowMouse)
        {
            m_cameraPos.z = -10f;
            transform.position = m_cameraPos;
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

    // 기타 분류하고 싶은 것이 있을 경우
}
