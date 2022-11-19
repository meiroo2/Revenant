using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawn : MonoBehaviour
{
    public bool p_SendToBtnMgr = false;
    
    private Vector3 m_OriginPos;
    private Vector3 m_SpawnPos;

    private TempParaBack m_ParaBack;
    private TitleCamMove m_CamMove;

    private float m_Timer = 3f;

    private bool m_Activated = false;

    private void Awake()
    {
        m_Activated = false;
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
        if (m_Activated)
            return;
        
        if(m_Timer >= 0f)
        {
            m_Timer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, m_OriginPos, Time.deltaTime * 3f);
        }
        else
        {
            m_CamMove.enabled = true;
            m_ParaBack.enabled = true;
            m_Activated = true;
            m_Activated = true;
            /*
            if(p_SendToBtnMgr)
                GameObject.FindObjectOfType<Title_BtnMgr>().ActiveBtn(true);
                */
        }
    }
}