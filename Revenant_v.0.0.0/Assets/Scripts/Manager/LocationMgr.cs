using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LocationMgr : MonoBehaviour
{
    // Member Variables
    public LocationInfo[] m_CurSceneLocations { get; private set; }

    // Constructors
    private void Awake()
    {
        GameObject[] roomLocations = GameObject.FindGameObjectsWithTag("RoomLocation");
        GameObject[] stairLocations = GameObject.FindGameObjectsWithTag("StairLocation");

        List<LocationInfo> temp = new List<LocationInfo>();
        temp.AddRange(roomLocations.Select(ele => ele.GetComponent<LocationInfo>()));
        temp.AddRange(stairLocations.Select(ele => ele.GetComponent<LocationInfo>()));

        m_CurSceneLocations = temp.ToArray();
    }
    
    // Functions
    public LocationInfo GetDefaultLocation()
    {
        return m_CurSceneLocations[0];
    }
}