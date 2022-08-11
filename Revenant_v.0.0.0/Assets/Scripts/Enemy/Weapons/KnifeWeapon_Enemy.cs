using System;
using System.Collections;
using UnityEngine;


public class KnifeWeapon_Enemy : BasicWeapon_Enemy
{
    // Visible Member Variables
    public float p_WeaponColliderTime = 0.1f;
    
    // Member Variables
    private KnifeBullet_Enemy m_KnifeBullet;

    private void Awake()
    {
        m_KnifeBullet = GetComponentInChildren<KnifeBullet_Enemy>();
        m_isPlayers = false;
    }

    private void Start()
    {
        m_KnifeBullet.gameObject.SetActive(false);
    }
    private IEnumerator Internal_Fire()
    {
        yield return new WaitForSeconds(p_WeaponColliderTime);
        m_Callback?.Invoke();
        m_KnifeBullet.gameObject.SetActive(false);
    }
    
    public override int Fire()
    {
        m_KnifeBullet.gameObject.SetActive(true);
        StopCoroutine(Internal_Fire());
        StartCoroutine(Internal_Fire());
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
}