using System;
using UnityEngine;
using UnityEngine.Events;


public class Switch : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UnityEvent p_Event;
    public DynamicOutline p_DynamicOutline = null;
    
    
    // Member Variables
    private Collider2D m_Collider;
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;


    // Constructors
    private void Awake()
    {
        m_Collider = GetComponentInChildren<Collider2D>();
        
        if (ReferenceEquals(p_DynamicOutline, null))
            Debug.Log(gameObject.name + " 스위치에서 DynamicOutline 할당이 되어있지 않음.");
    }

    public bool m_IsOutlineActivated { get; set; }
    
    public int useObj(IUseableObjParam _param)
    {
        if (m_isOn)
            return 0;

        // 문 열기 
        p_Event?.Invoke();
        m_isOn = true;

        if (!ReferenceEquals(p_DynamicOutline.m_Animator, null))
        {
            p_DynamicOutline.m_Animator.enabled = true;
            p_DynamicOutline.m_Animator.SetTrigger("Push");
        }

        m_Collider.enabled = false;
        
        return 1;
    }
    
    public void ActivateOutline(bool _isOn)
    {
        
    }
}