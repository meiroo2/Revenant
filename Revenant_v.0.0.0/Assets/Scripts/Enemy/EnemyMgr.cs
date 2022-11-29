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

    [Space(10f)] [Header("특수부대 변수 목록")]
    public int SF_Hp;
    public int SF_BulletDamage;
    public float SF_BulletSpeed;
    public float SF_BulletSpread;
    public float SF_FireAnimSpeed;
    public float SF_FireDelay;
    public float SF_MoveSpeed;
    public float SF_RunSpeedMulti;
    public float SF_StunAlertSpeed;
    public int SF_StunThreshold;
    public float SF_VisionDistance;
    public float SF_AttackDistance;
    public float SF_MeleeRollDistance;
    public float SF_GapDistance;
    public int SF_HeadDmgMulti;
    public int SF_BodyDmgMulti;
    public float SF_Roll_Refresh;
    public Vector2 SF_Roll_Tick;
    public int SF_Roll_Chance;
    public float SF_Roll_Cooldown;
    public float SF_Roll_Speed_Multi;
    public float SF_AlertSpeed;
    public float SF_AlertFadeSpeed;
    

    // Member Variables


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

    public void SetNormalGangs()
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

    public void SetShieldGangs()
    {
        ShieldGang[] temp = FindObjectsOfType<ShieldGang>();
        foreach (var VARIABLE in temp)
        {
            VARIABLE.SetEnemyValues(this);
        }
    }

    public void SetSpecialForce()
    {
        SpecialForce[] temp = FindObjectsOfType<SpecialForce>();
        foreach (var VARIABLE in temp)
        {
            VARIABLE.SetEnemyValues(this);
        }
    }
}