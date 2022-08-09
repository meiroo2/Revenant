using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class C4Box : MonoBehaviour, IUseableObj
{

    private IconMgr _mPlayerHuIconMgr;
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    
    
    // Member Variables
    private SpriteOutline m_Outline;
    
    
    // Constructors
    private void Awake()
    {
        m_Outline = GetComponent<SpriteOutline>();
    }


    // Functions
    public void ActivateOutline(bool _isOn)
    {
        if (_isOn)
            m_Outline.outlineSize = 1;
        else
            m_Outline.outlineSize = 0;
    }

    public bool m_IsOutlineActivated { get; set; }

    public int useObj(IUseableObjParam _param)
    {
        SceneManager.LoadScene(5);
        return 1;
    }
}