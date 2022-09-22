using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;



public class BombWeapon_Enemy : BasicWeapon_Enemy
{
    // Visible Member Variables
    public float p_BombRadius = 0.5f;

    
    // Member Variables
    private IHotBoxParam m_HotBoxParam;
    
    private List<IHotBox> m_HotBoxes = new List<IHotBox>();
    private List<Collider2D> m_Cols = new List<Collider2D>();

    private List<GameObject> m_ForDoubleCheck = new List<GameObject>();
    
    
    // Constructors
    private void Awake()
    {
        m_isPlayers = false;
        m_HotBoxParam = new IHotBoxParam(p_BulletDamage, p_StunValue, Vector2.zero, WeaponType.GRENADE, false);
        GetComponent<CircleCollider2D>().radius = p_BombRadius;
    }


    // Functions
    public override int Fire()
    {
        m_ForDoubleCheck.Clear();
        m_HotBoxParam.ResetContactPoint(transform.position);
        
        foreach (var ele in m_Cols)
        {
            m_HotBoxes.Add(ele.GetComponent<IHotBox>());
        }
        
        foreach (var ele in m_HotBoxes)
        {
            if (m_ForDoubleCheck.Contains(ele.m_ParentObj)) 
                continue;
            
            m_ForDoubleCheck.Add(ele.m_ParentObj);
            ele.HitHotBox(m_HotBoxParam);
        }
        
        return 1;
    }

    public override void Reload()
    {
        //return 0;
    }

    public override void InitWeapon()
    {
        
    }

    public override void ExitWeapon()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Cols.Add(col);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Cols.Remove(other);
    }
}