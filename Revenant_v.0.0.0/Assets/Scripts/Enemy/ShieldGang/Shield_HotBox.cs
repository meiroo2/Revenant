using System;
using UnityEngine;


public class Shield_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    private Shield m_Shield;
    
    
    // Constructors
    private void Awake()
    {
        m_Shield = GetComponentInParent<Shield>();
        m_ParentObj = m_Shield.gameObject;
    }
    
    
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
    public int HitHotBox(IHotBoxParam _param)
    {
        return 1;
    }
}