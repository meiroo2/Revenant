using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ValueManipulator : MonoBehaviour
{
    [Header("�÷��̾� ������")]
    public int Hp = 50;
    public float StunInvincibleTime = 2f;
    public float Speed = 1f;
    public float BackSpeedRatio = 0.7f;
    public float RunSpeedRatio = 1.5f;
    public float RollSpeedRatio = 2f;
    public int RollCountMax = 3;
    public float RollRecoverTime = 2f;
    public float RollCountRecoverSpeed = 1f;

    [Space(20f)]
    [Header("Negotiator(����) ������")]
    public float BulletSpeed = 15;
    public int BulletDamage = 10;
    public int StunValue = 1;
    public float MinimumShotDelay = 0.1f;
    public int BulletCount = 150;
    public int MagCount = 10;
    public float ReloadSpeed = 1f;

    [Space(20f)] [Header("UI ������")] 
    public float HitmarkRemainTime = 0.2f;

    private void Start()
    {
        InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.InitPlayerValue(this);
    }

    public void SetPlayerValues()
    {
        InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.InitPlayerValue(this);
    }
}