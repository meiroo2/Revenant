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
                    //DefenseEnemy_Controller d =
                    //collision.GetComponent<DefenseEnemy_Controller>();
                    Enemy e= collision.GetComponent<Enemy>();
                    e.m_canSensor = true;
                    e.m_playerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
                }
            break;
        }
        
    }
}
