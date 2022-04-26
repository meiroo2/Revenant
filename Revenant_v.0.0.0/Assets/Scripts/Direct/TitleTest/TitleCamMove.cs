using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamMove : MonoBehaviour
{
    // Visible Member Variables
    public float m_cameraSpeed = 1f;
    public bool m_FollowMouse = false;

    // Member Variables
    private Vector3 m_cameraPos;
    private Vector2 m_MousePos;
    private Vector3 m_OriginCamPos;

    // Constructors
    private void Awake()
    {
        m_OriginCamPos = transform.position;
    }



    // Updates
    private void Update()
    {
        if (m_FollowMouse)
        {
            m_MousePos = Input.mousePosition;
            m_cameraPos = m_OriginCamPos;
            m_cameraPos.x += (m_MousePos.x - 960) / 13000f;
            m_cameraPos.y += (m_MousePos.y - 540) / 13000f;
        }
        m_cameraPos.z = -10f;
        transform.position = Vector3.Lerp(transform.position, m_cameraPos, Time.deltaTime * m_cameraSpeed);
    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
