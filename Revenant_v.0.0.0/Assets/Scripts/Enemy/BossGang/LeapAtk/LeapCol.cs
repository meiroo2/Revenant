using System;
using System.Collections.Generic;
using UnityEngine;


public class LeapCol : MonoBehaviour
{
    public bool m_IsLeftCol = true;
    public LeapColMaster m_LeapColMaster;
    
    // Trigger Phase
    private int m_Phase = 0;

    private List<IHotBox> m_IHotBoxList = new List<IHotBox>();
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (m_Phase)
        {
            case 0:
                if (m_IsLeftCol)
                    m_LeapColMaster.m_IsLeftCollide = true;
                else
                    m_LeapColMaster.m_IsRightCollide = true;
                
                break;
            
            case 1:
                if (col.TryGetComponent(out IHotBox hotBox))
                {
                    if (hotBox.m_isEnemys)
                        return;
                    
                    m_IHotBoxList.Add(hotBox);
                }
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (m_Phase)
        {
            case 0:
                if (m_IsLeftCol)
                    m_LeapColMaster.m_IsLeftCollide = false;
                else
                    m_LeapColMaster.m_IsRightCollide = false;
                
                break;
            
            case 1:
                if (other.TryGetComponent(out IHotBox hotBox))
                {
                    if (hotBox.m_isEnemys)
                        return;

                    m_IHotBoxList.Remove(hotBox);
                }
                break;
        }
    }

    public void ChangeColPhase(int _phase)
    {
        switch (_phase)
        {
            case -1:
                m_Phase = -1;
                break;
            
            case 0:
                m_Phase = 0;
                break;
            
            case 1:
                m_Phase = 1;
                break;

            default:
                Debug.Log("LeapCol에서 잘못된 Phase 변경 요청");
                break;
        }
    }
    
    public void ClearList()
    {
        m_IHotBoxList.Clear();
        m_IHotBoxList.TrimExcess();
    }

    public void Attack()
    {
        IHotBoxParam param = new IHotBoxParam(10, 0, transform.position,
            WeaponType.KNIFE);
        
        for (int i = 0; i < m_IHotBoxList.Count; i++)
        {
            param.m_contactPoint = m_IHotBoxList[i].m_ParentObj.transform.position;
            m_IHotBoxList[i].HitHotBox(param);
        }
    }
}