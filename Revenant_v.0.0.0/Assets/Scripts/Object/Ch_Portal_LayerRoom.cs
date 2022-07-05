using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ch_Portal_LayerRoom : MonoBehaviour, IUseableObj
{
    private Portal_LayerRoom m_Pa_Portal;
    public SpriteOutline p_Outline;

    private void Awake()
    {
        m_Pa_Portal = GetComponentInParent<Portal_LayerRoom>();
    }
    
    public void ActivateOutline(bool _isOn)
    {
        if (_isOn)
            p_Outline.outlineSize = 1;
        else
        {
            p_Outline.outlineSize = 0;
        }
    }
    
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int useObj(IUseableObjParam _param)
    {
        m_Pa_Portal.moveObjToOtherSide(_param.m_UserTransform, _param.m_isPlayer);
        return 1;
    }
}
