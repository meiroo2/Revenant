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
    private int hitcount;


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

            int layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                            (1 << LayerMask.NameToLayer("HotBoxes"));
            Ray2D ray = new Ray2D(transform.position, Vector2.zero);
            hitcount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, m_Lists, 1f, layermask);

            Debug.DrawRay(transform.position, Vector3.forward * 30f, Color.blue);
            Debug.Log(hitcount.ToString());

            if (hitcount > 0)
            {
                for (int i = 0; i < hitcount; i++)
                {
                    Debug.Log(m_Lists[i].collider.name);
                }
            }


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
        /*
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
    */

        switch (hitcount)
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
                    float distance = (transform.position - m_Lists[0].collider.transform.position).sqrMagnitude;

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
