using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParaBack : MonoBehaviour
{
    // Visible Member Variables
    public String m_StandardTransformName;
    
    
    // Member Variables
    private Transform m_StandardTransform = null;
    private Transform m_MainCamTransform;
    
    private Vector3 m_CalculatedPos;

    private Vector3 m_StandardPos;
    private Vector3 m_TransformOriginPos;
    private float m_XPosDifferenceBetStandard;
    private bool m_IsFound = false;
    
    public float m_ParaValue = 0.1f;

    private void Awake()
    {
        m_StandardTransform = GameObject.Find(m_StandardTransformName).transform;
        if (!m_StandardTransform)
        {
            Debug.Log("ERR : TempParaBack can't find m_StandardTransform");
        }
        else
        {
            m_IsFound = true;
        }
    }

    private void Start()
    {
        if(!m_IsFound)
            return;
        
        m_MainCamTransform = Camera.main.transform;
        
        m_StandardPos = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform.position;
        m_TransformOriginPos = transform.localPosition;

        m_XPosDifferenceBetStandard = m_TransformOriginPos.x - m_StandardTransform.position.x;
    }

    private void Update()
    {
        if(!m_IsFound)
            return;
        
        m_XPosDifferenceBetStandard = m_MainCamTransform.position.x - m_StandardTransform.position.x;
        m_CalculatedPos = m_TransformOriginPos;
        
        m_CalculatedPos.x += m_XPosDifferenceBetStandard * m_ParaValue;

        transform.localPosition = m_CalculatedPos;
    }
}
