using System;
using UnityEngine;



public class Barrel : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public Sprite p_BrokenBarrelSprite { get; private set; }

    
    // Member Variables
    private SpriteRenderer m_SpriteRenderer;
    private int m_Hp = 2;
    private BombWeapon_Enemy m_BombWeapon;

    // Constructors
    private void Awake()
    {
        m_Hp = 2;
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_BombWeapon = GetComponentInChildren<BombWeapon_Enemy>();
    }

    // Functions
    public void BarrelHit(IHotBoxParam _param)
    {
        if (m_Hp <= 0)
            return;

        m_Hp -= 1;

        if (m_Hp == 1)
        {
            m_SpriteRenderer.sprite = p_BrokenBarrelSprite;
        }
        
        if (m_Hp <= 0)
        {
            m_Hp = 0;
            m_BombWeapon.Fire();
            gameObject.SetActive(false);
        }
    }
}