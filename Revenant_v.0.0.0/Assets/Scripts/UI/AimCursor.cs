using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class AimedColInfo
{
    public readonly Collider2D m_Collider;
    public readonly Vector2 m_AimCursorPos;

    public AimedColInfo(Collider2D _col, Vector2 _pos)
    {
        m_Collider = _col;
        m_AimCursorPos = _pos;
    }
}

public class AimCursor : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private Camera m_MainCamera;
    private Transform m_PlayerTransform;
    private Player_AniMgr m_PlayerAniMgr;
    private Coroutine m_UpdateCoroutine;

    private List<Collider2D> m_ColList;


    // Constructors
    private void Awake()
    {
        m_ColList = new List<Collider2D>();
        
        m_MainCamera = Camera.main;
        m_UpdateCoroutine = StartCoroutine(UpdateRoutine());
    }
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        
        m_PlayerTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.transform;
        m_PlayerAniMgr = instance.GetComponentInChildren<Player_Manager>().m_Player.m_PlayerAniMgr;
    }
    private void OnDestroy()
    {
        StopCoroutine(m_UpdateCoroutine);
    }


    // Updates
    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            transform.position = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
            yield return null;
        }
    }

    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
       // Debug.Log("Sans");
        m_ColList.Add(col);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_ColList.Remove(other);
    }

    
    // Functions
    public AimedColInfo GetAimedColInfo()
    {
        Vector2 aimPos = transform.position;

        switch (m_ColList.Count)
        {
            case <= 0:
                return null;
                break;
            
            case 1:
                return new AimedColInfo(m_ColList[0], aimPos);
                break;
            
            default:
                Collider2D returnCol = m_ColList[0];
                float minDistance = (aimPos - (Vector2)returnCol.transform.position).sqrMagnitude;
                for (int i = 1; i < m_ColList.Count; i++)
                {
                    float distance = (aimPos - (Vector2)m_ColList[i].transform.position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        returnCol = m_ColList[i];
                        minDistance = distance;
                    }
                }

                return new AimedColInfo(returnCol, aimPos);
                break;
        }

    }
}
