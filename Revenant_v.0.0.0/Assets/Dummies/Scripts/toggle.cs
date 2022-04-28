using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggle : MonoBehaviour
{
    public GameObject m_Player;
    public bool m_isStuckMove = true;
    private Moveable m_Moveable;

    private void Awake()
    {
        m_Moveable = m_Player.GetComponent<Moveable>();
    }

    private void Update()
    {
        m_isStuckMove = !this.GetComponent<Toggle>().isOn;
        //m_Moveable.m_SetMoveStuck = m_isStuckMove;
    }
}
