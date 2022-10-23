using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HitscanResult
{
    // 0 == 근처 장애물 감지 불가, 1 == 근처 벽이나 오브젝트 감지
    // 2 == 조준 성공(HitHotBox)
    public readonly int m_ResultCheckNum;
    public readonly Vector2 m_RayStartPoint;
    public readonly RaycastHit2D m_RayHitPoint;
    public readonly Vector2 m_RayDestinationPos;

    public HitscanResult(int _checkNum, Vector2 _rayStart, RaycastHit2D _hitPoint, Vector2 _desPos)
    {
        m_ResultCheckNum = _checkNum;
        m_RayStartPoint = _rayStart;
        m_RayHitPoint = _hitPoint;
        m_RayDestinationPos = _desPos;
    }
    public HitscanResult(int _checkNum, Vector2 _rayStart, Vector2 _desPos)
    {
        m_ResultCheckNum = _checkNum;
        m_RayStartPoint = _rayStart;
        m_RayDestinationPos = _desPos;
    }
}

public class Player_HitscanRay : MonoBehaviour
{
    // Visible Member Variables
    public float p_RayLength = 5f;
    
    // Member Variables
    private int m_HitCount = 0;
    private Vector2 m_RayStartPos;
    private RaycastHit2D[] m_AimRayHits = new RaycastHit2D[10];
    private RaycastHit2D m_AimRayHit;

    private List<RaycastHit2D> m_ListForGetRayHits = new List<RaycastHit2D>();
    private readonly RaycastHit2D m_NullRayHit;
    private Player m_Player;
    private AimCursor m_Cursor;
    
    // Constructors
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_Cursor = instance.GetComponentInChildren<AimCursor>();
    }


    // Functions
    private Vector2 GetRayDirection()
    {
        // 불릿타임 작동 중에는 전방위 레이 검출
        Vector2 rayDirection;
        if (m_Player.m_BulletTimeMgr.m_IsBulletTimeActivating)
        {
            rayDirection = ((Vector2)m_Cursor.transform.position - (Vector2)m_RayStartPos).normalized;
        }
        else
        {
            rayDirection = m_Player.m_IsRightHeaded ? transform.right : transform.right * -1;
        }

        return rayDirection;
    }

    private void UpdateRayStartPos()
    {
        m_RayStartPos = transform.position;
        m_RayStartPos += GetRayDirection() * 0.5f;
    }

    public HitscanResult GetHitscanResult()
    {
        AimedColInfo colInfo = m_Cursor.GetAimedColInfo();
        AimRaycast();
        

        // 아무것도 조준하지 않았을 경우(AimCursor가 null)
        if (ReferenceEquals(colInfo, null))
        {
            // 아무것도 검출되지 않음(멀리 있는 좌표 반환)
            if (m_HitCount <= 0 || IsThereNoObstacle() == true)
            {
                
                Debug.Log("아무것도 조준 X, 검출 X");
                return new HitscanResult(0, m_RayStartPos, m_RayStartPos += GetRayDirection() * 5f);
            }


            // 근처 벽이나 장애물로 검출해야 함
            UpdateNearObstacle();
            Debug.Log("아무것도 조준 X, 벽 검출 O");
            Debug.Log(m_AimRayHit.collider.gameObject);
            return new HitscanResult(1, m_RayStartPos, m_AimRayHit, m_AimRayHit.point);
        }
        else
        {
            // 어떠한 것을 조준한 상태   

            // 투과 Ray를 발사해서 조준한 Collider가 있는지 확인
            int foundIdx = 0;
            bool isFound = false;
            for (int i = 0; i < m_HitCount; i++)
            {
                if (m_AimRayHits[i].collider == colInfo.m_Collider)
                {
                    foundIdx = i;
                    isFound = true;
                    break;
                }
            }

            // 조준한 Collider가 Ray에 없을 경우 근처 장애물 Point를 반환
            if (isFound == false)
            {
                Debug.Log("조준한 대상이 Ray에 없음");
                UpdateNearObstacle();
                if (ReferenceEquals(m_AimRayHit.collider, null))
                {
                    return new HitscanResult(0, m_RayStartPos,
                        m_AimRayHit, m_RayStartPos += GetRayDirection() * 5f);
                }
                else
                {
                    return new HitscanResult(1, m_RayStartPos,
                        m_AimRayHit, m_AimRayHit.point);
                }
            }

            // 조준한 Collider가 Ray에 있는 상태
            // Ray보다 가까이에 장애물 존재할 경우
            if (IsThereAnyObstacle(m_AimRayHits[foundIdx]))
            {
                Debug.Log("조준한 대상이 있으나, 더 가까운 곳에 장애물이 존재");
                return new HitscanResult(1, m_RayStartPos, m_AimRayHit, m_AimRayHit.point);
            }
            else
            {
                Debug.Log("조준한 대상이 있고, 장애물도 없어서 명중");
                return new HitscanResult(2, m_RayStartPos, m_AimRayHits[foundIdx], colInfo.m_AimCursorPos);
            }
        }
    }

    private void AimRaycast()
    {
        UpdateRayStartPos();
        int layermask = 0;

        switch (m_Player.gameObject.layer)
        {
            case 10:
                layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                            (1 << LayerMask.NameToLayer("HotBoxes")) | (1 << LayerMask.NameToLayer("Stair"));
                break;
            
            case 12:
                layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                            (1 << LayerMask.NameToLayer("HotBoxes")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
                break;
        }
        

        m_HitCount = Physics2D.CircleCastNonAlloc(m_RayStartPos, 0.5f, GetRayDirection(), m_AimRayHits,
            p_RayLength, layermask);
    }

    // GetRayHits로 가져가서 다시 결과검출
    public bool IsThereAnyObstacle(RaycastHit2D _hit)
    {
        // 가장 가까운 장애물 검출
        UpdateNearObstacle();

        // 가장 가까운 장애물이 hit보다 더 가까이 있다면 true
        if (ReferenceEquals(m_AimRayHit.collider, null))
            return false;
        
        return (m_RayStartPos - m_AimRayHit.point).sqrMagnitude < (m_RayStartPos - _hit.point).sqrMagnitude;
    }

    // 벽에 맞았을 때, 빈 곳에 맞았을 때 구분 필요
    private void UpdateNearObstacle()
    {
        UpdateRayStartPos();
        int layermask = 0;

        switch (m_Player.gameObject.layer)
        {
            case 10:
                layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                            (1 << LayerMask.NameToLayer("Stair"));
                break;
            
            case 12:
                layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                            (1 << LayerMask.NameToLayer("EmptyFloor"));
                break;
        }
        
        m_AimRayHit = Physics2D.Raycast(m_RayStartPos, GetRayDirection(), p_RayLength, layermask);
    }

    // Ray 쏜 곳에 HotBox 제외하고 아무것도 없는지?
    private bool IsThereNoObstacle()
    {
        UpdateNearObstacle();
        return ReferenceEquals(m_AimRayHit.collider, null);
    }
}