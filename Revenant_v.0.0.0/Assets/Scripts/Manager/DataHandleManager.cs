using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataHandleManager : MonoBehaviour
{
    public static DataHandleManager Instance;

    public bool IsCheckPointActivated { get; set; }
    public int CheckPointSectionNumber { get; set; }
    public Vector2 PlayerPositionVector { get; set; }

    public Vector2 m_OriginPlayerPos { get; private set; }

    private void Awake()
    {
        if (!IsCheckPointActivated)
        {
            m_OriginPlayerPos = GameObject.FindWithTag("@Player").transform.position;
        }
        
        DontDestroyOnLoad(transform.root.gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
}
