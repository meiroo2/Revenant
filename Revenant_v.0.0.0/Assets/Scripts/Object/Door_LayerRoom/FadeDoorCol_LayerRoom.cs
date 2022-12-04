using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDoorCol_LayerRoom : MonoBehaviour, IUseableObj
{
    public bool m_IsOutlineActivated { get; set; } = false;
    
    public Door_LayerRoom m_Door;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    private InGame_UI m_IngameUI;
    
    private void Awake()
    {
        m_Door = GetComponentInParent<Door_LayerRoom>();
    }

    private void Start()
    {
        m_IngameUI = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<InGame_UI>();
    }

    public void ActivateOutline(bool _isOn)
    {
        if (ReferenceEquals(m_Door.m_Animator, null) || m_Door.m_IsOpen == _isOn)
            return;
        
        m_Door.ActivateBothOutline(_isOn);
    }

    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.JUSTTOUCH;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        if (m_isOn)
            return 0;
        
        Debug.Log("Sans");
        
        m_isOn = true;
        m_IngameUI.DoBlackFade(true, 2f, () => PassnFade(_param));
        return 1;
    }

    private void PassnFade(IUseableObjParam _param)
    {
        m_Door.MoveToOtherSide(_param.m_UserTransform, _param.m_isPlayer);
        m_IngameUI.DoBlackFade(false, 2f, null);
    }
}
