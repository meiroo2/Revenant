using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BezierMove : MonoBehaviour
{
    // Visible Member Variables
    public float m_Speed = 1f;
    

    // Member Variables
    private Vector2 m_DestPoint;
    private Coroutine m_MoveCoroutine = null;
    private Action m_ExitAction;
    
    
    // Functions
    public void SetAction(Action _action)
    {
        m_ExitAction = _action;
    }

    public void ResetAction()
    {
        m_ExitAction = null;
    }
    

    // Bezier
    private Vector2[] p_BezierPositions = new Vector2[4];
    private float m_BezierTime = 0f;    
    public float posA = 0.85f;
    public float posB = 0.75f;
    
    private void Start()
    {
        m_DestPoint = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().GetPlayerFootPos();
    }

    [Button]
    public void MoveToPoint()
    {
        if(!ReferenceEquals(m_MoveCoroutine, null))
            StopCoroutine(m_MoveCoroutine);
        
        m_DestPoint = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().GetPlayerFootPos();

        m_BezierTime = 0f;
        p_BezierPositions[0] = transform.position;
        p_BezierPositions[1] = SetBezierPoint(transform.position);
        p_BezierPositions[2] = SetBezierPoint(m_DestPoint);
        p_BezierPositions[3] = m_DestPoint;
        
        
        m_MoveCoroutine = StartCoroutine(Moving());
    }
    
    private Vector2 SetBezierPoint(Vector2 origin)
    {
        return new Vector2(posA + origin.x, posB + origin.y);
    }
    
    private Vector2 GetNewBezierPos()
    {
        return new Vector2(
            FourPointBezier(p_BezierPositions[0].x, p_BezierPositions[1].x, p_BezierPositions[2].x,
                p_BezierPositions[3].x),
            FourPointBezier(p_BezierPositions[0].y, p_BezierPositions[1].y, p_BezierPositions[2].y,
                p_BezierPositions[3].y)
        );
    }
    
    private float FourPointBezier(float _a, float _b, float _c, float _d) {
        return Mathf.Pow((1 - m_BezierTime), 3) * _a
               + Mathf.Pow((1 - m_BezierTime), 2) * 3 * m_BezierTime * _b
               + Mathf.Pow(m_BezierTime, 2) * 3 * (1 - m_BezierTime) * _c
               + Mathf.Pow(m_BezierTime, 3) * _d;
    }
    
    private IEnumerator Moving()
    {
        float timer = 0f;
        bool escape = false;
        
        while (true)
        {
            timer += Time.deltaTime;

            switch (m_BezierTime)
            {
                case < 0.4f:
                    transform.position = GetNewBezierPos();
                    m_BezierTime += timer * (m_Speed * 0.7f);
                    break;
                
                case < 0.55f:
                    transform.position = GetNewBezierPos();
                    m_BezierTime += timer * (m_Speed * 0.3f);
                    break;
                
                case < 1f:
                    transform.position = GetNewBezierPos();
                    m_BezierTime += timer * m_Speed;
                    break;
                
                case >= 1f:
                    escape = true;
                    break;
            }

            if (escape)
            {
                m_BezierTime = 1f;
                transform.position = GetNewBezierPos();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    
        m_ExitAction?.Invoke();
        yield break;
    }
}