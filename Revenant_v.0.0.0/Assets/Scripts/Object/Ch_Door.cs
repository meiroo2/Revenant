using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_Door : MonoBehaviour, IUseableObj
{
    private Door m_PDoor;
    private void Awake()
    {
        m_PDoor = GetComponentInParent<Door>();
    }

    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        m_PDoor.useDoor();
        return 1;
    }
}
