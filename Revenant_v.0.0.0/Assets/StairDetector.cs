using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairDetector : MonoBehaviour
{
    public bool m_isUpDetector;
    public bool m_isOn { get; set; } = false;

    private Player m_Player;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }
    public bool useObj()
    {
        m_Player.gameObject.layer = 10;
        m_Player.transform.position = transform.position;
        return true;
    }
}
