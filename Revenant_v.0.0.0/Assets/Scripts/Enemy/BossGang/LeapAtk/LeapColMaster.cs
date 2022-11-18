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

    // 1 = Left, 2 = Right
    public int m_SelectedLeapColIdx = 0;
    
    private void Awake()
    {
        transform.parent = null;
        
        m_LeftLeapCol = p_LeftCol.GetComponent<LeapCol>();
        m_LeftLeapCol.m_IsLeftCol = true;
        m_LeftLeapCol.m_LeapColMaster = this;
        p_LeftCol.enabled = false;
        
        m_RightLeapCol = p_RightCol.GetComponent<LeapCol>();
        m_RightLeapCol.m_IsLeftCol = false;
        m_RightLeapCol.m_LeapColMaster = this;
        p_RightCol.enabled = false;
        
        p_ColMaster.SetActive(false);
    }

    
    public void SpawnCols(bool _isRightHead, Vector2 _spawnPos)
    {
        m_SelectedLeapColIdx = 0;
        Transform masterTransform = p_ColMaster.transform;
        masterTransform.gameObject.SetActive(true);
        
        p_LeftCol.gameObject.SetActive(true);
        p_LeftCol.gameObject.layer = 22;
        p_LeftCol.enabled = true;
        m_LeftLeapCol.ChangeColPhase(0);
        
        p_RightCol.gameObject.SetActive(true);
        p_RightCol.gameObject.layer = 22;
        p_RightCol.enabled = true;
        m_RightLeapCol.ChangeColPhase(0);
        
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
        m_LeftLeapCol.ChangeColPhase(-1);
        m_RightLeapCol.ChangeColPhase(-1);

        Vector2 LeftLandPos = p_ColMaster.transform.position;
        LeftLandPos.x -= p_LeftCol.size.x / 2f;

        Vector2 RightLandPos = p_ColMaster.transform.position;
        RightLandPos.x += p_RightCol.size.x / 2f;

        float distanceBetLeft = Vector2.Distance(_playerFootPos, LeftLandPos);
        float distanceBetRight = Vector2.Distance(_playerFootPos, RightLandPos);
        
        switch (m_IsLeftCollide)
        {
            case true when !m_IsRightCollide:
                m_SelectedLeapColIdx = 1;
                return LeftLandPos;
            
            case false when m_IsRightCollide:
                m_SelectedLeapColIdx = 2;
                return RightLandPos;
            
            default:
            {
                if (distanceBetLeft < distanceBetRight)
                {
                    m_SelectedLeapColIdx = 1;
                    return LeftLandPos;
                }
                else
                {
                    m_SelectedLeapColIdx = 2;
                    return RightLandPos;
                }
            }
        }
    }

    public void ConvertSelectedCol()
    {
        switch (m_SelectedLeapColIdx)
        {
            case 1:
                p_RightCol.enabled = false;
                m_LeftLeapCol.ClearList();
                p_LeftCol.gameObject.layer = 8;
                m_LeftLeapCol.ChangeColPhase(1);
                break;
            
            case 2:
                p_LeftCol.enabled = false;
                m_RightLeapCol.ClearList();
                p_RightCol.gameObject.layer = 8;
                m_RightLeapCol.ChangeColPhase(1);
                break;
        }
    }

    public void DoAttack()
    {
        switch (m_SelectedLeapColIdx)
        {
            case 1:
                m_LeftLeapCol.ChangeColPhase(-1);
                m_LeftLeapCol.Attack();
            break;
            
            case 2:
                m_RightLeapCol.ChangeColPhase(-1);
                m_RightLeapCol.Attack();
            break;
        }
    }

    public void ReleaseAll()
    {
        p_LeftCol.gameObject.SetActive(false);
        p_RightCol.gameObject.SetActive(false);
    }
}