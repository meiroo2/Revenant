using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManipulator : MonoBehaviour
{
    private Player m_Player;

    [Header("플레이어 설정값")]
    public int P_HP = 50;
    public float P_StunInvincibleTime = 2f;
    public float P_Speed = 1f;
    public float P_BackSpeedMulti = 0.7f;
    public float P_RollSpeedMulti = 2f;
    public float P_MeleeSpeedMulti = 2f;
    public int P_MeleeDamage = 10;
    public int P_MeleeStunValue = 10;
    public float P_JustEvadeStartTime = 0f;
    public float P_JustEvadeEndTime = 1f;
    public float P_RollDecelerationSpeed = 1f;
    public float P_ReloadSpeed = 1f;

    [Space(20f)]
    [Header("Negotiator 설정값")]
    public int N_Damage = 10;
    public int N_StunValue = 1;
    public float N_MinFireDelay = 0.1f;


    // Constructor
    private void Awake()
    {
        Debug.Log("PlayerManipulator Awake");
        SetPlayer(false);
        SetNegotiator(false);
    }
    

    public void SetPlayer(bool _isEditor)
    {
        GameObject findPlayer = null;
        findPlayer = GameObject.FindGameObjectWithTag("@Player");

        if (ReferenceEquals(findPlayer, null))
        {
            Debug.Log("ERR : PlayerManipulator_Player Null");
            return;
        }

        if (findPlayer.TryGetComponent(out Player player))
        {
            m_Player = player;
            m_Player.SetPlayer(this, _isEditor);
        }
    }

    public void SetNegotiator(bool _isEditor)
    {
        GameObject findPlayer = null;
        findPlayer = GameObject.FindGameObjectWithTag("@Player");

        if (ReferenceEquals(findPlayer, null))
        {
            Debug.Log("ERR : PlayerManipulator_Player Null");
            return;
        }

        if (findPlayer.TryGetComponent(out Player player))
        {
            m_Player = player;
            m_Player.SetNegotiator(this, _isEditor);
        }
    }
}