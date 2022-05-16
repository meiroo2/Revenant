using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEnemy_Ear : MonoBehaviour
{
    private NoisePrefab m_DetectedNoise;
    private DaggerEnemy m_Enemy;
    private void Awake()
    {
        m_Enemy = GetComponentInParent<DaggerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Noise"))
        {
            m_DetectedNoise = collision.GetComponent<NoisePrefab>();
            m_Enemy.setNoiseSourceLocation(m_DetectedNoise.m_curLocation);
        }
        else if (collision.CompareTag("RoomLocation"))
        {
            m_Enemy.setEntityLocation(collision.GetComponent<LocationInfo>());
        }
    }
}