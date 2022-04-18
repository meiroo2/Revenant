using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1_PillarDoor : MonoBehaviour, IUseableObj
{
    public int m_Idx { get; set; } = 0;
    private Animator m_GateAni;
    public BoxCollider2D m_BoundCollider;

    private GameObject m_ProgressMgr;

    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;

    private void Awake()
    {
        m_ProgressMgr = GameObject.FindGameObjectWithTag("ProgressMgr");
        m_GateAni = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(m_Idx == 1)
        {
            if(m_GateAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                m_BoundCollider.enabled = false;
                m_Idx++;
            }
        }
    }

    public bool useObj()
    {
        if(m_Idx == 0)
        {
            m_ProgressMgr.SendMessage("NextProgress");
            GetComponentInParent<WorldUI>().AniSetIUI(new IUIParam("isOpen", 1));
            m_Idx++;
        }
        return true;
    }
}