using System;
using UnityEngine;



public class Barrel_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    private Barrel m_ParentBarrel;
    
    // Constructors
    private void Awake()
    {
        m_ParentBarrel = GetComponentInParent<Barrel>();
    }

    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; }
    public bool m_isEnemys { get; set; }
    public HitBoxPoint m_HitBoxInfo { get; set; }
    public int HitHotBox(IHotBoxParam _param)
    {
        m_ParentBarrel.BarrelHit(_param);
        return 1;
    }
}