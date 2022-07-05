using System;
using UnityEngine;


public class Switch_Room0 : MonoBehaviour, IUseableObj
{
    // Member Variables
    private Animator m_Animator;
    private SpriteOutline m_Outline;
    public Portal_LayerRoom m_Layerroom;
    
    
    // Constructors
    private void Awake()
    {
        m_Outline = GetComponent<SpriteOutline>();
        m_Animator = GetComponent<Animator>();
    }

    
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        if (m_isOn)
            return 0;

        // 문 열기 
        m_Animator.SetTrigger("Push");
        m_Layerroom.p_CanInteract = true;
        m_Layerroom.p_DoorAnimator.SetTrigger("Open");
        m_isOn = true;
        return 1;
    }
    public void ActivateOutline(bool _isOn)
    {
        if (_isOn)
            m_Outline.outlineSize = 1;
        else
            m_Outline.outlineSize = 0;
    }
}