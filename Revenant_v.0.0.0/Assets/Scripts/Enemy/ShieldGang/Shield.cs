using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public int p_ShieldHp = 30;
    
    public Transform p_OtherSidePos;
    public float p_MoveTime = 1f;
    private Vector2 p_OriginPos;
    private bool m_IsLeft = false;
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;


    // Constructors
    private void Awake()
    {
        p_OriginPos = transform.position;
        m_ParentObj = this.gameObject;
        
        //StartCoroutine(MoveToPosition(transform, p_OtherSidePos.position, p_MoveTime));
    }


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        throw new System.NotImplementedException();
    }
    
    public IEnumerator MoveToPosition(Transform transform, Vector2 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector2.Lerp(currentPos, position, t);
            yield return null;
        }

        if (m_IsLeft)
        {
            m_IsLeft = false;
            StartCoroutine(MoveToPosition(transform, p_OtherSidePos.position, p_MoveTime));
        }
        else
        {
            m_IsLeft = true;
            StartCoroutine(MoveToPosition(transform, p_OriginPos, p_MoveTime));
        }
    }
}