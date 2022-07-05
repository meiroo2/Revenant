using System;
using Unity.VisualScripting;
using UnityEngine;


public class Target_HotBox : MonoBehaviour, IHotBox
{
    public HitBoxPoint p_Point;
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; }

    private void Awake()
    {
        m_HitBoxInfo = p_Point;
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        return 1;
    }
}