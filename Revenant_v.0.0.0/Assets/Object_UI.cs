using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_UI : MonoBehaviour, IUI
{
    public Transform m_FollowingObj;
    public bool m_isActive { get; set; } = false;
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

    private void Update()
    {
        m_rectT.anchoredPosition = m_mainCam.WorldToScreenPoint(m_FollowingObj.position);
        //m_rectT.anchoredPosition = Vector2.Lerp(m_rectT.anchoredPosition, m_mainCam.WorldToScreenPoint(m_PlayerHeadUIPos), Time.deltaTime * 30f);
    }


    public int ActivateIUI(IUIParam _input)
    {
        if (_input.m_ToActive == false)
        {
            isFollowing = false;
            m_Image.enabled = false;
        }
        else if (_input.m_ToActive == true)
        {
            isFollowing = true;
            m_Image.enabled = true;
        }
        return 0;
    }
}
