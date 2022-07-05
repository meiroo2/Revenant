using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IconMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_PullingIconObj;
    public int m_PullingCount = 5;
    
    
    // Member Variables
    private IconObj[] m_PulledIcons;

    // Constructors
    private void Awake()
    {
        List<GameObject> tempObjs = new List<GameObject>();
        for (int i = 0; i < m_PullingCount; i++)
        {
            tempObjs.Add(Instantiate(m_PullingIconObj));
            tempObjs[i].transform.parent = this.transform;
            tempObjs[i].SetActive(false);
        }
    }
    
    
    // Functions

}
















