using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandleManager : MonoBehaviour
{
    public static DataHandleManager Instance;

    public bool IsCheckPointActivated { get; set; }
    public int CheckPointSectionNumber { get; set; }

    public Vector2 PlayerPositionVector { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
