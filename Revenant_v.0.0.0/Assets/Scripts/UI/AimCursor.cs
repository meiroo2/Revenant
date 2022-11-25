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
    public float p_AimRaycastRadius = 0.5f;

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
        var player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        
        m_PlayerTransform = player.transform;
        m_PlayerAniMgr = player.m_PlayerAniMgr;
    }

    private void OnDestroy()
    {
        StopCoroutine(m_UpdateCoroutine);
    }


    // Updates
    private IEnumerator UpdateRoutine()
    {
        Vector3 screenMousePos;
        
        while (true)
        {
            screenMousePos = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
            screenMousePos.z = 0f;
            transform.position = screenMousePos;
            CalculateAimRaycast();

            yield return null;
        }
    }

    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_HitBoxInfo == HitBoxPoint.COGNITION)
                hotBox.HitHotBox(new IHotBoxParam(10, 0, transform.position,
                    WeaponType.MOUSE));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_HitBoxInfo == HitBoxPoint.COGNITION)
                hotBox.HitHotBox(new IHotBoxParam(0, 0, transform.position,
                    WeaponType.MOUSE));
        }
    }


    // Functions
    
    /// <summary>
    /// 현재 위치에 충돌하고 있는 HotBox를 검출합니다.
    /// </summary>
    private void CalculateAimRaycast()
    {
        m_HitCount = Physics2D.CircleCastNonAlloc(transform.position,
            p_AimRaycastRadius, Vector2.zero, m_Lists, 1f, m_LayerMask);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, p_AimRaycastRadius);
    }
    */

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
                if (m_Lists[0].collider.CompareTag("Player"))
                {
                    return null;
                    break;
                }
                
                return new AimedColInfo(m_Lists[0].collider, transform.position);
                break;

            case > 1:
                Collider2D returnCol = m_Lists[0].collider;
                if (returnCol.CompareTag("Player"))
                {
                    returnCol = m_Lists[1].collider;
                }
                
                float minDistance = (transform.position - returnCol.transform.position).sqrMagnitude;
                
                for (int i = 1; i < m_ColList.Count; i++)
                {
                    if( m_Lists[i].collider.CompareTag("Player"))
                        continue;
                    
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
