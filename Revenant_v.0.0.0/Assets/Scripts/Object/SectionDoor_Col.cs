using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


public class SectionDoor_Col : MonoBehaviour, IUseableObj
{
    public SectionDoor MainSectionDoor;
    
    
    public bool m_IsOutlineActivated { get; set; } = false;
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.LAYERDOOR;
    public bool m_isOn { get; set; } = true;
    
    public int useObj(IUseableObjParam _param)
    {
        MainSectionDoor.TeleportTransform(_param.m_UserTransform);
        return 1;
    }
}