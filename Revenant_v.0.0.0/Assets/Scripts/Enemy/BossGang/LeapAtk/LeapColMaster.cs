using System;
using UnityEngine;


public class LeapColMaster : MonoBehaviour
{
    public GameObject p_ColMaster;
    public BoxCollider2D p_LeftCol;
    public BoxCollider2D p_RightCol;

    public Transform p_LeftLimit;
    public Transform p_RightLimit;

    public bool m_IsLeftCollide = false;
    public bool m_IsRightCollide = false;

    private LeapCol m_LeftLeapCol;
    private LeapCol m_RightLeapCol;
    
    private Vector2 m_LeftEdgePos;
    private Vector2 m_RightEdgePos;
    
    private void Awake()
    {
        m_LeftLeapCol = p_LeftCol.GetComponent<LeapCol>();
        m_LeftLeapCol.m_IsLeftCol = true;
        m_LeftLeapCol.m_LeapColMaster = this;
        m_LeftLeapCol.m_Phase = 0;
        p_LeftCol.enabled = false;
        
        m_RightLeapCol = p_RightCol.GetComponent<LeapCol>();
        m_RightLeapCol.m_IsLeftCol = false;
        m_RightLeapCol.m_LeapColMaster = this;
        m_RightLeapCol.m_Phase = 0;
        p_RightCol.enabled = false;
        
        p_ColMaster.SetActive(false);
    }

    
    public void SpawnCols(bool _isRightHead, Vector2 _spawnPos)
    {
        Transform masterTransform = p_ColMaster.transform;
        masterTransform.gameObject.SetActive(true);
        
        p_LeftCol.enabled = true;
        p_LeftCol.gameObject.layer = 22;
        m_LeftLeapCol.m_Phase = 0;
        
        p_RightCol.enabled = true;
        p_RightCol.gameObject.layer = 22;
        m_RightLeapCol.m_Phase = 0;
        
        m_IsLeftCollide = false;
        m_IsRightCollide = false;

        Vector2 spawnPos = _spawnPos;

        m_LeftEdgePos = spawnPos;
        m_LeftEdgePos.x -= p_LeftCol.size.x;
        
        m_RightEdgePos = spawnPos;
        m_RightEdgePos.x += p_RightCol.size.x;
        
        
        if (_isRightHead)
        {
            // 우측 방향에 생성
            if (m_RightEdgePos.x > p_RightLimit.position.x)
            {
                spawnPos.x = p_RightLimit.position.x - p_RightCol.size.x;
            }
        }
        else
        {
            // 좌측 방향에 생성
            if (m_LeftEdgePos.x < p_LeftLimit.position.x)
            {
                spawnPos.x = p_LeftLimit.position.x + p_LeftCol.size.x;
            }
        }
        
        masterTransform.position = spawnPos;
    }

    public Vector2 GetLandingPos(Vector2 _playerFootPos)
    {
        m_LeftLeapCol.m_Phase = 1;
        m_RightLeapCol.m_Phase = 1;

        Vector2 LeftLandPos = p_ColMaster.transform.position;
        LeftLandPos.x -= p_LeftCol.size.x / 2f;

        Vector2 RightLandPos = p_ColMaster.transform.position;
        RightLandPos.x += p_RightCol.size.x / 2f;

        if (m_IsLeftCollide && m_IsRightCollide)
        {
            if (Vector2.Distance(_playerFootPos, LeftLandPos) <
                Vector2.Distance(_playerFootPos, RightLandPos))
            {
                return LeftLandPos;
            }
            else
            {
                return RightLandPos;
            }
        }
        else if (m_IsLeftCollide && !m_IsRightCollide)
            return LeftLandPos;
        else if (!m_IsLeftCollide && m_IsRightCollide)
            return RightLandPos;
        else
        {
            if (Vector2.Distance(_playerFootPos, LeftLandPos) <
                Vector2.Distance(_playerFootPos, RightLandPos))
            {
                return LeftLandPos;
            }
            else
            {
                return RightLandPos;
            }
        }
    }
}