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
    public int N_MaxBullet = 150;
    public int N_MaxMag = 10;
    public float N_ReloadSpeed = 1f;
    
    

    public void SetPlayer()
    {
        m_Player = null;
        m_Player = GameObject.FindGameObjectWithTag("@Player").GetComponent<Player>();
        if (ReferenceEquals(m_Player, null))
        {
            Debug.Log("ERR : PlayerManipulator_Player Null");
        }
        
        m_Player.SetPlayer(this);
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(m_Player);
        #endif
    }

    public void SetNegotiator()
    {
        m_Player.SetNegotiator(this);
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(m_Player);
        #endif
    }
}