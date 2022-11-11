using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class BossGang_HoloWeapon : BasicWeapon_Enemy
{
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_HotBoxDic = new Dictionary<Collider2D, IHotBox>();
    

    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BossGang>().GetComponent<BasicEnemy>();
    }

    private void OnEnable()
    {
        m_HotBoxDic.Clear();
        m_HotBoxDic.TrimExcess();
    }

    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_isEnemys)
                return;

            m_HotBoxDic.Add(col, hotBox);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            if (m_HotBoxDic.ContainsKey(other))
            {
                m_HotBoxDic.Remove(other);
            }
        }
    }


    public override int Fire()
    {
        IHotBoxParam param = new IHotBoxParam(p_BulletDamage, 0,
            transform.position, WeaponType.KNIFE);
        var hotBoxArr = m_HotBoxDic.Values.ToArray();
        
        for (int i = 0; i < hotBoxArr.Length; i++)
        {
            param.m_contactPoint = hotBoxArr[i].m_ParentObj.transform.position;
            hotBoxArr[i].HitHotBox(param);
        }
        
        return 1;
    }
}