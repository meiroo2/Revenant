using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead_UI : MonoBehaviour, IUI
{
    public bool m_isActive { get; set; } = false;
    private GameObject m_Player;
    private bool isFollowing = false;
    private Image m_Image;
    private RectTransform m_rectT;
    private Camera m_mainCam;

    private Vector2 m_PlayerHeadUIPos;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_rectT = GetComponent<RectTransform>();
        m_mainCam = Camera.main;
    }

    private void Start()
    {
        m_Player =  GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.gameObject;
    }

    private void Update()
    {
        m_PlayerHeadUIPos = m_Player.transform.position;
        m_PlayerHeadUIPos.y += 0.35f;
        m_rectT.anchoredPosition = Vector2.Lerp(m_rectT.anchoredPosition, m_mainCam.WorldToScreenPoint(m_PlayerHeadUIPos), Time.deltaTime * 30f);
        //m_rectT.anchoredPosition = m_mainCam.WorldToScreenPoint(m_PlayerHeadUIPos);
    }


    public int ActivateUI(IUIParam _input)
    {
        if(_input.m_ToActive == false)
        {
            isFollowing = false;
            m_Image.enabled = false;
        }
        else if(_input.m_ToActive == true)
        {
            isFollowing = true;
            m_Image.enabled = true;
        }
        return 0;
    }
}
