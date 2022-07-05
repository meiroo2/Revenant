using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStage2_Door : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    
    
    // Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    private SpriteOutline m_Outline;

    // Constructors
    private void Awake()
    {
        m_Outline = GetComponentInParent<SpriteOutline>();
        m_Outline.enabled = false;
    }

    // Functions
    public void NowCanColliderToPlayer()
    {
        m_isOn = true;
        m_Outline.enabled = true;
        m_Outline.outlineSize = 0;
    }
    public int useObj(IUseableObjParam _param)
    {
        if (!m_isOn)
            return 0;

        SceneManager.LoadScene(4);
        return 1;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Outline.outlineSize = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Outline.outlineSize = 0;
    }
}