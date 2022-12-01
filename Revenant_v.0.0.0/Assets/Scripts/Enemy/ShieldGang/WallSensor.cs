using System;
using System.Collections.Generic;
using UnityEngine;



public class WallSensor : MonoBehaviour
{
    // Member Variables
    public bool m_IsTouch { get; private set; } = false;

    private Dictionary<Collider2D, IHotBox> m_ColnHotBoxDic = new Dictionary<Collider2D, IHotBox>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out IHotBox hotBox)) 
            return;
        
        if (hotBox.m_HitBoxInfo == HitBoxPoint.FLOOR)
        {
            m_IsTouch = true;
            m_ColnHotBoxDic.Add(col, hotBox);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_ColnHotBoxDic.ContainsKey(other))
        {
            m_ColnHotBoxDic.Remove(other);

            if (m_ColnHotBoxDic.Count <= 0)
                m_IsTouch = false;
        }
    }
}