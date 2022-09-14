using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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
    [ReadOnly] public GameObject p_AimedObj;

    // Member Variables
    private Camera m_MainCamera;
    private Transform m_PlayerTransform;
    private Player_AniMgr m_PlayerAniMgr;
    private Coroutine m_UpdateCoroutine;

    private List<Collider2D> m_ColList;
    
    RaycastHit2D[] m_Lists = new RaycastHit2D[10];
    private int m_HitCount;
    private int m_LayerMask;

    // Constructors
    private void Awake()
    {
        m_ColList = new List<Collider2D>();
        m_MainCamera = Camera.main;
        
        m_UpdateCoroutine = StartCoroutine(UpdateRoutine());
        
        m_LayerMask =
            (1 << LayerMask.NameToLayer("HotBoxes"));
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
            CalculateAimRaycast();

            yield return null;
        }
    }


    // Functions
    
    /// <summary>
    /// 현재 위치에 충돌하고 있는 HotBox를 검출합니다.
    /// </summary>
    private void CalculateAimRaycast()
    {
        m_HitCount = Physics2D.RaycastNonAlloc(transform.position,
            Vector2.zero, m_Lists, 1f, m_LayerMask);
    }
    
    
    /// <summary>
    /// m_HitCount의 Ray 판정 검사 후, 가장 가까운 콜라이더를 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public AimedColInfo GetAimedColInfo()
    {
        switch (m_HitCount)
        {
            case 0:
                return null;
                break;

            case 1:
                return new AimedColInfo(m_Lists[0].collider, transform.position);
                break;

            case > 1:
                Collider2D returnCol = m_Lists[0].collider;
                float minDistance = (transform.position - returnCol.transform.position).sqrMagnitude;
                
                for (int i = 1; i < m_ColList.Count; i++)
                {
                    float distance = (transform.position - m_Lists[i].collider.transform.position).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        returnCol = m_Lists[i].collider;
                        minDistance = distance;
                    }
                }

                return new AimedColInfo(returnCol, transform.position);
                break;
        }

        return null;
    }
}
