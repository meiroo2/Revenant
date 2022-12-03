using System;
using System.Collections.Generic;
using UnityEngine;


public class ShieldGang_Baton : BasicWeapon_Enemy
{
    // Visible Member Variables
    public BasicEnemy p_BasicEnemy;
    public Transform p_FootTransform;
    public float p_AxeEffectPos = 1f;
    
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_HotBoxDic = new Dictionary<Collider2D, IHotBox>();

    private List<IHotBox> m_HotBoxList = new List<IHotBox>();
    private SimpleEffectPuller m_SEPuller;
    private Collider2D m_IHotBoxDetectCol;

    private bool m_DicLocker = false;
    
    // Constructors
    private void Awake()
    {
        if (TryGetComponent(out Collider2D col))
            m_IHotBoxDetectCol = col;
        else
            Debug.Log("ERR : " + gameObject.name + "에서 Collider2D를 찾지 못함.");
    }

    private void Start()
    {
        m_SEPuller = InstanceMgr.GetInstance().GetComponentInChildren<SimpleEffectPuller>();
    }


    // BasicWeapon Functions
    public override int Fire()
    {
        if (p_BasicEnemy.m_IsRightHeaded)
        {
            m_SEPuller.SpawnSimpleEffect(13, new Vector2(p_FootTransform.position.x +  p_AxeEffectPos,
                p_FootTransform.position.y));
        }
        else
        {
            m_SEPuller.SpawnSimpleEffect(13, new Vector2(p_FootTransform.position.x -  p_AxeEffectPos,
                p_FootTransform.position.y), true);
        }
     
            
        for (int i = 0; i < m_HotBoxList.Count; i++)
        {
            m_HotBoxList[i].HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue,
                m_HotBoxList[i].m_ParentObj.transform.position, WeaponType.AXE));
        }

        return 1;
    }
    
    public override void Reload()
    {
        
    }
    public override void InitWeapon()
    {
        
    }
    public override void ExitWeapon()
    {
        
    }
    
    
    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IHotBox box))
        {
            if (!box.m_isEnemys && box.m_HitBoxInfo == HitBoxPoint.BODY)
                m_HotBoxList.Add(box);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHotBox box))
        {
            if (m_HotBoxList.Contains(box))
                m_HotBoxList.Remove(box);
        }
    }
}