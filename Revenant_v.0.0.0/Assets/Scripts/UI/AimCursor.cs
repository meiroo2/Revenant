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
    

    /*
     // Updates
    private void Update()
    {
        m_CursorPos = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = m_CursorPos;
        
        
        m_Dist_Aim_Player = Vector2.SqrMagnitude(transform.position - m_PlayerTransform.position);

        CalculateNearestHotBox();

        if (m_canAimCursorShot && m_Dist_Aim_Player < p_FireMinimumDistance)
        {
            m_canAimCursorShot = false;
            m_PlayerAniMgr.changeArm(1);
        }
        else if(!m_canAimCursorShot && m_Dist_Aim_Player > p_FireMinimumDistance)
        {
            m_canAimCursorShot = true;
            m_PlayerAniMgr.changeArm(0);
        }
        
    }
    

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player의 Hotbox Collider 제외
        if (collision.CompareTag("Player"))
            return;
        
        // 충돌한 히트박스의 오브젝트 정보(InstanceID, Position)를 리스트에 담음
        m_AimedObjs.Add(new AimedObjInfo(collision, collision.GetInstanceID(), collision.transform.position));
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Exit 시 Exit한 히트박스는 리스트에서 제거
        if(m_AimedObjs.Count > 0)
        {
            for(int i = 0; i < m_AimedObjs.Count; i++)
            {
                if (m_AimedObjs[i].m_ColliderInstanceID == collision.GetInstanceID())
                {
                    m_AimedObjs.RemoveAt(i);
                    break;
                }
            }

            // 만약 LIst 비었으면 조준한 오브젝트 Id를 -1로 재할당
            if (m_AimedObjs.Count == 0)
            {
                AimedObjid = -1;
                m_ShortestIdx = -1;
                AimedObjName = "";
            }
        }
    }

    // Functions
    public Vector2 GetAimCursorPos()
    {
        return transform.position;
    }
    
    public HitscanTargetInfo GetHitscanTargetInfo()
    {
        List<RaycastHit2D> rayHitList = new List<RaycastHit2D>();
        rayHitList = m_PlayerHitscanRay.GetRayHits();

        if (ReferenceEquals(rayHitList, null))
        {
            // Ray에 아무것도 맞지 않음(빈 공간)
            return new HitscanTargetInfo(0, transform.position);
        }
        else
        {
            // Ray에 무언가 검출됨
            int coinsideIdx = -1;
            
            // Aimcursor와 충돌한 대상들과 Ray에 맞은 대상이 같은지 비교
            for (int i = 0; i < m_AimedObjs.Count; i++)
            {
                for (int j = 0; j < rayHitList.Count; j++)
                {
                    if (m_AimedObjs[i].m_Collider2d == rayHitList[j].collider)
                    {
                        coinsideIdx = i;
                        break;
                    }
                }

                if (coinsideIdx != -1)
                    break;
            }

            if (coinsideIdx == -1)
            {
                // 벽 맞음
                return new HitscanTargetInfo(1, m_PlayerHitscanRay.GetPosWhenAimFailed());
            }
            else
            {
                // 명중
                return new HitscanTargetInfo(2,m_AimedObjs[coinsideIdx].m_Collider2d.GetComponent<IHotBox>(),
                    transform.position);
            }
        }
    }

    private void CalculateNearestHotBox()
    {
        if (m_AimedObjs.Count > 0)
        {
            m_ShortestLength = Vector2.Distance(transform.position, m_AimedObjs[0].m_ColliderPos);
            m_ShortestIdx = 0;

            for (int i = 0; i < m_AimedObjs.Count; i++)
            {
                m_TempLength = Vector2.Distance(transform.position, m_AimedObjs[i].m_ColliderPos);
                if (m_TempLength < m_ShortestLength)
                {
                    m_ShortestLength = m_TempLength;
                    m_ShortestIdx = i;
                }
            }

            AimedObjid = m_AimedObjs[m_ShortestIdx].m_ColliderInstanceID;
            AimedObjName = m_AimedObjs[m_ShortestIdx].m_Collider2d.name;
        }
    }
    private void DeleteElementUsingIdx(int _idx)
    {
        // Idx Out of range
        if (_idx < 0 || _idx >= m_CurInfoArrLength)
            return;

        // 마지막 요소 제거가 아닐 시,마지막 요소 전까지 뒷 요소를 나아가며 본인에게 대입
        if (_idx != m_CurInfoArrLength - 1)
        {
            for (int i = _idx; i < m_CurInfoArrLength - 1; i++)
            {
                m_AimedColInfoArr[i] = m_AimedColInfoArr[i + 1];
            }
        }

        m_CurInfoArrLength--;
    }
    private void AddElement(Collider2D _col)
    {
        // Out of range check
        if (m_CurInfoArrLength >= m_RO_ColInfoArrLength)
            return;

        m_AimedColInfoArr[m_CurInfoArrLength] = _col;
        
        m_CurInfoArrLength++;
    }
    
     */
}
