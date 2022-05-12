using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosTrigger : MonoBehaviour
{
    float m_fixValue = 0.2f;
    bool m_canDetect = true; // 일회성 감지 bool
    Player m_player;

    private void Start()
    {
        m_player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    private void Update()
    {
        if(m_canDetect)
        {
            if (m_player.transform.position.x > transform.position.x - m_fixValue
            && m_player.transform.position.x < transform.position.x + m_fixValue)
            {
                Debug.Log("Player IN");
                m_canDetect = false;
            }
        }
        
    }

}
