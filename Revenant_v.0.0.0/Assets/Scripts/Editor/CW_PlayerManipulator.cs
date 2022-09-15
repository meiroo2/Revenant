using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
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
    public static float P_MeleeSpeedMulti;
    
    #endregion

    
    #region Negotiator

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_Damage;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_StunValue;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float N_MinFireDelay;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_BulletCount;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static int N_MagCount;

    [TabGroup("Negotiator"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    public static float N_ReloadSpeed;

    #endregion

    // Constructor
    [MenuItem(("에디터/플레이어 변수 수정기"))]
    private static void OpenWindow()
    {
        GetWindow<CW_PlayerManipulator>().Show();
        
        TransferPlayerValues(false);
        TransferNegotiatorValues(false);
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
        }
        else
        {
            P_HP = p_ValMani.P_HP;
            P_StunInvincibleTime = p_ValMani.P_StunInvincibleTime;
            P_Speed = p_ValMani.P_Speed;
            P_BackSpeedMulti = p_ValMani.P_BackSpeedMulti;
            P_RollSpeedMulti = p_ValMani.P_RollSpeedMulti;
            P_MeleeSpeedMulti = p_ValMani.P_MeleeSpeedMulti;
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
            p_ValMani.N_MaxBullet = N_BulletCount;
            p_ValMani.N_MaxMag = N_MagCount;
            p_ValMani.N_ReloadSpeed = N_ReloadSpeed;
        }
        else
        {
            N_Damage = p_ValMani.N_Damage;
            N_StunValue = p_ValMani.N_StunValue;
            N_MinFireDelay = p_ValMani.N_MinFireDelay;
            N_BulletCount = p_ValMani.N_MaxBullet;
            N_MagCount = p_ValMani.N_MaxMag;
            N_ReloadSpeed = p_ValMani.N_ReloadSpeed;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(p_ValMani);
        #endif
    }
}
