using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Room1_Button0 : MonoBehaviour, IUseableObj
{
    private GameObject m_ProgressMgr;

    private void Awake()
    {
        m_ProgressMgr = GameObject.FindGameObjectWithTag("ProgressMgr");
    }

    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        m_ProgressMgr.SendMessage("NextProgress");
        gameObject.SetActive(false);
        return 1;
    }
}
