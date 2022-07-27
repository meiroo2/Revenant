using System;
using UnityEngine;


public class BoomBox_HotBox : MonoBehaviour, IHotBox
{
    private BoomBox m_BoomBox;
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
 
    
    // Constructors
    private void Awake()
    {
        m_BoomBox = GetComponentInParent<BoomBox>();
        m_ParentObj = m_BoomBox.gameObject;
    }
    
    
    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_BoomBox.GetHit(_param.m_Damage);
        return 1;
    }
}