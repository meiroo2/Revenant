using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeWeapon_Enemy : BasicWeapon_Enemy
{
    // Visible Member Variables

    
    // Member Variables
    private Collider2D m_Collider;
    private IHotBox m_HotBox;

    private void Awake()
    {
        m_Collider = GetComponentInChildren<Collider2D>();
        
        m_isPlayers = false;
    }

    private void OnDisable()
    {
        m_HotBox = null;
    }

    public override int Fire()
    {
        if (ReferenceEquals(m_HotBox, null))
            return 0;
        
        m_HotBox.HitHotBox(new IHotBoxParam(p_BulletDamage, 0, m_HotBox.m_ParentObj.transform.position,
            WeaponType.KNIFE));
        
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
                m_HotBox = hotBox;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            if (m_HotBox == hotBox)
                m_HotBox = null;
        }
    }
}