using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Knife_Enemy : BasicWeapon_Enemy
{
    // Visible Member Variables
    public GameObject p_KnifeObj;
    public Transform p_KnifePos;
    public float p_KnifeHotBoxTime = 0.1f;
    
    
    // Member Variables
    private GameObject m_InstantiatedKnifeHotBox;

    
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
        m_InstantiatedKnifeHotBox = GameObject.Instantiate(p_KnifeObj, transform, true);
    }

    private void Start()
    {
        var tempIns = InstanceMgr.GetInstance();
        m_SoundMgrSFX = tempIns.GetComponentInChildren<SoundMgr_SFX>();
        m_Player = tempIns.GetComponentInChildren<Player_Manager>().m_Player;
        m_InstantiatedKnifeHotBox.GetComponent<KnifeBullet_Enemy>().m_SoundMgrSFX = m_SoundMgrSFX;
        m_InstantiatedKnifeHotBox.SetActive(false);
    }

    public override int Fire()
    {
        m_InstantiatedKnifeHotBox.SetActive(true);
        StopCoroutine(Internal_Fire());
        StartCoroutine(Internal_Fire());
        return 1;
    }

    private IEnumerator Internal_Fire()
    {
        yield return new WaitForSeconds(p_KnifeHotBoxTime);
        m_InstantiatedKnifeHotBox.SetActive(false);
    }

    public override int Reload()
    {
        return 0;
    }

    public override void InitWeapon()
    {
        
    }

    public override void ExitWeapon()
    {
        
    }
}
