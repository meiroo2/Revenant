using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class CW_EnemyManipulator : OdinEditorWindow
{
    private const float m_LabelWidth = 150f;
    
    [MenuItem(("에디터/적 변수 수정기"))]
    private static void OpenWindow()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        
        TransferNormalGangValues(false);
        TransferMeleeGangValues(false);
        TransferDroneGangValues(false);
        TransferShieldGangValues(false);
        
        GetWindow<CW_EnemyManipulator>().Show();
    }

    private void OnBecameVisible()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferNormalGangValues(false);
        TransferMeleeGangValues(false);
        TransferDroneGangValues(false);
        TransferShieldGangValues(false);
    }

    private void OnProjectChange()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferNormalGangValues(false);
        TransferMeleeGangValues(false);
        TransferDroneGangValues(false);
        TransferShieldGangValues(false);
    }
    

    #region NormalGang

    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int N_HP;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int N_BulletDamage;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_BulletSpeed;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_BulletRandomRotation;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_FireDelay;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_Speed;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int N_StunThreshold;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_Vision_Distance;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_GunFire_Distance;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_MeleeAttack_Distance;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_AlertSpeedMulti;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float N_StunAlertSpeedMulti;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int N_HeadDmgMulti;
    [TabGroup("NormalGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int N_BodyDmgMulti;

    #endregion


    #region MeleeGang
    
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int M_HP;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int M_StunThreshold;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int M_MeleeDamage;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float M_Speed;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float M_Vision_Distance;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float M_MeleeAttack_Distance;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Range(0.0f, 1.0f)]
    public static float M_PointAttackTime;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int M_HeadDmgMulti;
    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int M_BodyDmgMulti;

    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float M_FollowSpeedMulti;

    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float M_DelayAfterAttack;

    [TabGroup("MeleeGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float M_StunWaitTime;

    #endregion
    

    #region DroneGang

    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int D_HP;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int D_BombDamage;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_BombRadius;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_BreakPower;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_Speed;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_RushSpeedMulti;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_RushTriggerDistance;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int D_DroneDmgMulti;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int D_BombDmgMulti;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_DetectSpeed;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float D_VisionDistance;
    [TabGroup("DroneGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Range(0f, 1f)] public static float D_DecidePositionPointTime;
    
    #endregion
    

    #region ShieldGang

    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static int S_HP;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static int S_ShieldHp;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static int S_MeleeDamage;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static float S_Speed;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static float S_BackSpeedMulti;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static float S_BrokenSpeedMulti;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static float S_VisionDistance;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static float S_AttackDistance;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]  
    public static float S_GapDistance;

    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float S_AtkAniSpeedMulti;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Range(0.0f, 1.0f)] 
    public static float S_PointAtkTime;
    
    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static float S_AtkHoldTime;

    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static int S_ShieldDmgMulti;

    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static int S_HeadDmgMulti;

    [TabGroup("ShieldGang"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] 
    public static int S_BodyDmgMulti;

    #endregion
   
    
    
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("NormalGang")]
    private void NormalGang_적용하기()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferNormalGangValues(true);
        
        enemyMgr.SetNormalGangs();
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("MeleeGang")]
    private void MeleeGang_적용하기()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferMeleeGangValues(true);
        
        enemyMgr.SetMeleeGangs();
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("DroneGang")]
    private void DroneGang_적용하기()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferDroneGangValues(true);
        
        enemyMgr.SetDrones();
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("ShieldGang")]
    private void ShieldGang_적용하기()
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();
        TransferShieldGangValues(true);
        
        enemyMgr.SetShieldGangs();
    }

    /// <summary>
    /// 각종 NormalGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferNormalGangValues(bool _toEnemyMgr)
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();

        if (_toEnemyMgr)
        {
            enemyMgr.N_HP = N_HP;
            enemyMgr.N_BulletDamage = N_BulletDamage;
            enemyMgr.N_BulletSpeed = N_BulletSpeed;
            enemyMgr.N_BulletRandomRotation = N_BulletRandomRotation;
            enemyMgr.N_FireDelay = N_FireDelay;
            enemyMgr.N_Speed = N_Speed;
            enemyMgr.N_StunThreshold = N_StunThreshold;
            enemyMgr.N_Vision_Distance = N_Vision_Distance;
            enemyMgr.N_GunFire_Distance = N_GunFire_Distance;
            enemyMgr.N_MeleeAttack_Distance = N_MeleeAttack_Distance;
            enemyMgr.N_AlertSpeedMulti = N_AlertSpeedMulti;
            enemyMgr.N_StunAlertSpeedMulti = N_StunAlertSpeedMulti;
            enemyMgr.N_HeadDmgMulti = N_HeadDmgMulti;
            enemyMgr.N_BodyDmgMulti = N_BodyDmgMulti;
        }
        else
        {
            N_HP = enemyMgr.N_HP;
            N_BulletDamage = enemyMgr.N_BulletDamage;
            N_BulletSpeed = enemyMgr.N_BulletSpeed;
            N_BulletRandomRotation = enemyMgr.N_BulletRandomRotation;
            N_FireDelay = enemyMgr.N_FireDelay;
            N_Speed = enemyMgr.N_Speed;
            N_StunThreshold = enemyMgr.N_StunThreshold;
            N_Vision_Distance = enemyMgr.N_Vision_Distance;
            N_GunFire_Distance = enemyMgr.N_GunFire_Distance;
            N_MeleeAttack_Distance = enemyMgr.N_MeleeAttack_Distance;
            N_AlertSpeedMulti = enemyMgr.N_AlertSpeedMulti;
            N_StunAlertSpeedMulti = enemyMgr.N_StunAlertSpeedMulti;
            N_HeadDmgMulti = enemyMgr.N_HeadDmgMulti;
            N_BodyDmgMulti = enemyMgr.N_BodyDmgMulti;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 MeleeGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferMeleeGangValues(bool _toEnemyMgr)
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();

        if (_toEnemyMgr)
        {
            enemyMgr.M_HP = M_HP;
            enemyMgr.M_StunThreshold = M_StunThreshold;
            enemyMgr.M_MeleeDamage = M_MeleeDamage;
            enemyMgr.M_Speed = M_Speed;
            enemyMgr.M_Vision_Distance = M_Vision_Distance;
            enemyMgr.M_MeleeAttack_Distance = M_MeleeAttack_Distance;
            enemyMgr.M_PointAttackTime = M_PointAttackTime;
            enemyMgr.M_HeadDmgMulti = M_HeadDmgMulti;
            enemyMgr.M_BodyDmgMulti = M_BodyDmgMulti;
            enemyMgr.M_FollowSpeedMulti = M_FollowSpeedMulti;
            enemyMgr.M_DelayAfterAttack = M_DelayAfterAttack;
            enemyMgr.M_StunWaitTime = M_StunWaitTime;
        }
        else
        {
            M_HP = enemyMgr.M_HP;
            M_StunThreshold = enemyMgr.M_StunThreshold;
            M_MeleeDamage = enemyMgr.M_MeleeDamage;
            M_Speed = enemyMgr.M_Speed;
            M_Vision_Distance = enemyMgr.M_Vision_Distance;
            M_MeleeAttack_Distance = enemyMgr.M_MeleeAttack_Distance;
            M_PointAttackTime = enemyMgr.M_PointAttackTime;
            M_HeadDmgMulti = enemyMgr.M_HeadDmgMulti;
            M_BodyDmgMulti = enemyMgr.M_BodyDmgMulti;
            M_FollowSpeedMulti = enemyMgr.M_FollowSpeedMulti;
            M_DelayAfterAttack = enemyMgr.M_DelayAfterAttack;
            M_StunWaitTime = enemyMgr.M_StunWaitTime;
        }

    #if UNITY_EDITOR
            EditorUtility.SetDirty(enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 Drone 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferDroneGangValues(bool _toEnemyMgr)
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();

        if (_toEnemyMgr)
        {
            enemyMgr.D_HP = D_HP;
            enemyMgr.D_BombDamage = D_BombDamage;
            enemyMgr.D_BombRadius = D_BombRadius;
            enemyMgr.D_BreakPower = D_BreakPower;
            enemyMgr.D_Speed = D_Speed;
            enemyMgr.D_RushSpeedMulti = D_RushSpeedMulti;
            enemyMgr.D_RushTriggerDistance = D_RushTriggerDistance;
            enemyMgr.D_DroneDmgMulti = D_DroneDmgMulti;
            enemyMgr.D_BombDmgMulti = D_BombDmgMulti;
            enemyMgr.D_DetectSpeed = D_DetectSpeed;
            enemyMgr.D_VisionDistance = D_VisionDistance;
            enemyMgr.D_DecidePositionPointTime = D_DecidePositionPointTime;
        }
        else
        {
            D_HP = enemyMgr.D_HP;
            D_BombDamage = enemyMgr.D_BombDamage;
            D_BombRadius = enemyMgr.D_BombRadius;
            D_BreakPower = enemyMgr.D_BreakPower;
            D_Speed = enemyMgr.D_Speed;
            D_RushSpeedMulti = enemyMgr.D_RushSpeedMulti;
            D_RushTriggerDistance = enemyMgr.D_RushTriggerDistance;
            D_DroneDmgMulti = enemyMgr.D_DroneDmgMulti;
            D_BombDmgMulti = enemyMgr.D_BombDmgMulti;
            D_DetectSpeed = enemyMgr.D_DetectSpeed;
            D_VisionDistance = enemyMgr.D_VisionDistance;
            D_DecidePositionPointTime = enemyMgr.D_DecidePositionPointTime;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 ShieldGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferShieldGangValues(bool _toEnemyMgr)
    {
        var enemyMgr = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<EnemyMgr>();

        if (_toEnemyMgr)
        {
            enemyMgr.S_HP = S_HP;
            enemyMgr.S_ShieldHp = S_ShieldHp;
            enemyMgr.S_MeleeDamage = S_MeleeDamage;
            enemyMgr.S_Speed = S_Speed;
            enemyMgr.S_BackSpeedMulti = S_BackSpeedMulti;
            enemyMgr.S_BrokenSpeedMulti = S_BrokenSpeedMulti;
            enemyMgr.S_VisionDistance = S_VisionDistance;
            enemyMgr.S_AttackDistance = S_AttackDistance;
            enemyMgr.S_GapDistance = S_GapDistance;
            enemyMgr.S_AtkAniSpeedMulti = S_AtkAniSpeedMulti;
            enemyMgr.S_PointAtkTime = S_PointAtkTime;
            enemyMgr.S_AtkHoldTime = S_AtkHoldTime;
            enemyMgr.S_ShieldDmgMulti = S_ShieldDmgMulti;
            enemyMgr.S_HeadDmgMulti = S_HeadDmgMulti;
            enemyMgr.S_BodyDmgMulti = S_BodyDmgMulti;
        }
        else
        {
            S_HP = enemyMgr.S_HP;
            S_ShieldHp = enemyMgr.S_ShieldHp;
            S_MeleeDamage = enemyMgr.S_MeleeDamage;
            S_Speed = enemyMgr.S_Speed;
            S_BackSpeedMulti = enemyMgr.S_BackSpeedMulti;
            S_BrokenSpeedMulti = enemyMgr.S_BrokenSpeedMulti;
            S_VisionDistance = enemyMgr.S_VisionDistance;
            S_AttackDistance = enemyMgr.S_AttackDistance;
            S_GapDistance = enemyMgr.S_GapDistance;
            S_AtkAniSpeedMulti = enemyMgr.S_AtkAniSpeedMulti;
            S_PointAtkTime = enemyMgr.S_PointAtkTime;
            S_AtkHoldTime = enemyMgr.S_AtkHoldTime;
            S_ShieldDmgMulti = enemyMgr.S_ShieldDmgMulti;
            S_HeadDmgMulti = enemyMgr.S_HeadDmgMulti;
            S_BodyDmgMulti = enemyMgr.S_BodyDmgMulti;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(enemyMgr);
        #endif
    }


    private void DataSaveNLoad()
    {
        TransferNormalGangValues(true);
        TransferMeleeGangValues(true);
        TransferDroneGangValues(true);
        TransferShieldGangValues(true);
        
        TransferNormalGangValues(false);
        TransferMeleeGangValues(false);
        TransferDroneGangValues(false);
        TransferShieldGangValues(false);
    }
}