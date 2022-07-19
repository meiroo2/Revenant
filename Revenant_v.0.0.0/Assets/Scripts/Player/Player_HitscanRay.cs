using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HitscanResult
{
    // 0 == 빈 곳 조준함(Ray를 발사해도 아무것도 감지되지 않음), 1 == 조준 실패로 근처 벽이나 오브젝트 감지
    // 2 == 조준 성공
    public readonly int m_ResultCheckNum;
    public readonly IHotBox m_Hotbox;
    public readonly Vector2 m_RayPoint;

    public HitscanResult(int _checkNum, IHotBox _hotBox, Vector2 _rayPoint)
    {
        m_ResultCheckNum = _checkNum;
        m_Hotbox = _hotBox;
        m_RayPoint = _rayPoint;
    }
    public HitscanResult(int _checkNum, Vector2 _rayPoint)
    {
        m_ResultCheckNum = _checkNum;
        m_RayPoint = _rayPoint;
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
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_Cursor = instance.GetComponentInChildren<AimCursor>();
    }


    // Functions
    private Vector2 GetRayDirection()
    {
        return m_Player.m_IsRightHeaded ? transform.right : transform.right * -1;
    }

    private void UpdateRayStartPos()
    {
        m_RayStartPos = transform.position;
        m_RayStartPos += GetRayDirection() * 0.3f;
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
                return new HitscanResult(0, m_RayStartPos += GetRayDirection() * 5f);

            
            // 근처 벽이나 장애물로 검출해야 함
            UpdateNearObstacle();
            return new HitscanResult(1, m_AimRayHit.collider.GetComponent<IHotBox>(), m_AimRayHit.point);
        }
        else
        {
            // 어떠한 것을 조준한 상태   

            // 투과 Ray를 발사해서 조준한 Collider가 있는지 확인
            int foundIdx = 0;
            bool isFound = false;
            for (int i = 0; i < m_HitCount; i++)
            {
                if (m_AimRayHits[i] == colInfo.m_Collider)
                {
                    foundIdx = i;
                    isFound = true;
                    break;
                }
            }

            // 조준한 Collider가 Ray에 없을 경우 근처 장애물 Point를 반환
            if (isFound == false)
            {
                UpdateNearObstacle();
                return new HitscanResult(1, m_AimRayHit.collider.GetComponent<IHotBox>(), m_AimRayHit.point);
            }

            // 조준한 Collider가 Ray에 있는 상태
            // Ray보다 가까이에 장애물 존재할 경우
            if (IsThereAnyObstacle(m_AimRayHits[foundIdx]))
                return new HitscanResult(1, m_AimRayHit.collider.GetComponent<IHotBox>(), m_AimRayHit.point);
            else
                return new HitscanResult(1, m_AimRayHits[foundIdx].collider.GetComponent<IHotBox>(),
                    m_AimRayHits[foundIdx].point);
        }
    }
    private void AimRaycast()
    {
        UpdateRayStartPos();

        int layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object")) |
                        (1 << LayerMask.NameToLayer("HotBoxes"));
        
        m_HitCount = Physics2D.RaycastNonAlloc(m_RayStartPos, GetRayDirection(), m_AimRayHits, p_RayLength,
            layermask);
    }

    // 여기서는 또 벽을 검출 못함(HotBoxes만 검출)
    public List<RaycastHit2D> GetRayHits()
    {
        AimRaycast();

        if (m_HitCount <= 0)
            return null;
        else
        {
            m_ListForGetRayHits.Clear();
            for (int i = 0; i < m_HitCount; i++)
            {
                m_ListForGetRayHits.Add(m_AimRayHits[i]);
            }
            return m_ListForGetRayHits;
        }
    }
    
    // GetRayHits로 가져가서 다시 결과검출
    public bool IsThereAnyObstacle(RaycastHit2D _hit)
    {
        // 가장 가까운 장애물 검출
        UpdateNearObstacle();
        
        // 가장 가까운 장애물이 hit보다 더 가까이 있다면 true
        return (m_RayStartPos - m_AimRayHit.point).sqrMagnitude < (m_RayStartPos - _hit.point).sqrMagnitude;
    }

    // 벽에 맞았을 때, 빈 곳에 맞았을 때 구분 필요
    private void UpdateNearObstacle()
    {
        UpdateRayStartPos();
        int layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object"));
        m_AimRayHit = Physics2D.Raycast(m_RayStartPos, GetRayDirection(), p_RayLength, layermask);
    }

    // Ray 쏜 곳에 HotBox 제외하고 아무것도 없는지?
    private bool IsThereNoObstacle()
    {
        UpdateNearObstacle();
        return m_AimRayHit.collider == null;
    }
}