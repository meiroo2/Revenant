using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Cysharp;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class CamEftByJob : MonoBehaviour
{
    private Camera m_Camera;
    private float m_OrthoSize;
    
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_OrthoSize = m_Camera.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UniTask.SwitchToTaskPool();
            DoZoomnOut();
        }
    }

    async UniTaskVoid DoZoomnOut()
    {
        float AddZoom = 1f;
        while (true)
        {
            m_Camera.orthographicSize = m_OrthoSize + AddZoom;
            AddZoom -= Time.deltaTime;
            if (AddZoom <= 0f)
            {
                m_Camera.orthographicSize = m_OrthoSize;
                break;
            }
            await UniTask.Yield();
        }
    }
}