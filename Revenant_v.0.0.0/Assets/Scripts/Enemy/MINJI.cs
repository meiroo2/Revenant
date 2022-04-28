using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MINJI : MonoBehaviour
{
    public static MINJI Instance { get;private set; } = null;

    [SerializeField]
    GameObject m_enemyMgrprefab  = null;
    public EnemyMgr m_enemyMgr { get; private set; } = null;

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        InitMgrs();
    }

    void InitMgrs()
    {
        GameObject obj =Instantiate(m_enemyMgrprefab);
        m_enemyMgr = GetComponent<EnemyMgr>();
    }
}
