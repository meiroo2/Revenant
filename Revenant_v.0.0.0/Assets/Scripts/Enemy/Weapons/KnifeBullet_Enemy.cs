using System;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;


public class KnifeBullet_Enemy : MonoBehaviour
{
    // Visible Member Variables
    public int p_WeaponMode = 0;
    
    // Member Variables
    private Knife_Enemy m_KnifeParent;
    private KnifeWeapon_Enemy _mKnifeWeapon;
    public SoundMgr_SFX m_SoundMgrSFX;


    // Constructor
    private void Awake()
    {
        switch (p_WeaponMode)
        {
            case 0:
                m_KnifeParent = GetComponentInParent<Knife_Enemy>();
                break;
            
            case 1:
                _mKnifeWeapon = GetComponentInParent<KnifeWeapon_Enemy>();
                break;
        }
    }

    private void Start()
    {
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }


    private void OnEnable()
    {
        switch (p_WeaponMode)
        {
            case 0:
                transform.localPosition = m_KnifeParent.p_KnifePos.localPosition;
                break;
            
            case 1:
                
                break;
        }
    }

    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        var colHotBox = col.GetComponent<IHotBox>();

        if (colHotBox.m_isEnemys == false)
        {
            m_SoundMgrSFX.playGunFireSound(0, gameObject);
        
            switch (p_WeaponMode)
            {
                case 0:
                    colHotBox.HitHotBox(new IHotBoxParam(m_KnifeParent.p_BulletDamage, 0, transform.position, WeaponType.KNIFE));
                    break;
            
                case 1:
                    colHotBox.HitHotBox(new IHotBoxParam(_mKnifeWeapon.p_BulletDamage, _mKnifeWeapon.p_StunValue, transform.position, WeaponType.KNIFE));
                    break;
            }
        }
    }
}