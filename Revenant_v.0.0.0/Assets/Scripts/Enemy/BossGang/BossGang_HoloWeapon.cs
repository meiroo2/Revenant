﻿using System;
using System.Collections.Generic;
using UnityEngine;




public class BossGang_HoloWeapon : BasicWeapon_Enemy
{
    // Member Variables
    private List<IHotBox> m_HotBoxList = new List<IHotBox>();

    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BossGang>().GetComponent<BasicEnemy>();
    }

    private void OnEnable()
    {
        m_HotBoxList.Clear();
        m_HotBoxList.TrimExcess();
    }

    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_isEnemys)
                return;
            
            m_HotBoxList.Add(hotBox);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            if (m_HotBoxList.Contains(hotBox))
                m_HotBoxList.Remove(hotBox);
        }
    }


    public override int Fire()
    {
        var hotBoxArr = m_HotBoxList.ToArray();
        IHotBoxParam param = new IHotBoxParam(p_BulletDamage, 0,
            transform.position, WeaponType.KNIFE);
        
        for (int i = 0; i < hotBoxArr.Length; i++)
        {
            hotBoxArr[i].HitHotBox(param);
        }
        
        return 1;
    }
}