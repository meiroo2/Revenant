using System;
using System.Collections.Generic;
using UnityEngine;


public class ShieldGang_Baton : BasicWeapon_Enemy
{
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_HotBoxDic = new Dictionary<Collider2D, IHotBox>();

    private List<IHotBox> m_HotBoxList = new List<IHotBox>();
    
    private Collider2D m_IHotBoxDetectCol;

    private bool m_DicLocker = false;
    
    // Constructors
    private void Awake()
    {
        if (TryGetComponent(out Collider2D col))
            m_IHotBoxDetectCol = col;
        else
            Debug.Log("ERR : " + gameObject.name + "에서 Collider2D를 찾지 못함.");
    }
    
    
    // BasicWeapon Functions
    public override int Fire()
    {
        for (int i = 0; i < m_HotBoxList.Count; i++)
        {
            m_HotBoxList[i].HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue,
                m_HotBoxList[i].m_ParentObj.transform.position, WeaponType.AXE));
        }
        
        /*
        m_DicLocker = true;
        
        // 여기서 사운드 재생&이펙트 출력도 해야 합니다.
        foreach (var VARIABLE in m_HotBoxDic)
        {
            VARIABLE.Value.HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue,
                VARIABLE.Value.m_ParentObj.transform.position, WeaponType.KNIFE));
        }
        m_HotBoxDic.Clear();

        m_DicLocker = false;
        */
        
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
        if (col.TryGetComponent(out IHotBox box))
        {
            m_HotBoxList.Add(box);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox box))
        {
            if (m_HotBoxList.Contains(box))
                m_HotBoxList.Remove(box);
        }
    }
}