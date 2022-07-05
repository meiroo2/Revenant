using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : ObjectDefine
{
    // Visible Member Variables
    [field: SerializeField] public int p_Hp { get; protected set; } = 10;
    [field: SerializeField] public float p_Speed { get; protected set; } = 1f;
    [field: SerializeField] public float p_stunTime { get; protected set; } = 0f;
    [field: SerializeField] public int p_stunThreshold { get; protected set; } = 0;



    // Member Variables
    protected int m_CurStunValue = 0;
    public bool m_IsRightHeaded { get; protected set; } = true;
    public Vector2 m_OriginPos { get; protected set; } = Vector2.zero;
    public LocationInfo m_CurLocation { get; protected set; }
    
    public bool m_CanMove { get; set; } = true;
    public bool m_CanFire { get; set; } = true;
    public Vector2 m_HumanFootNormal { get; set; }

    
    // Constructor
    protected void InitHuman()
    {
        setisRightHeaded(transform.localScale.x > 0);
    }

    // Functions
    public Vector2 GetBodyCenterPos()
    {
        Vector2 position = transform.position;
        return new Vector2(position.x, position.y - 0.36f);
    }

    public virtual void setisRightHeaded(bool _isRightHeaded)
    {
        m_IsRightHeaded = _isRightHeaded;
        var TempLocalScale = transform.localScale;

        if ((m_IsRightHeaded && TempLocalScale.x < 0) || (!m_IsRightHeaded && TempLocalScale.x > 0))
            transform.localScale = new Vector3(-TempLocalScale.x, TempLocalScale.y, 1);
    }
}