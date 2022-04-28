using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_Portal_LayerRoom : MonoBehaviour, IUseableObj
{
    private Portal_LayerRoom m_Pa_Portal;

    private void Awake()
    {
        m_Pa_Portal = GetComponentInParent<Portal_LayerRoom>();
    }

    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        m_Pa_Portal.moveObjToOtherSide(_param.m_UserTransform, _param.m_isPlayer);
        return 1;
    }
}
