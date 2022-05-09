using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AimedObjInfo
{
    public int m_ObjID;
    public Vector2 m_ObjPos;
    public AimedObjInfo(int _objid, Vector2 _objpos)
    {
        m_ObjID = _objid;
        m_ObjPos = _objpos;
    }
}

public class AimCursor : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public float p_FireMinimumDistance { get; private set; } = 0.2f;

    // Member Variables
    [field: SerializeField] public float m_Dist_Aim_Player { get; private set; } = 0f;

    public bool m_canAimCursorShot { get; private set; } = true;
    public int AimedObjid { get; private set; } = -1;
    private Collider2D m_AimedCollider;
    private Vector2 m_CursorPos;

    private List<AimedObjInfo> m_AimedObjs = new List<AimedObjInfo>();

    private int m_ShortestId = 0;
    private float m_ShortestLength = 0f;

    private Camera m_MainCamera;
    private AimImageCanvas m_ImageofAim;
    private Transform m_PlayerTransform;
    private Player_AniMgr m_PlayerAniMgr;

    // Constructors
    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_ImageofAim = GameObject.FindGameObjectWithTag("AimImage").GetComponent<AimImageCanvas>();
    }
    private void Start()
    {
        m_PlayerTransform = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
        m_PlayerAniMgr = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_PlayerAniMgr;
    }

    // Updates
    private void Update()
    {
        m_Dist_Aim_Player = Vector2.SqrMagnitude(transform.position - m_PlayerTransform.position);

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
        // 충돌한 히트박스의 정보를 리스트에 담음
        m_AimedObjs.Add(new AimedObjInfo(collision.gameObject.GetInstanceID(), collision.transform.position));
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 실시간으로 가장 가까운 히트박스를 계산
        if(m_AimedObjs.Count > 0)
        {
            m_ShortestId = m_AimedObjs[0].m_ObjID;
            m_ShortestLength = ((Vector2)transform.position - m_AimedObjs[0].m_ObjPos).sqrMagnitude;

            for(int i = 1; i < m_AimedObjs.Count; i++)
            {
                if(m_ShortestLength > ((Vector2)transform.position - m_AimedObjs[i].m_ObjPos).sqrMagnitude)
                {
                    m_ShortestLength = ((Vector2)transform.position - m_AimedObjs[i].m_ObjPos).sqrMagnitude;
                    m_ShortestId = m_AimedObjs[i].m_ObjID;
                }
            }

            AimedObjid = m_ShortestId;
        }
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

            if (m_AimedObjs.Count == 0)
                AimedObjid = -1;
        }
    }

    // Functions
}
