using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Visible Member Variables
    public float m_cameraSpeed = 1f;
    public GameObject m_Player;

    // Member Variables
    private Vector3 m_cameraPos;

    // Constructors
    private void Awake()
    {
        m_cameraPos = transform.position;
    }
    private void Start()
    {

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        m_cameraPos = Vector3.Lerp(m_cameraPos, m_Player.transform.position, Time.deltaTime * m_cameraSpeed);
        m_cameraPos.z = -10f;

        transform.position = m_cameraPos;
    }
    private void FixedUpdate()
    {
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
