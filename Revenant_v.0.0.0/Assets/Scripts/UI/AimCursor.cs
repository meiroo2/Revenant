using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AimedObjInfo
{
    public Collider2D m_Collider2d { get; private set; }
    public int m_ColliderInstanceID { get; private set; }
    public Vector2 m_ColliderPos { get; private set; }


    public AimedObjInfo(Collider2D _collider, int _id, Vector2 _pos)
    {
        m_Collider2d = _collider;
        m_ColliderInstanceID = _id;
        m_ColliderPos = _pos;
    }
}

public class HitscanTargetInfo
{
    public IHotBox m_HotBox { get; private set; }
    public Vector2 m_AimedPos { get; private set; }

    public HitscanTargetInfo(IHotBox _hotBox, Vector2 _aimPos)
    {
        m_HotBox = _hotBox;
        m_AimedPos = _aimPos;
    }
}

public class AimCursor : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public float p_FireMinimumDistance { get; private set; } = 0.2f;
    [field: SerializeField] public float m_Dist_Aim_Player { get; private set; } = 0f;



    // Member Variables
    private Camera m_MainCamera;
    private AimImageCanvas m_ImageofAim;
    private Transform m_PlayerTransform;
    private Player_AniMgr m_PlayerAniMgr;
    private Player_HitscanRay m_PlayerHitscanRay;

    public bool m_canAimCursorShot { get; private set; } = true;
    public int AimedObjid { get; private set; } = -1;
    public string AimedObjName { get; private set; } = "";

    private List<AimedObjInfo> m_AimedObjs = new List<AimedObjInfo>();
    private Vector2 m_CursorPos;
    private int m_ShortestIdx = -1;
    private float m_ShortestLength = 0f;

    private float m_TempLength;



    // Constructors
    private void Awake()
    {
        m_MainCamera = Camera.main;
    }
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        
        m_ImageofAim = instance.m_MainCanvas.GetComponentInChildren<AimImageCanvas>();
        m_PlayerTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.transform;
        m_PlayerAniMgr = instance.GetComponentInChildren<Player_Manager>().m_Player.m_PlayerAniMgr;
        m_PlayerHitscanRay = instance.GetComponentInChildren<Player_Manager>().m_Player.m_PlayerHitscanRay;
    }



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
    public HitscanTargetInfo GetHitscanTargetInfo()
    {
        if (m_AimedObjs.Count > 0)
        {
            int isFound = -1;
            List<RaycastHit2D> arr = m_PlayerHitscanRay.GetRayHits();

            if (arr.Count <= 0)
            {
                return new HitscanTargetInfo(null, m_PlayerHitscanRay.GetAimFailedHit().point);
            }

            for (int i = 0; i < m_AimedObjs.Count; i++)
            {
                for (int j = 0; j < arr.Count; j++)
                {
                    if (m_AimedObjs[i].m_Collider2d == arr[j].collider)
                    {
                        isFound = i;
                        break;
                    }
                }

                if (isFound != -1)
                    break;
            }

            if (isFound == -1)
                return new HitscanTargetInfo(null, m_PlayerHitscanRay.GetAimFailedHit().point);
            else
            {
                return new HitscanTargetInfo(m_AimedObjs[isFound].m_Collider2d.GetComponent<IHotBox>(),
                    transform.position);
            }
        }
        else
        {
            return new HitscanTargetInfo(null, m_PlayerHitscanRay.GetAimFailedHit().point);
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
}
