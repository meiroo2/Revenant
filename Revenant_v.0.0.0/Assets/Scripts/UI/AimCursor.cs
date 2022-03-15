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

    // Member Variables
    public int AimedObjid { get; private set; } = -1;
    private Collider2D m_AimedCollider;
    private Vector2 m_CursorPos;

    private List<AimedObjInfo> m_AimedObjs = new List<AimedObjInfo>();

    private int m_ShortestId = 0;
    private float m_ShortestLength = 0f;

    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        m_CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = m_CursorPos;
    }
    private void FixedUpdate()
    {

    }

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_AimedObjs.Add(new AimedObjInfo(collision.gameObject.GetInstanceID(), collision.transform.position));
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
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


    // 기타 분류하고 싶은 것이 있을 경우
}
