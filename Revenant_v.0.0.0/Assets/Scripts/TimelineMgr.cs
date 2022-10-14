using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class TimelineMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject[] p_Timelines;
    public GameObject p_DeathTimeline;
    public Canvas p_CamCanvas;

    
    // Member Variables
    private static TimelineMgr m_Instance = null;
    private Player m_Player;
    
    
    // Delegates
    public delegate void TimelineDelegate();
    private TimelineDelegate m_Callback = null;
    

    public static TimelineMgr GetInstance()
    {
        return m_Instance;
    }


    // Constructors
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            if (this != m_Instance)
            {
                Destroy(this.gameObject);
            }
        }

        for (int i = 0; i < p_Timelines.Length; i++)
        {
            p_Timelines[i].SetActive(false);
        }

        p_CamCanvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }

    public void DeathTimelineEnd()
    {
        GameMgr.GetInstance().m_CanInputAnyKey = true;
        Debug.Log("Signal Received");
    }

    public void StartDeathTimeline()
    {
        transform.position = m_Player.transform.position;
        SetAllTimelinesOff();
        p_DeathTimeline.SetActive(true);
    }

    public void StartTimeline(int _idx)
    {
        
    }

    private void SetAllTimelinesOff()
    {
        p_DeathTimeline.SetActive(false);
        for (int i = 0; i < p_Timelines.Length; i++)
        {
            p_Timelines[i].SetActive(false);
        }
    }
    
    // Functions
    public void SetCallback(TimelineDelegate _input)
    {
        m_Callback = null;
        m_Callback = _input;
    }
}
