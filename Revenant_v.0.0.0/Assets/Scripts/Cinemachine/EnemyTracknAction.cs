using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class EnemyTracknAction : EnemySpawner
{
    // Visible Member Variables
    public UnityEvent p_WillExecute;
    public BasicEnemy[] p_CheckEnemyArr;
    private bool m_IsActivated = false;
    
    // Member Variables
    [Space(50f)]
    private Dictionary<GameObject, bool> m_EnemyObjDic = new Dictionary<GameObject, bool>();


    // Constructors
    private new void Awake()
    {
        m_EnemyObjDic = new Dictionary<GameObject, bool>();
        
        for (int i = 0; i < p_CheckEnemyArr.Length; i++)
        {
            p_CheckEnemyArr[i].AddEnemySpawner(this);
           
            m_EnemyObjDic.Add(p_CheckEnemyArr[i].gameObject, false);
        }
    }

    public override void AchieveEnemyDeath(GameObject _enemy)
    {
        if (m_IsActivated)
            return;

        if (m_EnemyObjDic.ContainsKey(_enemy))
        {
            m_EnemyObjDic[_enemy] = true;
        }
        
        bool allDead = true;

        foreach (var VARIABLE in m_EnemyObjDic.Values)
        {
            if (!VARIABLE)
                allDead = false;
        }
        
        if (allDead)
        {
            m_IsActivated = true;

            if (p_SpawnDelay <= 0f)
                p_WillExecute?.Invoke();
            else
                StartCoroutine(WaitnExecute(p_WillExecute));
        }
    }

    private IEnumerator WaitnExecute(UnityEvent _event)
    {
        yield return new WaitForSeconds(p_SpawnDelay);
        _event?.Invoke();
    }
}