using System;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;


public class KnifeBullet_Enemy : MonoBehaviour
{
    // Visible Member Variables
    public int p_WeaponMode = 0;

    
    // Member Variables
    private KnifeWeapon_Enemy m_KnifeParent;


    // Constructor
    private void Awake()
    {
        m_KnifeParent = GetComponentInParent<KnifeWeapon_Enemy>();
    }


    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        var colHotBox = col.GetComponent<IHotBox>();

        if (colHotBox.m_isEnemys == false && colHotBox.m_HitBoxInfo == HitBoxPoint.BODY)
        {
            m_KnifeParent.m_SoundMgrSFX.playGunFireSound(2, gameObject);
            colHotBox.HitHotBox(new IHotBoxParam(m_KnifeParent.p_BulletDamage, 0, transform.position,
                WeaponType.KNIFE));
        }
    }
}