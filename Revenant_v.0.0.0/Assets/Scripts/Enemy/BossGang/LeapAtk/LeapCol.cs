using System;
using UnityEngine;


public class LeapCol : MonoBehaviour
{
    public bool m_IsLeftCol = true;
    public LeapColMaster m_LeapColMaster;
    public int m_Phase = 0;
    
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
        }
    }
}