using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Door_Col_LayerRoom : MonoBehaviour, IUseableObj
{
    public bool m_IsOutlineActivated { get; set; } = false;
    
    public Door_LayerRoom m_Door;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    private void Awake()
    {
        m_Door = GetComponentInParent<Door_LayerRoom>();
    }
    
    public void ActivateOutline(bool _isOn)
    {
        if (ReferenceEquals(m_Door.m_Animator, null) || m_Door.m_IsOpen == _isOn)
            return;
        
        m_Door.ActivateBothOutline(_isOn);
    }

    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.LAYERDOOR;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        m_Door.MoveToOtherSide(_param.m_UserTransform, _param.m_isPlayer);
        return 1;
    }
}
