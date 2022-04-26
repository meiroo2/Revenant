using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Light : MonoBehaviour
{
    private Transform m_Player;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
    }

    private void Update()
    {
        transform.position = m_Player.position;
    }
}
