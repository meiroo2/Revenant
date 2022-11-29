using System;
using System.Collections;
using UnityEngine;


public class Subway_SubwayMove : MonoBehaviour
{
    public Transform EndTransform;
    public float BrakingTime = 3.0f;

    private Vector2 vel = Vector2.zero;

    private Coroutine m_MoveCoroutine = null;
    
    public void MoveToTargetTransform()
    {
        if (!ReferenceEquals(m_MoveCoroutine, null))
        {
            StopCoroutine(m_MoveCoroutine);
        }
        
        m_MoveCoroutine = StartCoroutine(MoveEnumerator());
    }

    private IEnumerator MoveEnumerator()
    {
        while (true)
        {
            transform.position = Vector2.SmoothDamp(transform.position, EndTransform.position, ref vel, BrakingTime);

            if(IsReachedTargetTransform())
            {
                break;
            }
            yield return null;
        }
    }

    /** 액션용 함수 */
    public void ArrivedTargetTransform()
    {
        Debug.Log("액션 부르기! ArrivedTargetTransform");

        if(IsReachedTargetTransform())
        {
            // To do
        }
    }

    private bool IsReachedTargetTransform()
    {
        if (Vector2.Distance(transform.position, EndTransform.position) <= 0.1f)
        {
            Debug.Log("TargetTransform에 도착");
            return true;
        }

        return false;
    }
}