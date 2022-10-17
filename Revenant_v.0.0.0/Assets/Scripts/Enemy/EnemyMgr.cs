using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyMgr : MonoBehaviour
{
    // Visible Member Variables
    [Header("원거리 적 변수 목록")] 
    public int N_HP;
    public int N_BulletDamage;
    public float N_BulletSpeed;
    public float N_BulletRandomRotation;
    public float N_FireDelay;
    public float N_Speed;
    public int N_StunThreshold;
    public float N_Vision_Distance;
    public float N_GunFire_Distance;
    public float N_MeleeAttack_Distance;
    public float N_AlertSpeedMulti;
    public float N_StunAlertSpeedMulti;
    public int N_HeadDmgMulti;
    public int N_BodyDmgMulti;
    
    [Space(10f)]
    [Header("근거리 적 변수 목록")] 
    public int M_HP;
    public int M_StunThreshold;
    public int M_MeleeDamage;
    public float M_Speed;
    public float M_Vision_Distance;
    public float M_MeleeAttack_Distance;
    [Range(0.0f, 1.0f)] public float M_PointAttackTime;
    public int M_HeadDmgMulti;
    public int M_BodyDmgMulti;
    public float M_FollowSpeedMulti;
    public float M_DelayAfterAttack;
    public float M_StunWaitTime;
    
    [Space(10f)]
    [Header("드론 변수 목록")] 
    public int D_HP;
    public int D_BombDamage;
    public float D_BombRadius;
    public float D_BreakPower;
    public float D_Speed;
    public float D_RushSpeedMulti;
    public float D_RushTriggerDistance;
    public int D_DroneDmgMulti;
    public int D_BombDmgMulti;
    public float D_DetectSpeed;
    public float D_VisionDistance;
    public float D_DecidePositionPointTime;

    [Space(10f)] 
    [Header("방패적 변수 목록")] 
    public int S_HP;
    public int S_ShieldHp;
    public int S_MeleeDamage;
    public float S_Speed;
    public float S_BackSpeedMulti;
    public float S_BrokenSpeedMulti;
    public float S_VisionDistance;
    public float S_AttackDistance;
    public float S_GapDistance;
    public float S_AtkAniSpeedMulti;
    [Range(0.0f, 1.0f)] public float S_PointAtkTime;
    public float S_AtkHoldTime;
    public int S_ShieldDmgMulti;
    public int S_HeadDmgMulti;
    public int S_BodyDmgMulti;
    

    // Member Variables
    private List<NormalGang> m_NormalGangs = new List<NormalGang>();
    private List<MeleeGang> m_MeleeGangs = new List<MeleeGang>();
    private List<Drone> m_Drones = new List<Drone>();
    
    
    // Constructors


    // Functions
    [Button]
    public void StickToFloor()
    {
        RaycastHit2D cast;
        int m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
        
        NormalGang[] tempNGangs = FindObjectsOfType<NormalGang>();
        for (int i = 0; i < tempNGangs.Length; i++)
        {
            cast = Physics2D.Raycast(tempNGangs[i].transform.position, -transform.up, 1f, m_LayerMask);
            tempNGangs[i].transform.position = new Vector2(cast.point.x, cast.point.y + 0.64f);
        }
        
        MeleeGang[] tempMGangs = FindObjectsOfType<MeleeGang>();
        for (int i = 0; i < tempMGangs.Length; i++)
        {
            cast = Physics2D.Raycast(tempMGangs[i].transform.position, -transform.up, 1f, m_LayerMask);
            tempMGangs[i].transform.position = new Vector2(cast.point.x, cast.point.y + 0.64f);
        }
    }
    
    public void LoadMeleeGangData()
    {
        
    }

    public void SetAllEnemys()
    {
        SetNormalGangs();
        SetMeleeGangs();
        SetDrones();
        
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }
    
    public void SetNormalGangs()
    {
        NormalGang[] temp = FindObjectsOfType<NormalGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }

    public void SetMeleeGangs()
    {
        MeleeGang[] temp = FindObjectsOfType<MeleeGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }

    public void SetDrones()
    {
        Drone[] temp = FindObjectsOfType<Drone>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }

    public void SetShieldGangs()
    {
        ShieldGang[] temp = FindObjectsOfType<ShieldGang>();
        foreach (var VARIABLE in temp)
        {
            VARIABLE.SetEnemyValues(this);
        }
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }
}