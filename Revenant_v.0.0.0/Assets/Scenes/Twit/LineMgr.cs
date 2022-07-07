using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{
    private LineRenderer m_Renderer;

    private Transform m_LineStartTransform;
    private Transform m_LineEndTransform;

    private Color m_Color;
    private Coroutine m_Coroutine;

    private void Awake()
    {
        m_Renderer = GetComponent<LineRenderer>();
        
        m_Color = new Color(1, 1, 1, 0);
        m_Renderer.startColor = m_Color;
        m_Renderer.endColor = m_Color;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();

        m_LineStartTransform =
            instance.GetComponentInChildren<Player_Manager>().m_Player.m_WeaponMgr.m_CurWeapon.transform;
        
        m_LineEndTransform = instance.GetComponentInChildren<AimCursor>().transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Color.a = 1;
            m_Renderer.startColor = m_Color;
            m_Renderer.endColor = m_Color;
            
            m_Renderer.SetPosition(0, m_LineStartTransform.position);
            m_Renderer.SetPosition(1, m_LineEndTransform.position);

            if(m_Coroutine != null)
                StopCoroutine(m_Coroutine);
            
            m_Coroutine = StartCoroutine(DrawBulletLine());
        }
    }

    private IEnumerator DrawBulletLine()
    {
        yield return new WaitForSeconds(0.05f);

        while (true)
        {
            m_Color.a -= Time.deltaTime * 5f;
            m_Renderer.startColor = m_Color;
            m_Renderer.endColor = m_Color;

            if (m_Color.a <= 0f)
                break;
            
            yield return null;
        }
    }
}
