using System;
using System.Collections.Generic;
using UnityEngine;


public class Player_MeleeAttack : MonoBehaviour
{
    // Visible Member Variables
    public int m_Damage = 10;
    
    
    // Member Variables
    private SoundMgr_SFX m_SoundSFXMgr;
    private HitSFXMaker m_SFXMaker;
    private BoxCollider2D m_MeleeCol;
    private List<Collider2D> m_HotBoxList;
    
    
    // Constructors
    private void Awake()
    {
        // Assignment
        m_MeleeCol = GetComponent<BoxCollider2D>();
        
        // Init
        m_HotBoxList = new List<Collider2D>();
        
        m_MeleeCol.enabled = false;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_SFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SoundSFXMgr = instance.GetComponentInChildren<SoundMgr_SFX>();
    }


    // Functions
    public void StartFindHotBoxes()
    {
        Debug.Log("히트박스 검색 시작");
        m_HotBoxList.Clear();
        m_MeleeCol.enabled = true;
    }

    public void AttackAllHotBoxes()
    {
        Debug.Log("히트박스 타격 시작");
        m_MeleeCol.enabled = false;
        foreach (var element in m_HotBoxList)
        {
            IHotBox hotBox = element.GetComponent<IHotBox>();
            if (hotBox.m_HitBoxInfo == HitBoxPoint.HEAD)
                continue;

            hotBox.HitHotBox(new IHotBoxParam(m_Damage, 0,
                transform.position, WeaponType.KNIFE));
            
            m_SFXMaker.EnableNewObj(2, element.transform.position);
            m_SoundSFXMgr.playGunFireSound(0, element.transform.position);
        }
    }
    
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        m_HotBoxList.Add(col);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_MeleeCol.enabled == false)
            return;
        
        m_HotBoxList.Remove(other);
    }
}