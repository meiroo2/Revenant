using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ValueManipulator : MonoBehaviour
{
    [Header("플레이어 수정값")]
    public int Hp = 50;
    public float StunInvincibleTime = 2f;
    public float Speed = 1f;
    public float BackSpeedRatio = 0.7f;
    public float RunSpeedRatio = 1.5f;
    public float RollSpeedRatio = 2f;
    public int RollCountMax = 3;
    public float RollRecoverTime = 2f;

    private void Start()
    {
        GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.InitPlayerValue(this);
    }

    public void SetPlayerValues() { GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.InitPlayerValue(this); }
}