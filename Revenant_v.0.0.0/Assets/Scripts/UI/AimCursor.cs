using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    // Visible Member Variables

    // Member Variables
    public int AimedObjid { get; private set; } = -1;
    private Collider2D m_AimedCollider;
    private Vector2 m_CursorPos;

    private List<int> m_ObjIds = new List<int>();
    private List<Vector2> m_ObjPoses = new List<Vector2>();

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
        m_ObjIds.Add(collision.gameObject.GetInstanceID());
        m_ObjPoses.Add(collision.transform.position);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(m_ObjPoses.Count > 0)
        {
            m_ShortestId = m_ObjIds[0];
            m_ShortestLength = Vector2.Distance(m_ObjPoses[0], transform.position);

            if(m_ObjPoses.Count > 1)
            {
                for (int i = 1; i < m_ObjPoses.Count; i++)
                {
                    if (m_ShortestLength > Vector2.Distance(m_ObjPoses[i], transform.position))
                    {
                        m_ShortestLength = Vector2.Distance(m_ObjPoses[i], transform.position);
                        m_ShortestId = m_ObjIds[i];
                    }
                }
            }

            AimedObjid = m_ShortestId;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(m_ObjIds.Count > 0)
        {
            for(int i = 0; i < m_ObjIds.Count; i++)
            {
                if (m_ObjIds[i] == collision.GetInstanceID())
                {
                    int tempidx = m_ObjIds.IndexOf(m_ObjIds[i]);
                    m_ObjIds.RemoveAt(tempidx);
                    m_ObjPoses.RemoveAt(tempidx);
                    break;
                }
            }

            if (m_ObjIds.Count == 0)
                AimedObjid = 0;
        }
    }

    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
