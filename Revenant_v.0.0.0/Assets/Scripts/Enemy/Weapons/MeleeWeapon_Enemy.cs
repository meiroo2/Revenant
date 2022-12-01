using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeWeapon_Enemy : BasicWeapon_Enemy
{
    // Visible Member Variables

    
    // Member Variables
    private List<Collider2D> m_HotBoxColliderList = new List<Collider2D>();
    

    private void Awake()
    {
        m_isPlayers = false;
    }
    

    public override int Fire()
    {
        if (m_HotBoxColliderList.Count <= 0)
            return 0;
        
        float minDist = 999999f;
        int minIdx = 0;
        for (int i = 0; i < m_HotBoxColliderList.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, m_HotBoxColliderList[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                minIdx = i;
            }
        }
        
        if (m_HotBoxColliderList[minIdx].TryGetComponent(out IHotBox hotBox))
        {
            hotBox.HitHotBox(new IHotBoxParam(10, 0, transform.position,
                WeaponType.KNIFE));
        }
        
        m_Callback?.Invoke();
        
        return 1;
    }

    public override void Reload()
    {
        //return 0;
    }

    public override void InitWeapon()
    {
        
    }

    public override void ExitWeapon()
    {
        
    }
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_isEnemys == false && hotBox.m_HitBoxInfo == HitBoxPoint.BODY)
            {
                m_HotBoxColliderList.Add(col);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_isEnemys == false && hotBox.m_HitBoxInfo == HitBoxPoint.BODY)
            {
                m_HotBoxColliderList.Remove(other);
            }
        }
    }
}