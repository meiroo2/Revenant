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
    [field: SerializeField] public float p_FireMinimumDistance { get; private set; } = 0.2f;
    [field: SerializeField] public float m_Dist_Aim_Player { get; private set; } = 0f;
    public bool m_canAimCursorShot = true;
    

    // Member Variables
    private Camera m_MainCamera;
    private Transform m_PlayerTransform;
    private Player_AniMgr m_PlayerAniMgr;
    private Coroutine m_UpdateCoroutine;

    private readonly int m_RO_ColInfoArrLength = 3;
    private Collider2D[] m_AimedColInfoArr;
    private int m_CurInfoArrLength = 0;


    // Constructors
    private void Awake()
    {
        m_AimedColInfoArr = new Collider2D[m_RO_ColInfoArrLength];
        m_CurInfoArrLength = 0;
        
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
        if (m_CurInfoArrLength >= m_RO_ColInfoArrLength)
            return;

        AddElement(col);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < m_CurInfoArrLength; i++)
        {
            if (m_AimedColInfoArr[i] == other)
            {
                DeleteElementUsingIdx(i);
                break;
            }
        }
    }

    
    // Functions
    public AimedColInfo GetAimedColInfo()
    {
        switch (m_CurInfoArrLength)
        {
            case <= 0:
                return null;
                break;
            
            case 1:
                return new AimedColInfo(m_AimedColInfoArr[0], transform.position);
                break;
            
            default:
                Vector2 aimCursorPos = transform.position;
                
                int minIdx = 0;
                float minDistance = (aimCursorPos - (Vector2)m_AimedColInfoArr[0].transform.position).sqrMagnitude;
                float distance = 0f;
                
                for (int i = 1; i < m_CurInfoArrLength; i++)
                {
                    distance = (aimCursorPos - (Vector2)m_AimedColInfoArr[i].transform.position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minIdx = i;
                    }
                }

                return new AimedColInfo(m_AimedColInfoArr[minIdx], aimCursorPos);
                break;
        }
    }
    private void DeleteElementUsingIdx(int _idx)
    {
        // Idx Out of range
        if (_idx < 0 || _idx >= m_CurInfoArrLength)
            return;

        // ������ ��� ���Ű� �ƴ� ��,������ ��� ������ �� ��Ҹ� ���ư��� ���ο��� ����
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
        // Player�� Hotbox Collider ����
        if (collision.CompareTag("Player"))
            return;
        
        // �浹�� ��Ʈ�ڽ��� ������Ʈ ����(InstanceID, Position)�� ����Ʈ�� ����
        m_AimedObjs.Add(new AimedObjInfo(collision, collision.GetInstanceID(), collision.transform.position));
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Exit �� Exit�� ��Ʈ�ڽ��� ����Ʈ���� ����
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

            // ���� LIst ������� ������ ������Ʈ Id�� -1�� ���Ҵ�
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
            // Ray�� �ƹ��͵� ���� ����(�� ����)
            return new HitscanTargetInfo(0, transform.position);
        }
        else
        {
            // Ray�� ���� �����
            int coinsideIdx = -1;
            
            // Aimcursor�� �浹�� ����� Ray�� ���� ����� ������ ��
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
                // �� ����
                return new HitscanTargetInfo(1, m_PlayerHitscanRay.GetPosWhenAimFailed());
            }
            else
            {
                // ����
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
     */
}
