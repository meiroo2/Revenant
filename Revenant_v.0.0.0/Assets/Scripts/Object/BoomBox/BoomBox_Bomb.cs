using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class BoomBox_Bomb : MonoBehaviour
{
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_Dic = new Dictionary<Collider2D, IHotBox>();
    private bool m_SafetyLock = false;
    
    // Constructors


    // Functions
    public void Explode(int _damage, int _stunValue)
    {
        m_SafetyLock = true;
        
        foreach (var hotBox in m_Dic.Values)
        {
            hotBox.HitHotBox(new IHotBoxParam(_damage, _stunValue,
                transform.position, WeaponType.BULLET));
        }

        m_SafetyLock = false;
    }
    
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_SafetyLock)
            return;
        
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_HitBoxInfo is HitBoxPoint.BODY or HitBoxPoint.OBJECT)
                m_Dic.Add(col, hotBox);
        }
        else
        {
            Debug.Log("ERR : HotBox 찾지 못함");
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_SafetyLock)
            return;
        
        m_Dic.Remove(other);
    }
}