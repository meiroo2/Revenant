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
        m_HotBoxParam.ResetContactPoint(transform.position);

        var arr = m_HotBoxes.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].HitHotBox(m_HotBoxParam);
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
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            m_HotBoxes.Add(hotBox);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox hotBox))
        {
            m_HotBoxes.Remove(hotBox);
        }
    }
}