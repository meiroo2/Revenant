using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BoomBox_Bomb : MonoBehaviour
{
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_Dic = new Dictionary<Collider2D, IHotBox>();
    
    
    // Functions
    public void Explode(int _damage, int _stunValue)
    {
        foreach (var hotBox in m_Dic.Values)
        {
            hotBox.HitHotBox(new IHotBoxParam(_damage, _stunValue,
                transform.position, WeaponType.BULLET));
        }
    }
    
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        IHotBox colHotbox = col.GetComponent<IHotBox>();

        if (colHotbox.m_HitBoxInfo == HitBoxPoint.BODY)
            m_Dic.Add(col, colHotbox);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Dic.Remove(other);
    }
}