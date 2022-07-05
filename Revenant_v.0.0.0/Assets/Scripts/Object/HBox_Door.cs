using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBox_Door : MonoBehaviour, IHotBox
{
    private Vector2 m_InitBoxColliderSize;
    private int m_DoLerp = 0;
    private BoxCollider2D m_HotBoxCol;

    private void Awake()
    {
        m_ParentObj = GetComponentInParent<Door>().gameObject;
        m_HotBoxCol = GetComponent<BoxCollider2D>();
        m_InitBoxColliderSize = new Vector2(m_HotBoxCol.size.x, m_HotBoxCol.size.y);
    }

    private void Update()
    {
        switch (m_DoLerp)
        {
            case 1:
                m_HotBoxCol.size = Vector2.Lerp(m_HotBoxCol.size, m_InitBoxColliderSize, Time.deltaTime * 10f);
                if (m_InitBoxColliderSize.x - m_HotBoxCol.size.x <= 0.01f && m_InitBoxColliderSize.y - m_HotBoxCol.size.y <= 0.01f)
                    m_DoLerp = 0;
                break;
        }
    }

    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;

    public int HitHotBox(IHotBoxParam _param)
    {
        return 1;
    }

    public void EnableCol(bool _isEnable)
    {
        if (_isEnable)
        {
            m_HotBoxCol.enabled = true;
            m_HotBoxCol.size = new Vector2(0.01f, 1f);
            m_DoLerp = 1;
        }
        else
        {
            m_HotBoxCol.enabled = false;
        }
    }
}
