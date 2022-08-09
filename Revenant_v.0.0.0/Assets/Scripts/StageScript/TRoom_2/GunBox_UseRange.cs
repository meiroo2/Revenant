using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox_UseRange : MonoBehaviour, IUseableObj
{
    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    private GameObject m_ProgressMgr;

    private void Awake()
    {
        m_ProgressMgr = GameObject.FindGameObjectWithTag("ProgressMgr");
    }

    public int useObj(IUseableObjParam _param)
    {
        if (m_isOn == false)
            m_ProgressMgr.SendMessage("NextProgress");

        m_isOn = true;
        return 1;
    }
}
