using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParaBack : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 m_BackPosVec;
    private Vector3 m_OriginPos;
    public bool m_isPixelPerfectCam = true;
    public float m_ParaValue = 0.1f;

    private void Awake()
    {
        mainCam = Camera.main;
        m_OriginPos = transform.position;
    }

    private void Update()
    {
        m_BackPosVec = m_OriginPos;

        m_BackPosVec.x = m_BackPosVec.x + (m_OriginPos.x - mainCam.transform.position.x) * m_ParaValue;
        m_BackPosVec.y = m_BackPosVec.y + (m_OriginPos.y - mainCam.transform.position.y) * m_ParaValue;

        if(m_isPixelPerfectCam)
        {
            m_BackPosVec.x = Mathf.RoundToInt(m_BackPosVec.x * 100);
            m_BackPosVec.y = Mathf.RoundToInt(m_BackPosVec.y * 100);

            m_BackPosVec.x = m_BackPosVec.x / 100;
            m_BackPosVec.y = m_BackPosVec.y / 100;
        }

        transform.position = m_BackPosVec;
    }
}
