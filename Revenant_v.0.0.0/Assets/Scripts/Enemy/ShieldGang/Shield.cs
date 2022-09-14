using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Shield : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public int p_Shield_Hp = 30;
    public int p_ShieldDmgMulti = 1;
    
    // Member Variables
    private ShieldGang m_ShieldGang;
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;

    private Collider2D m_Collider;
    private List<Rigidbody2D> m_SpriteRigids = new List<Rigidbody2D>();

    
    // Constructors
    private void Awake()
    {
        m_ParentObj = gameObject;

        m_Collider = GetComponent<Collider2D>();

        var SpRenders = GetComponentsInChildren<SpriteRenderer>();
        foreach (var element in SpRenders)
        {
            var rigid = element.GetComponent<Rigidbody2D>();
            m_SpriteRigids.Add(rigid);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.isKinematic = true;
        }
        
        m_ShieldGang = GetComponentInParent<ShieldGang>();
    }


    // Functions

    public int HitHotBox(IHotBoxParam _param)
    {
        if (p_Shield_Hp <= 0)
            return 0;
        
        p_Shield_Hp -= _param.m_Damage * p_ShieldDmgMulti;

        if (p_Shield_Hp <= 0)
        {
            m_ShieldGang.ShieldBroken();
            StartCoroutine(PopShield());
            m_Collider.enabled = false;
        }
        
        return 1;
    }

    private IEnumerator PopShield()
    {
        Vector2 RotateVec = Vector2.up;

        foreach (var element in m_SpriteRigids)
        {
            RotateVec = Vector2.up;
            RotateVec = StaticMethods.GetRotatedVec(RotateVec, Random.Range(-60f, 60f));
            
            element.isKinematic = false;
            element.AddForce(RotateVec * 3f, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

        yield break;
    }
}