using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EnemyMgr : MonoBehaviour
{
    // Visible Member Variables
    [Header("원거리 적 변수 목록")] 
    public int N_HP;
    public float N_Speed;
    public float N_StunTime;
    public int N_StunThreshold;
    public float N_Vision_Distance;
    public float N_GunFire_Distance;
    public float N_MeleeAttack_Distance;
    public float N_AlertSpeedRatio;
    
    [Space(10f)]
    [Header("근거리 적 변수 목록")] 
    public int M_HP;
    public float M_Speed;
    public float M_StunTime;
    public int M_StunThreshold;
    public float M_Vision_Distance;
    public float M_MeleeAttack_Distance;
    [Range(0.0f, 1.0f)] public float M_PointAttackTime;
    
    [Space(10f)]
    [Header("드론 변수 목록")] 
    public int D_HP;
    public float D_Speed;
    public float D_StunTime;
    public int D_StunThreshold;
    public float D_AlertSpeedRatio;
    public float D_RushSpeedRatio;
    public float D_ToRush_Distance;
    public float D_WiggleSpeed;
    public float D_WigglePower;
    public float D_BombRadius;


    // Member Variables
    private List<NormalGang> m_NormalGangs = new List<NormalGang>();
    private List<MeleeGang> m_MeleeGangs = new List<MeleeGang>();
    private List<Drone> m_Drones = new List<Drone>();
    
    
    // Constructors


    // Functions
    public void SetAllEnemys()
    {
        SetNoramlGangs();
        SetMeleeGangs();
        SetDrones();
        
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }
    
    public void SetNoramlGangs()
    {
        NormalGang[] temp = FindObjectsOfType<NormalGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }

    public void SetMeleeGangs()
    {
        MeleeGang[] temp = FindObjectsOfType<MeleeGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }

    public void SetDrones()
    {
        Drone[] temp = FindObjectsOfType<Drone>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }
}