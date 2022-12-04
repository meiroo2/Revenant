using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground_Move : MonoBehaviour
{
    // Empty Object 만들어서 도착 위치 지정할 Transform
    // 반드시 해당 Object를 인스펙터에 넣어야함 (Y 높이 '무조건' 동일하게 만들 것)
    [SerializeField] protected Transform EndTransform;
    protected float GetCurrentDistanceToEndTransform;   // 현재 남은 거리 구하기 
    
    private Coroutine m_MoveCoroutine = null;

    /** 오브젝트가 움직일 때만 Update가 돌아갑니다. */
    private void Update()
    {
        GetCurrentDistanceToEndTransform = Vector2.Distance(transform.position, EndTransform.position);
    }

    /**
     * EndTransform에 도착했을 때 사용
     * 혹은 원하는대로 커스텀해서 사용
     * 필요할 때 Override
     */
    protected virtual void ArrivedEndTransform() { }

    public void MoveToTargetTransform()
    {
        if (!ReferenceEquals(m_MoveCoroutine, null))
        {
            StopCoroutine(m_MoveCoroutine);
        }
        
        m_MoveCoroutine = StartCoroutine(MoveEnumerator());
    }

    protected virtual IEnumerator MoveEnumerator()
    {
        while (true)
        {
            Move();

            if(IsReachedEndTransform())
            {
                ArrivedEndTransform();
                break;
            }
            yield return null;
        }
    }

    /** EndTransform에 도착했는지 확인하는 함수 */
    protected bool IsReachedEndTransform()
    {
        if (GetCurrentDistanceToEndTransform <= 0.1f)
        {
            return true;
        }

        return false;
    }

    protected virtual void Move() { }
}
