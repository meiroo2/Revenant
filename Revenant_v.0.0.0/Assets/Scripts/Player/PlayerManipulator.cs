using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManipulator : MonoBehaviour
{
    public Player p_Player;
    public RageGauge_UI p_RageGauge;
    
    [Header("플레이어 설정값")]
    public int P_HP = 50;
    public float P_StunInvincibleTime = 2f;
    public float P_Speed = 1f;
    public float P_BackSpeedMulti = 0.7f;
    public float P_RollSpeedMulti = 2f;
    public float P_MeleeSpeedMulti = 2f;
    
    [Space(20f)]
    [Header("Negotiator 설정값")]
    public int N_Damage = 10;
    public int N_StunValue = 1;
    public float N_MinFireDelay = 0.1f;
    public int N_MaxBullet = 150;
    public int N_MaxMag = 10;
    public float N_ReloadSpeed = 1f;

    [Space(20f)] [Header("RageGauge 설정값")] 
    public float R_Gauge_Max;
    public float R_Gauge_Refill_Nature;
    public float R_Gauge_Refill_Attack_Multi;
    public float R_Gauge_Refill_Evade;
    public float R_Gauge_Refill_JustEvade;
    public float R_Gauge_Refill_Limit;
    public float R_Gauge_Consume_Nature;
    public float R_Gauge_Consume_Roll;
    public float R_Gauge_Consume_Melee;
    
    
    private void Start()
    {
        InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.
           SetPlayer(this);
    }

    public void SetPlayer()
    {
        p_Player.SetPlayer(this);
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_Player);
        #endif
    }

    public void SetNegotiator()
    {
        p_Player.SetNegotiator(this);
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_Player);
        #endif
    }

    public void SetRageGauge()
    {
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_RageGauge);
        #endif
    }
}