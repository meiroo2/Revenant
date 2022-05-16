using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefenseActivate : MonoBehaviour, IEnemySpawn
{
    [SerializeField]
    Vector2 defensePos = new Vector2(0, 0);

    bool isActive = false;
    DefenseEnemy_Controller m_enemy;

    private Transform m_PlayerTransform;

    private void Awake()
    {
        m_enemy = GetComponent<DefenseEnemy_Controller>();
    }



    public void setActive()
    {
        DefenseMode();

        if (isActive)
            Debug.Log("Already Active == True");
        else
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    // Enemy_DefenseMap
    // 처음 = 맵 중간
    public void DefenseMode()
    {
        m_enemy.m_sensorPos = defensePos;
        m_enemy.GuardAIState();
        m_enemy.curEnemyState = EnemyState.GUARD;
    }


    // 확인용 로그
    public void getInfo()
    {
        Debug.Log(gameObject.name);
    }
}

