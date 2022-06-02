using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMgr : MonoBehaviour
{
    // Visible Member Variables
    public List<BasicEnemy> p_Enemys;
    
    
    // Member Variables
    
    
    // Constructors
    private void Awake()
    {
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < tempArr.Length; i++)
        {
            p_Enemys.Add(tempArr[i].GetComponent<BasicEnemy>());
        }
    }
    
    // Functions
    public void SetAllEnemysDestinationToPlayer()
    {
        
    }
}