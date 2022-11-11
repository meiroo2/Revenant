using System;
using JetBrains.Annotations;
using UnityEngine;





public class TimeSliceCircleCol : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public TimeSliceObj p_TimeSliceObj;
    
    // Member Variables
    private HitSFXMaker m_HitSFXMaker;
    
    private Vector2 m_StartPos;
    private Vector2 m_EndPos;
    private float m_LerpPos = 0f;
    
    private float m_Speed;
    private bool m_DoUpdate = false;

    public Action m_MoveCompleteAction = null;
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
    public int HitHotBox(IHotBoxParam _param)
    {
        p_TimeSliceObj.GetCircleColInfo(1);
        m_HitSFXMaker.EnableNewObj(0, transform.position);
        m_DoUpdate = false;
        return 1;
    }

    private void Awake()
    {
        m_ParentObj = gameObject;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
    }

    public void InitTimeSliceCircleCol(float _startPos, float _destPos, float _speed, Action _moveCompleteAcion)
    {
        m_StartPos = new Vector2(0f, _startPos);
        m_EndPos = new Vector2(0f, _destPos);
        m_Speed = _speed;
        m_MoveCompleteAction = _moveCompleteAcion;
        
        m_LerpPos = 0f;
        transform.localPosition = m_StartPos;
        
        m_DoUpdate = true;
    }

    private void FixedUpdate()
    {
        if (!m_DoUpdate)
            return;

        m_LerpPos += Time.deltaTime * m_Speed;
        if (m_LerpPos > 1f)
        {
            m_LerpPos = 1f;
            transform.localPosition = Vector2.Lerp(m_StartPos, m_EndPos, m_LerpPos);
            m_MoveCompleteAction?.Invoke();
            m_DoUpdate = false;
            p_TimeSliceObj.GetCircleColInfo(0);
        }
        else
        {
            transform.localPosition = Vector2.Lerp(m_StartPos, m_EndPos, m_LerpPos);
        }
    }
}