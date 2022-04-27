using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefenseActivate : MonoBehaviour, IEnemySpawn
{
    [SerializeField]
    Vector2 defensePos = new Vector2(0, 0);

    bool isActive = false;
    EnemyA m_enemy;


    private void Awake()
    {
        m_enemy = GetComponent<EnemyA>();

    }
    public void setActive()
    {
        if (isActive)
            Debug.Log("Already Active == True");
        else
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }
    public void getInfo()
    {
        Debug.Log(gameObject.name);
    }
    // Enemy_DefenseMap
    // Ã³À½ = ¸Ê Áß°£
    public void DefenseMode()
    {
        m_enemy.m_sensorPos = defensePos;
        m_enemy.GuardAIState();
        m_enemy.curEnemyState = EnemyState.GUARD;
    }
    
}

