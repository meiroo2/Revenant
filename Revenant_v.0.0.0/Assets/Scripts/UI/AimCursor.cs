using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AimedObjInfo
{
    public string m_ObjName;
    public int m_ObjID;
    public Vector2 m_ObjPos;
    public AimedObjInfo(string _objName, int _objid, Vector2 _objpos)
    {
        m_ObjName = _objName;
        m_ObjID = _objid;
        m_ObjPos = _objpos;
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
        m_ImageofAim = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<AimImageCanvas>();
        m_PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
        m_PlayerAniMgr = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_PlayerAniMgr;
    }



    // Updates
    private void Update()
    {
        m_Dist_Aim_Player = Vector2.SqrMagnitude(transform.position - m_PlayerTransform.position);

        CalculateNearestHotBox();

        if (m_canAimCursorShot && m_Dist_Aim_Player < p_FireMinimumDistance)
        {
            m_canAimCursorShot = false;
            m_PlayerAniMgr.changeArm(1);
            m_ImageofAim.changeAimImage(1);
        }
        else if(!m_canAimCursorShot && m_Dist_Aim_Player > p_FireMinimumDistance)
        {
            m_canAimCursorShot = true;
            m_PlayerAniMgr.changeArm(0);
            m_ImageofAim.changeAimImage(0);
        }
        
    }
    private void FixedUpdate()
    {
        m_CursorPos = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = m_CursorPos;
    }

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 히트박스의 오브젝트 정보(InstanceID, Position)를 리스트에 담음
        m_AimedObjs.Add(new AimedObjInfo(collision.gameObject.name, collision.gameObject.GetInstanceID(), collision.transform.position));
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Exit 시 Exit한 히트박스는 리스트에서 제거
        if(m_AimedObjs.Count > 0)
        {
            for(int i = 0; i < m_AimedObjs.Count; i++)
            {
                if (m_AimedObjs[i].m_ObjID == collision.gameObject.GetInstanceID())
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
    private void CalculateNearestHotBox()
    {
        if (m_AimedObjs.Count > 0)
        {
            m_ShortestLength = Vector2.Distance(transform.position, m_AimedObjs[0].m_ObjPos);
            m_ShortestIdx = 0;

            for (int i = 0; i < m_AimedObjs.Count; i++)
            {
                m_TempLength = Vector2.Distance(transform.position, m_AimedObjs[i].m_ObjPos);
                if (m_TempLength < m_ShortestLength)
                {
                    m_ShortestLength = m_TempLength;
                    m_ShortestIdx = i;
                }
            }

            AimedObjid = m_AimedObjs[m_ShortestIdx].m_ObjID;
            AimedObjName = m_AimedObjs[m_ShortestIdx].m_ObjName;
        }
    }
}
