using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class Turret_Room1 : MonoBehaviour
{
    // Member Variables
    private Animator m_Animator;
    private BICTutoMgr m_BICTutoMgr;
    
    public GameObject m_TransparentEnemyPrefab;

    private GameObject m_SpawnedEnemy;

    // Constructors
    private void Awake()
    {
        m_BICTutoMgr = GameObject.FindObjectOfType<BICTutoMgr>();
        m_Animator = GetComponent<Animator>();
    }
    
    
    // Functions
    public void SpawnTurret()
    {
        m_SpawnedEnemy = Instantiate(m_TransparentEnemyPrefab);
        m_SpawnedEnemy.transform.position = transform.position;
        
        m_Animator.SetTrigger("Open");
        StartCoroutine(DeActivate());
    }

    public void ReSpawnTurret()
    {
        m_SpawnedEnemy = Instantiate(m_TransparentEnemyPrefab);
        m_SpawnedEnemy.transform.position = transform.position;
        
        StartCoroutine(DeActivate());
    }

    private IEnumerator DeActivate()
    {
        yield return new WaitForSeconds(4.8f);
        Destroy(m_SpawnedEnemy);
        m_BICTutoMgr.NextPhase();
    }
}