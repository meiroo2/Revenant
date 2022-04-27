using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PosType
{
    DefenseMap,
    NormalStage
}


public class EnemyEventPos : MonoBehaviour
{
    [SerializeField]
    PosType m_posType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(m_posType)
        {
            case PosType.DefenseMap:
                if (collision.CompareTag("Enemy"))
                {
                    DefenseEnemy d =
                    collision.GetComponent<DefenseEnemy>();
                    d.m_canSensor = true;
                    d.m_playerTransform = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
                }
            break;
        }
        
    }
}
