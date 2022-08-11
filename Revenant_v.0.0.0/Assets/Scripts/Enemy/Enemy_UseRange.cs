using System;
using UnityEngine;

public class Enemy_UseRange : MonoBehaviour
{
    // Visible Member Variables
    public Enemy_FootMgr p_Enemy_FootMgr;


    // Member Variables
    private BasicEnemy m_Enemy;
    private StairPos m_StairPos;
    private IUseableObj m_UseableObj;

    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if (col.CompareTag("Player"))
            return;
        
        m_UseableObj = col.GetComponent<IUseableObj>();

        switch (m_UseableObj.m_ObjProperty)
        {
            case UseableObjList.LAYERDOOR:
                m_UseableObj.ActivateOutline(true);
                break;
        }
        */
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player"))
            return;
        
        m_UseableObj = other.GetComponent<IUseableObj>();

        switch (m_UseableObj.m_ObjProperty)
        {
            case UseableObjList.LAYERDOOR:
                m_UseableObj.ActivateOutline(false);
                break;
        }
        */
    }
}