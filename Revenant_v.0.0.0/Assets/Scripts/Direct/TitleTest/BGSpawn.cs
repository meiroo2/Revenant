using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawn : MonoBehaviour
{
    private Vector3 m_OriginPos;
    private Vector3 m_SpawnPos;

    private TempParaBack m_ParaBack;
    private TitleCamMove m_CamMove;

    private float m_Timer = 3f;

    private void Awake()
    {
        m_CamMove = Camera.main.gameObject.GetComponent<TitleCamMove>();
        m_CamMove.enabled = false;
        m_ParaBack = GetComponent<TempParaBack>();
        m_ParaBack.enabled = false;
        m_OriginPos = transform.position;
        m_SpawnPos = m_OriginPos;
    }
    private void Start()
    {
        m_SpawnPos.y = -(m_SpawnPos.z * 2f);
        transform.position = m_SpawnPos;
    }

    private void Update()
    {
        if(m_Timer >= 0f)
        {
            m_Timer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, m_OriginPos, Time.deltaTime * 3f);
        }
        else
        {
            m_CamMove.enabled = true;
            m_ParaBack.enabled = true;
        }
    }
}