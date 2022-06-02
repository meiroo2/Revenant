using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEnemy_Locator : MonoBehaviour
{
    private DaggerEnemy m_Enemy;

    private LocationInfo m_KnownLocationInfo;

    private void Awake()
    {
        m_Enemy = GetComponentInParent<DaggerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_KnownLocationInfo = collision.GetComponent<LocationInfo>();
        //m_Enemy.m_curLocation.setLocation(m_KnownLocationInfo);
    }
}