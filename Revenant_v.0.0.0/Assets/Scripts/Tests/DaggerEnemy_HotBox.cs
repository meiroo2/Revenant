using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEnemy_HotBox : MonoBehaviour, IHotBox
{
    public bool p_isHead = false;
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.BODY;
    public GameObject m_ParentObj { get; set; }

    private DaggerEnemy m_Enemy;

    private void Awake()
    {
        m_ParentObj = GetComponentInParent<Human>().gameObject;
        m_Enemy = GetComponentInParent<DaggerEnemy>();
    }

    // 0 = No Hit, 1 = Hit
    public int HitHotBox(IHotBoxParam _param)
    {
        if (p_isHead)
            m_Enemy.GetHit(_param.m_Damage * 2);
        else
            m_Enemy.GetHit(_param.m_Damage);

        return 1;
    }
}
