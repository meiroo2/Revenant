using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Player_OldMelee : MonoBehaviour
{
    // Visible Member Variables
    public int m_Damage = 10;
    public int m_StunValue = 10;
    
    
    // Member Variables
    private Dictionary<Collider2D, IHotBox> m_HotBoxDic = new Dictionary<Collider2D, IHotBox>();
    private List<IHotBox> m_HotBoxList = new List<IHotBox>();

    private HitSFXMaker m_HitSFXMaker;


    private void Start()
    {
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
    }


    // Functions

    public void DoOldMelee()
    {
        m_HotBoxList = m_HotBoxDic.Values.ToList();

        if (m_HotBoxList.Count <= 0)
            return;

        IHotBox nearBox = StaticMethods.GetNearestHotBox(transform.position, m_HotBoxList);
        
        nearBox.HitHotBox(new IHotBoxParam(m_Damage, m_StunValue, nearBox.m_ParentObj.transform.position,
            WeaponType.KNIFE, true));
        m_HitSFXMaker.EnableNewObj(2, nearBox.m_ParentObj.transform.position, true);
    }
    
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        m_HotBoxDic.Add(col, col.GetComponent<IHotBox>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_HotBoxDic.Remove(other);
    }
}