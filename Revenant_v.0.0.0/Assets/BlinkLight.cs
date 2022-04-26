using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlinkLight : MonoBehaviour
{
    Light2D m_WillLight;

    private bool m_isOn = true;
    private float m_Timer = 1f;

    private void Awake()
    {
        m_WillLight = GetComponent<Light2D>();
    }
    private void Update()
    {
        if (m_isOn)
        {
            m_WillLight.enabled = true;
            m_Timer -= Time.deltaTime;
            if(m_Timer <= 0f)
            {
                m_Timer = Random.Range(0.05f, 1f);
                m_isOn = false;
            }
        }
        else
        {
            m_WillLight.enabled = false;
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0f)
            {
                m_Timer = Random.Range(0.05f, 1f);
                m_isOn = true;
            }
        }
    }

}