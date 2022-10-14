using System;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class JitterIssue : MonoBehaviour
{
    public bool p_DoPositionCorrection = false;
    private Player m_Player;

    private void Start()
    {
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }

    private void Update()
    {
        transform.position = m_Player.transform.position;

        if (p_DoPositionCorrection)
            transform.position = new Vector2(Mathf.Round(transform.position.x * 100) * 0.01f,
                Mathf.Round(transform.position.y * 100) * 0.01f);
    }
}