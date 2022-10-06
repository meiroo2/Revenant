using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaserMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_LaserPrefab;
    public int p_LaserCount = 5;
    
    
    // Member Variables
    private BulletLaser[] m_BulletLaserArr;
    private int m_Idx = 0;
    
    
    // Constructors
    private void Awake()
    {
        m_BulletLaserArr = new BulletLaser[p_LaserCount];
        for (int i = 0; i < p_LaserCount; i++)
        {
            m_BulletLaserArr[i] = Instantiate(p_LaserPrefab).GetComponent<BulletLaser>();
            m_BulletLaserArr[i].transform.parent = this.transform;
        }
    }
    
    
    // Functions
    public void PoolingBulletLaser(Vector2 _startPos, Vector2 _endPos)
    {
        if (m_Idx < m_BulletLaserArr.Length)
        {
            m_BulletLaserArr[m_Idx].MakeBulletLaser(_startPos, _endPos);
            m_Idx++;
        }
        else
        {
            m_Idx = 0;
            m_BulletLaserArr[m_Idx].MakeBulletLaser(_startPos, _endPos);
        }
    }
}
