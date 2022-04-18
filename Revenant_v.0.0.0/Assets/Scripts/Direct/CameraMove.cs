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
        m_cameraPos = Vector3.Lerp(m_cameraPos, m_Player.transform.position, Time.deltaTime * m_cameraSpeed);
        m_cameraPos.z = -10f;

        if (m_FollowMouse)
        {
            m_MousePos = Input.mousePosition;
            m_cameraPos.x += (m_MousePos.x - 960) / 15000f;
            m_cameraPos.y += (m_MousePos.y - 540) / 15000f;

            m_cameraPos.y += m_OffSet;
        }


        transform.position = m_cameraPos;

        /*
        m_cameraPos.x = Mathf.RoundToInt(m_cameraPos.x * 100);
        m_cameraPos.y = Mathf.RoundToInt(m_cameraPos.y * 100);

        m_cameraPos.x = m_cameraPos.x / 100;
        m_cameraPos.y = m_cameraPos.y / 100;
        */
    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
