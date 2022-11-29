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
    // Const Variables
    private const float m_LabelWidth = 150f;
    
    
    // Member Variables
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
   
    
    #region SpecialForce

    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_Hp;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_BulletDamage;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_BulletSpeed;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_BulletSpread;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_FireAnimSpeed;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_FireDelay;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_MoveSpeed;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_RunSpeedMulti;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_StunAlertSpeed;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_StunThreshold;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_VisionDistance;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_AttackDistance;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_MeleeRollDistance;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_GapDistance;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_HeadDmgMulti;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_BodyDmgMulti;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_AlertSpeed;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_AlertFadeSpeed;
    
    // Roll Values
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Title("Roll Values")]
    public static float SF_Roll_Refresh;

    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_Roll_Tick_Min;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_Roll_Tick_Max;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static int SF_Roll_Chance;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_Roll_Cooldown;
    [TabGroup("SpecialForce"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)] public static float SF_Roll_Speed_Multi;
    
    
    #endregion
    
    
    
    // Constructors
    [MenuItem(("에디터/적 변수 수정기"))]
    private static void OpenWindow()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferNormalGangValues(enemyMgr, false);
        TransferMeleeGangValues(enemyMgr,false);
        TransferDroneGangValues(enemyMgr,false);
        TransferShieldGangValues(enemyMgr,false);
        TransferSpecialForceValues(enemyMgr, false);
        
        GetWindow<CW_EnemyManipulator>().Show();
    }
    
    
    // Functions
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("NormalGang")]
    private void NormalGang_적용하기()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferNormalGangValues(enemyMgr,true);
        enemyMgr.SetNormalGangs();
        
        PrefabUtility.RevertPrefabInstance(GameObject.FindObjectOfType<GameMgr>().gameObject,
            InteractionMode.UserAction);
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("MeleeGang")]
    private void MeleeGang_적용하기()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferMeleeGangValues(enemyMgr, true);
        enemyMgr.SetMeleeGangs();
        
        PrefabUtility.RevertPrefabInstance(GameObject.FindObjectOfType<GameMgr>().gameObject,
            InteractionMode.UserAction);
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("DroneGang")]
    private void DroneGang_적용하기()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferDroneGangValues(enemyMgr, true);
        enemyMgr.SetDrones();
        
        PrefabUtility.RevertPrefabInstance(GameObject.FindObjectOfType<GameMgr>().gameObject,
            InteractionMode.UserAction);
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("ShieldGang")]
    private void ShieldGang_적용하기()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferShieldGangValues(enemyMgr, true);
        enemyMgr.SetShieldGangs();
        
        PrefabUtility.RevertPrefabInstance(GameObject.FindObjectOfType<GameMgr>().gameObject,
            InteractionMode.UserAction);
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("SpecialForce")]
    private void SpecialForce_적용하기()
    {
        var enemyMgr = Resources.Load<GameMgr>("GameMgr").p_EnemyMgr;
        
        TransferSpecialForceValues(enemyMgr, true);
        enemyMgr.SetSpecialForce();

        PrefabUtility.RevertPrefabInstance(GameObject.FindObjectOfType<GameMgr>().gameObject,
            InteractionMode.UserAction);
    }

    /// <summary>
    /// 각종 NormalGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_enemyMgr"></param>
    /// <param name="_toEnemyMgr"></param>
    private static void TransferNormalGangValues(EnemyMgr _enemyMgr, bool _toEnemyMgr)
    {
        if (_toEnemyMgr)
        {
            _enemyMgr.N_HP = N_HP;
            _enemyMgr.N_BulletDamage = N_BulletDamage;
            _enemyMgr.N_BulletSpeed = N_BulletSpeed;
            _enemyMgr.N_BulletRandomRotation = N_BulletRandomRotation;
            _enemyMgr.N_FireDelay = N_FireDelay;
            _enemyMgr.N_Speed = N_Speed;
            _enemyMgr.N_StunThreshold = N_StunThreshold;
            _enemyMgr.N_Vision_Distance = N_Vision_Distance;
            _enemyMgr.N_GunFire_Distance = N_GunFire_Distance;
            _enemyMgr.N_MeleeAttack_Distance = N_MeleeAttack_Distance;
            _enemyMgr.N_AlertSpeedMulti = N_AlertSpeedMulti;
            _enemyMgr.N_StunAlertSpeedMulti = N_StunAlertSpeedMulti;
            _enemyMgr.N_HeadDmgMulti = N_HeadDmgMulti;
            _enemyMgr.N_BodyDmgMulti = N_BodyDmgMulti;
        }
        else
        {
            N_HP = _enemyMgr.N_HP;
            N_BulletDamage = _enemyMgr.N_BulletDamage;
            N_BulletSpeed = _enemyMgr.N_BulletSpeed;
            N_BulletRandomRotation = _enemyMgr.N_BulletRandomRotation;
            N_FireDelay = _enemyMgr.N_FireDelay;
            N_Speed = _enemyMgr.N_Speed;
            N_StunThreshold = _enemyMgr.N_StunThreshold;
            N_Vision_Distance = _enemyMgr.N_Vision_Distance;
            N_GunFire_Distance = _enemyMgr.N_GunFire_Distance;
            N_MeleeAttack_Distance = _enemyMgr.N_MeleeAttack_Distance;
            N_AlertSpeedMulti = _enemyMgr.N_AlertSpeedMulti;
            N_StunAlertSpeedMulti = _enemyMgr.N_StunAlertSpeedMulti;
            N_HeadDmgMulti = _enemyMgr.N_HeadDmgMulti;
            N_BodyDmgMulti = _enemyMgr.N_BodyDmgMulti;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 MeleeGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferMeleeGangValues(EnemyMgr _enemyMgr, bool _toEnemyMgr)
    {
        if (_toEnemyMgr)
        {
            _enemyMgr.M_HP = M_HP;
            _enemyMgr.M_StunThreshold = M_StunThreshold;
            _enemyMgr.M_MeleeDamage = M_MeleeDamage;
            _enemyMgr.M_Speed = M_Speed;
            _enemyMgr.M_Vision_Distance = M_Vision_Distance;
            _enemyMgr.M_MeleeAttack_Distance = M_MeleeAttack_Distance;
            _enemyMgr.M_PointAttackTime = M_PointAttackTime;
            _enemyMgr.M_HeadDmgMulti = M_HeadDmgMulti;
            _enemyMgr.M_BodyDmgMulti = M_BodyDmgMulti;
            _enemyMgr.M_FollowSpeedMulti = M_FollowSpeedMulti;
            _enemyMgr.M_DelayAfterAttack = M_DelayAfterAttack;
            _enemyMgr.M_StunWaitTime = M_StunWaitTime;
        }
        else
        {
            M_HP = _enemyMgr.M_HP;
            M_StunThreshold = _enemyMgr.M_StunThreshold;
            M_MeleeDamage = _enemyMgr.M_MeleeDamage;
            M_Speed = _enemyMgr.M_Speed;
            M_Vision_Distance = _enemyMgr.M_Vision_Distance;
            M_MeleeAttack_Distance = _enemyMgr.M_MeleeAttack_Distance;
            M_PointAttackTime = _enemyMgr.M_PointAttackTime;
            M_HeadDmgMulti = _enemyMgr.M_HeadDmgMulti;
            M_BodyDmgMulti = _enemyMgr.M_BodyDmgMulti;
            M_FollowSpeedMulti = _enemyMgr.M_FollowSpeedMulti;
            M_DelayAfterAttack = _enemyMgr.M_DelayAfterAttack;
            M_StunWaitTime = _enemyMgr.M_StunWaitTime;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 Drone 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferDroneGangValues(EnemyMgr _enemyMgr, bool _toEnemyMgr)
    {
        if (_toEnemyMgr)
        {
            _enemyMgr.D_HP = D_HP;
            _enemyMgr.D_BombDamage = D_BombDamage;
            _enemyMgr.D_BombRadius = D_BombRadius;
            _enemyMgr.D_BreakPower = D_BreakPower;
            _enemyMgr.D_Speed = D_Speed;
            _enemyMgr.D_RushSpeedMulti = D_RushSpeedMulti;
            _enemyMgr.D_RushTriggerDistance = D_RushTriggerDistance;
            _enemyMgr.D_DroneDmgMulti = D_DroneDmgMulti;
            _enemyMgr.D_BombDmgMulti = D_BombDmgMulti;
            _enemyMgr.D_DetectSpeed = D_DetectSpeed;
            _enemyMgr.D_VisionDistance = D_VisionDistance;
            _enemyMgr.D_DecidePositionPointTime = D_DecidePositionPointTime;
        }
        else
        {
            D_HP = _enemyMgr.D_HP;
            D_BombDamage = _enemyMgr.D_BombDamage;
            D_BombRadius = _enemyMgr.D_BombRadius;
            D_BreakPower = _enemyMgr.D_BreakPower;
            D_Speed = _enemyMgr.D_Speed;
            D_RushSpeedMulti = _enemyMgr.D_RushSpeedMulti;
            D_RushTriggerDistance = _enemyMgr.D_RushTriggerDistance;
            D_DroneDmgMulti = _enemyMgr.D_DroneDmgMulti;
            D_BombDmgMulti = _enemyMgr.D_BombDmgMulti;
            D_DetectSpeed = _enemyMgr.D_DetectSpeed;
            D_VisionDistance = _enemyMgr.D_VisionDistance;
            D_DecidePositionPointTime = _enemyMgr.D_DecidePositionPointTime;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_enemyMgr);
        #endif
    }
    
    
    /// <summary>
    /// 각종 ShieldGang 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toEnemyMgr">True시 EnemyMgr로 전송</param>
    private static void TransferShieldGangValues(EnemyMgr _enemyMgr, bool _toEnemyMgr)
    {
        if (_toEnemyMgr)
        {
            _enemyMgr.S_HP = S_HP;
            _enemyMgr.S_ShieldHp = S_ShieldHp;
            _enemyMgr.S_MeleeDamage = S_MeleeDamage;
            _enemyMgr.S_Speed = S_Speed;
            _enemyMgr.S_BackSpeedMulti = S_BackSpeedMulti;
            _enemyMgr.S_BrokenSpeedMulti = S_BrokenSpeedMulti;
            _enemyMgr.S_VisionDistance = S_VisionDistance;
            _enemyMgr.S_AttackDistance = S_AttackDistance;
            _enemyMgr.S_GapDistance = S_GapDistance;
            _enemyMgr.S_AtkAniSpeedMulti = S_AtkAniSpeedMulti;
            _enemyMgr.S_PointAtkTime = S_PointAtkTime;
            _enemyMgr.S_AtkHoldTime = S_AtkHoldTime;
            _enemyMgr.S_ShieldDmgMulti = S_ShieldDmgMulti;
            _enemyMgr.S_HeadDmgMulti = S_HeadDmgMulti;
            _enemyMgr.S_BodyDmgMulti = S_BodyDmgMulti;
        }
        else
        {
            S_HP = _enemyMgr.S_HP;
            S_ShieldHp = _enemyMgr.S_ShieldHp;
            S_MeleeDamage = _enemyMgr.S_MeleeDamage;
            S_Speed = _enemyMgr.S_Speed;
            S_BackSpeedMulti = _enemyMgr.S_BackSpeedMulti;
            S_BrokenSpeedMulti = _enemyMgr.S_BrokenSpeedMulti;
            S_VisionDistance = _enemyMgr.S_VisionDistance;
            S_AttackDistance = _enemyMgr.S_AttackDistance;
            S_GapDistance = _enemyMgr.S_GapDistance;
            S_AtkAniSpeedMulti = _enemyMgr.S_AtkAniSpeedMulti;
            S_PointAtkTime = _enemyMgr.S_PointAtkTime;
            S_AtkHoldTime = _enemyMgr.S_AtkHoldTime;
            S_ShieldDmgMulti = _enemyMgr.S_ShieldDmgMulti;
            S_HeadDmgMulti = _enemyMgr.S_HeadDmgMulti;
            S_BodyDmgMulti = _enemyMgr.S_BodyDmgMulti;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_enemyMgr);
        #endif
    }

    /// <summary>
    /// 각종 SpecialForce 데이터를 EnemyMgr로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_enemyMgr"></param>
    /// <param name="_toEnemyMgr"></param>
    private static void TransferSpecialForceValues(EnemyMgr _enemyMgr, bool _toEnemyMgr)
    {
        if (_toEnemyMgr)
        {
            _enemyMgr.SF_Hp = SF_Hp;
            _enemyMgr.SF_BulletDamage = SF_BulletDamage;
            _enemyMgr.SF_BulletSpeed = SF_BulletSpeed;
            _enemyMgr.SF_BulletSpread = SF_BulletSpread;
            _enemyMgr.SF_FireAnimSpeed = SF_FireAnimSpeed;
            _enemyMgr.SF_FireDelay = SF_FireDelay;
            _enemyMgr.SF_MoveSpeed = SF_MoveSpeed;
            _enemyMgr.SF_RunSpeedMulti = SF_RunSpeedMulti;

            _enemyMgr.SF_StunAlertSpeed = SF_StunAlertSpeed;
            _enemyMgr.SF_StunThreshold = SF_StunThreshold;
            _enemyMgr.SF_VisionDistance = SF_VisionDistance;
            _enemyMgr.SF_AttackDistance = SF_AttackDistance;
            _enemyMgr.SF_MeleeRollDistance = SF_MeleeRollDistance;
            _enemyMgr.SF_GapDistance = SF_GapDistance;
            _enemyMgr.SF_HeadDmgMulti = SF_HeadDmgMulti;
            _enemyMgr.SF_BodyDmgMulti = SF_BodyDmgMulti;
            
            _enemyMgr.SF_Roll_Refresh = SF_Roll_Refresh;
            _enemyMgr.SF_Roll_Tick.x = SF_Roll_Tick_Min;
            _enemyMgr.SF_Roll_Tick.y = SF_Roll_Tick_Max;
            _enemyMgr.SF_Roll_Chance = SF_Roll_Chance;
            _enemyMgr.SF_Roll_Cooldown = SF_Roll_Cooldown;
            _enemyMgr.SF_Roll_Speed_Multi = SF_Roll_Speed_Multi;

            _enemyMgr.SF_AlertSpeed = SF_AlertSpeed;
            _enemyMgr.SF_AlertFadeSpeed = SF_AlertFadeSpeed;
        }
        else
        {
            SF_Hp = _enemyMgr.SF_Hp;
            SF_BulletDamage = _enemyMgr.SF_BulletDamage;
            SF_BulletSpeed = _enemyMgr.SF_BulletSpeed;
            SF_BulletSpread = _enemyMgr.SF_BulletSpread;
            SF_FireAnimSpeed = _enemyMgr.SF_FireAnimSpeed;
            SF_FireDelay = _enemyMgr.SF_FireDelay;
            SF_MoveSpeed = _enemyMgr.SF_MoveSpeed;
            SF_RunSpeedMulti = _enemyMgr.SF_RunSpeedMulti;

            SF_StunAlertSpeed = _enemyMgr.SF_StunAlertSpeed;
            SF_StunThreshold = _enemyMgr.SF_StunThreshold;
            SF_VisionDistance = _enemyMgr.SF_VisionDistance;
            SF_AttackDistance = _enemyMgr.SF_AttackDistance;
            SF_MeleeRollDistance = _enemyMgr.SF_MeleeRollDistance;
            SF_GapDistance = _enemyMgr.SF_GapDistance;
            SF_HeadDmgMulti = _enemyMgr.SF_HeadDmgMulti;
            SF_BodyDmgMulti = _enemyMgr.SF_BodyDmgMulti;

            SF_Roll_Refresh = _enemyMgr.SF_Roll_Refresh;
            SF_Roll_Tick_Min = _enemyMgr.SF_Roll_Tick.x;
            SF_Roll_Tick_Max = _enemyMgr.SF_Roll_Tick.y;
            SF_Roll_Chance = _enemyMgr.SF_Roll_Chance;
            SF_Roll_Cooldown = _enemyMgr.SF_Roll_Cooldown;
            SF_Roll_Speed_Multi = _enemyMgr.SF_Roll_Speed_Multi;

            SF_AlertSpeed = _enemyMgr.SF_AlertSpeed;
            SF_AlertFadeSpeed = _enemyMgr.SF_AlertFadeSpeed;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_enemyMgr);
        #endif
    }
}