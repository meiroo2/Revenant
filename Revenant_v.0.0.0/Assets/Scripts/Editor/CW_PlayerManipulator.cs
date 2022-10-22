using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public class CW_PlayerManipulator : OdinEditorWindow
{
    // Const Variables
    private const float m_LabelWidth = 150f;
    
    
    // Member Variables

    #region Player_Basic

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int P_HP;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_StunInvincibleTime;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_Speed;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_BackSpeedMulti;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_RollSpeedMulti;
    
    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_RollDecelerationSpeed;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_ReloadSpeed;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_MeleeSpeedMulti;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int P_MeleeDamage;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int P_MeleeStunValue;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Title("JustEvade")]
    public static float P_JustEvadeStartTime;

    [TabGroup("Player"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float P_JustEvadeEndTime;

    #endregion

    
    #region Negotiator

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_Damage;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_StunValue;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float N_MinFireDelay;

    /*
    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_BulletCount;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_MagCount;
    */
    
    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float N_ReloadSpeed;

    #endregion

    #region Aim

    [TabGroup("Aim"), ShowInInspector, TableList, LabelWidth(m_LabelWidth), Range(0f, 0.5f)]
    public static float A_AimCursorRadius;

    #endregion

    // Constructor
    [MenuItem(("에디터/플레이어 변수 수정기"))]
    private static void OpenWindow()
    {
        GetWindow<CW_PlayerManipulator>().Show();
        
        TransferPlayerValues(false);
        TransferNegotiatorValues(false);
        TransferAimValues(false);
    }
    
    // Functions

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Player")]
    private void Player_적용하기()
    {
        var p_ValMani = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<PlayerManipulator>();
        TransferPlayerValues(true);
        
        p_ValMani.SetPlayer();
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Negotiator")]
    private void Negotiator_적용하기()
    {
        var p_ValMani = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<PlayerManipulator>();
        TransferNegotiatorValues(true);
        
        p_ValMani.SetNegotiator();
    }
    
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Aim")]
    private void Aim_적용하기()
    {
        TransferAimValues(true);
    }
    
    /// <summary>
    /// 각종 Player 데이터를 Player_ValueManipulator로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toPlayerManipulator">True시 내보내기</param>
    private static void TransferPlayerValues(bool _toPlayerManipulator)
    {
        var p_ValMani = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<PlayerManipulator>();
        
        if (_toPlayerManipulator)
        {
            p_ValMani.P_HP = P_HP;
            p_ValMani.P_StunInvincibleTime = P_StunInvincibleTime;
            p_ValMani.P_Speed = P_Speed;
            p_ValMani.P_BackSpeedMulti = P_BackSpeedMulti;
            p_ValMani.P_RollSpeedMulti = P_RollSpeedMulti;
            p_ValMani.P_MeleeSpeedMulti = P_MeleeSpeedMulti;
            p_ValMani.P_MeleeDamage = P_MeleeDamage;
            p_ValMani.P_MeleeStunValue = P_MeleeStunValue;
            p_ValMani.P_JustEvadeStartTime = P_JustEvadeStartTime;
            p_ValMani.P_JustEvadeEndTime = P_JustEvadeEndTime;
            p_ValMani.P_RollDecelerationSpeed = P_RollDecelerationSpeed;
            p_ValMani.P_ReloadSpeed = P_ReloadSpeed;
        }
        else
        {
            P_HP = p_ValMani.P_HP;
            P_StunInvincibleTime = p_ValMani.P_StunInvincibleTime;
            P_Speed = p_ValMani.P_Speed;
            P_BackSpeedMulti = p_ValMani.P_BackSpeedMulti;
            P_RollSpeedMulti = p_ValMani.P_RollSpeedMulti;
            P_MeleeSpeedMulti = p_ValMani.P_MeleeSpeedMulti;
            P_MeleeDamage = p_ValMani.P_MeleeDamage;
            P_MeleeStunValue = p_ValMani.P_MeleeStunValue;
            P_JustEvadeStartTime = p_ValMani.P_JustEvadeStartTime;
            P_JustEvadeEndTime = p_ValMani.P_JustEvadeEndTime;
            P_RollDecelerationSpeed = p_ValMani.P_RollDecelerationSpeed;
            P_ReloadSpeed = p_ValMani.P_ReloadSpeed;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_ValMani);
        #endif
    }

    /// <summary>
    /// 각종 Negotiator 데이터를 Player_ValueManipulator로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toPlayerManipulator"></param>
    private static void TransferNegotiatorValues(bool _toPlayerManipulator)
    {
        var p_ValMani = GameObject.FindGameObjectWithTag("GameMgr").GetComponent<PlayerManipulator>();

        if (_toPlayerManipulator)
        {
            p_ValMani.N_Damage = N_Damage;
            p_ValMani.N_StunValue = N_StunValue;
            p_ValMani.N_MinFireDelay = N_MinFireDelay;
            /*
            p_ValMani.N_MaxBullet = N_BulletCount;
            p_ValMani.N_MaxMag = N_MagCount;
            */
            p_ValMani.N_ReloadSpeed = N_ReloadSpeed;
        }
        else
        {
            N_Damage = p_ValMani.N_Damage;
            N_StunValue = p_ValMani.N_StunValue;
            N_MinFireDelay = p_ValMani.N_MinFireDelay;
            /*
            N_BulletCount = p_ValMani.N_MaxBullet;
            N_MagCount = p_ValMani.N_MaxMag;
            */
            N_ReloadSpeed = p_ValMani.N_ReloadSpeed;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_ValMani);
        #endif
    }
    
    /// <summary>
    /// 각종 Aim 데이터를 AimCursor로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_toAimCursor"></param>
    private static void TransferAimValues(bool _toAimCursor)
    {
        var aimCursor = Resources.Load<AimCursor>("Logic/AimCursor");

        if (_toAimCursor)
        {
            aimCursor.p_AimRaycastRadius = A_AimCursorRadius;
        }
        else
        {
            A_AimCursorRadius = aimCursor.p_AimRaycastRadius;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(aimCursor);
        #endif
    }
}
