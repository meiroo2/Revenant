using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataHandleManager : MonoBehaviour
{
    public bool IsCheckPointActivated { get; set; }
    public int CheckPointSectionNumber { get; set; }
    public Vector2 PlayerPositionVector { get; set; }
    public Vector2 m_OriginPlayerPos { get; private set; }

    private void Awake()
    {
        Debug.Log("DataHandleMgr Awake");
        SetDataHandleMgr();
    }

    public void SetDataHandleMgr()
    {
        Debug.Log("DataHandleMgr OnSceneLoaded");
        if (!IsCheckPointActivated)
        {
            GameObject findPlayer = GameObject.FindWithTag("@Player");

            if (!ReferenceEquals(findPlayer, null))
            {
                m_OriginPlayerPos = findPlayer.transform.position;
            }
            else
            {
                m_OriginPlayerPos = Vector2.zero;
            }
        }
    }

    public void ResetCheckPoint()
    {
        IsCheckPointActivated = false;
    }
}
