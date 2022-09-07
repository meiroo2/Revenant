using System;
using System.Collections.Generic;
using UnityEngine;


public class ShieldGang_Baton : BasicWeapon_Enemy
{
    // Member Variables
    private List<IHotBox> m_HotBoxList;
    private Collider2D m_IHotBoxDetectCol;

    private bool m_IsAttacking = false;

    // Constructors
    private void Awake()
    {
        m_HotBoxList = new List<IHotBox>();

        if (TryGetComponent(out Collider2D col))
            m_IHotBoxDetectCol = col;
        else
            Debug.Log("ERR : " + gameObject.name + "에서 Collider2D를 찾지 못함.");
    }
    
    
    // BasicWeapon Functions
    public override int Fire()
    {
        m_IsAttacking = true;
        
        // 여기서 사운드 재생&이펙트 출력도 해야 합니다.
        foreach (var element in m_HotBoxList)
        {
            element.HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue, 
                element.m_ParentObj.transform.position, WeaponType.KNIFE));
        }

        m_IsAttacking = false;
        
        return 1;
    }
    public override void Reload()
    {

    }
    public override void InitWeapon()
    {
        
    }
    public override void ExitWeapon()
    {
        
    }
    
    
    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_IsAttacking)
            return;
        
        var hotBox = col.GetComponent<IHotBox>();
        if (hotBox.m_isEnemys == false)
        {
            m_HotBoxList.Add(hotBox);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_IsAttacking)
            return;
        
        var hotBox = other.GetComponent<IHotBox>();
        m_HotBoxList.Remove(hotBox);
    }
}