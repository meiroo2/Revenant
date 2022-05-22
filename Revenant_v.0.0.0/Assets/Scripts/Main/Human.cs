using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : ObjectDefine
{
    // Visible Member Variables
    
    
    // Member Variables
    [field: SerializeField] public int m_Hp { get; protected set; } = 10;
    [field: SerializeField] public float m_Speed { get; protected set; } = 1f;
    [field: SerializeField] public float m_stunTime { get; protected set; } = 0f;
    [field: SerializeField] public float m_stunThreshold { get; protected set; } = 0f;
    public bool m_isRightHeaded { get; protected set; } = true;
    public Vector2 m_originPos { get; protected set; } = Vector2.zero;
    public LocationInfo m_curLocation { get; protected set; }
    
    public bool m_CanMove { get; set; } = true;
    public bool m_CanShot { get; set; } = true;
    public Vector2 m_HumanMoveVec { get; protected set; }

    
    // Constructor


    // Functions
    public virtual void setisRightHeaded(bool _isRightHeaded)
    {
        m_isRightHeaded = _isRightHeaded;
        var TempLocalScale = transform.localScale;

        if ((m_isRightHeaded && TempLocalScale.x < 0) || (!m_isRightHeaded && TempLocalScale.x > 0))
            transform.localScale = new Vector3(-TempLocalScale.x, TempLocalScale.y, 1);
    }
    
}