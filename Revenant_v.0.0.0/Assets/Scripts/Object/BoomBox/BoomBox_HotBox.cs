using System;
using UnityEngine;


public class BoomBox_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    [HideInInspector] public BoomBox m_BoomBox;

    // IHotBox Variables
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
 
    
    // Constructors
    private void Awake()
    {
        m_ParentObj = this.gameObject;
    }


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_BoomBox.GetHit(_param.m_Damage);
        return 1;
    }
}