using System;
using Unity.VisualScripting;
using UnityEngine;


public class LocationSensor : MonoBehaviour
{
    // Visible Member Variables
    public bool m_isPlayers { get; set; }
    

    // Member Variables
    private Human m_ParentHuman;
    private LocationInfo m_ParentLocationInfo;  // 본인의 LocationInfo (Human에 있다.)
    private EnemyMgr m_EnemyMgr;

    private bool m_IsLocationReset = false;

    public LocationInfo m_PreLocationInfo { get; private set; } = null;
    public LocationInfo m_CurLocationInfo { get; private set; } = null;

    
    // Constructors
    private void Awake()
    {
        m_ParentHuman = GetComponentInParent<Human>();
        m_ParentLocationInfo = m_ParentHuman.GetComponentInChildren<LocationInfo>();
        
        if (GetComponentInParent<Player>())
        {
            m_isPlayers = true;
        }
    }

    private void Start()
    {
        m_EnemyMgr = InstanceMgr.GetInstance().GetComponentInChildren<EnemyMgr>();
    }


    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_isPlayers)
        {
            switch (m_ParentHuman.gameObject.layer)
            {
                case 12:
                    if (!col.CompareTag("RoomLocation"))
                        return;
                    break;
                
                case 10:
                    if (!col.CompareTag("StairLocation"))
                        return;
                    break;
            }
        }
        else
        {
            switch (m_ParentHuman.gameObject.layer)
            {
                case 11:
                    if (!col.CompareTag("RoomLocation"))
                        return;
                    break;
                
                case 9:
                    if (!col.CompareTag("StairLocation"))
                        return;
                    break;
            }
        }

        if (!m_IsLocationReset)
        {
            m_IsLocationReset = true;
            m_CurLocationInfo = col.GetComponentInChildren<LocationInfo>();
            m_PreLocationInfo = m_CurLocationInfo;
        }

        m_PreLocationInfo = m_CurLocationInfo;
        m_CurLocationInfo = col.GetComponentInChildren<LocationInfo>();
        
        m_ParentLocationInfo.SetLocation(m_CurLocationInfo);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_isPlayers)
        {
            
        }
        else
        {
            
        }
        
        if (!(other.CompareTag("RoomLocation") || other.CompareTag("StairLocation")))
            return;
        
        
        LocationInfo temp = other.GetComponentInChildren<LocationInfo>();
        if (temp == m_CurLocationInfo)
        {
            m_CurLocationInfo = m_PreLocationInfo;
            m_PreLocationInfo = temp;

            m_ParentLocationInfo.SetLocation(m_CurLocationInfo);
            
            if(m_isPlayers)
                m_EnemyMgr.SetAllEnemysDestinationToPlayer();
        }
    }
}