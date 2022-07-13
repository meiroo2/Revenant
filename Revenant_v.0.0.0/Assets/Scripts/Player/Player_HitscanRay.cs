using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Constructors
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
    }


    // Functions
    private void AimRaycast()
    {
        Vector2 rayDirection = m_Player.m_IsRightHeaded ? transform.right : transform.right * -1;
        
        m_RayStartPos = transform.position;
        m_RayStartPos += rayDirection * 0.3f;
        
        

        m_HitCount = Physics2D.RaycastNonAlloc(m_RayStartPos, rayDirection, m_AimRayHits, p_RayLength,
            LayerMask.GetMask("HotBoxes"));
    }

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
    public RaycastHit2D GetClosestHit()
    {
        AimRaycast();
        
        if (m_HitCount <= 0)
            return m_NullRayHit;
        else
        {
            RaycastHit2D closestHit = new RaycastHit2D();
            float closestDistance = 9999999f;
            
            for (int i = 0; i < m_HitCount; i++)
            {
                float distance = (m_AimRayHits[i].collider.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestHit = m_AimRayHits[i];
                    closestDistance = distance;
                }
            }

            return closestHit;
        }
    }

    public RaycastHit2D GetAimFailedHit()
    {
        float multiplier = 5f;
        Vector2 rayDirection = m_Player.m_IsRightHeaded ? transform.right : transform.right * -1;
        
        m_RayStartPos = transform.position;
        m_RayStartPos += rayDirection * 0.3f;

        int layermask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Object"));
        
        m_AimRayHit = Physics2D.Raycast(m_RayStartPos, rayDirection, multiplier * 5f,
            layermask);

        return m_AimRayHit;
    }
}