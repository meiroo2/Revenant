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

        PlayerManipulator manipulator = Resources.Load<GameMgr>("GameMgr").p_PlayerManipulator;
        TransferPlayerValues(manipulator, false);
        TransferNegotiatorValues(manipulator, false);
        TransferAimValues(false);
    }
    
    // Functions

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Player")]
    private void Player_적용하기()
    {
        var ResourceManipulator = Resources.Load<GameMgr>("GameMgr").p_PlayerManipulator;
        var SceneManipulator = GameObject.FindObjectOfType<GameMgr>().p_PlayerManipulator;
        
        TransferPlayerValues(ResourceManipulator, true);
        ResourceManipulator.SetPlayer(true);

        PrefabUtility.RevertPrefabInstance(SceneManipulator.gameObject, InteractionMode.UserAction);
    }

    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Negotiator")]
    private void Negotiator_적용하기()
    {
        var ResourceManipulator = Resources.Load<GameMgr>("GameMgr").p_PlayerManipulator;
        var SceneManipulator = GameObject.FindObjectOfType<GameMgr>().p_PlayerManipulator;

        TransferNegotiatorValues(ResourceManipulator, true);
        ResourceManipulator.SetNegotiator(true);
        
        PrefabUtility.RevertPrefabInstance(SceneManipulator.gameObject, InteractionMode.UserAction);
    }
    
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Aim")]
    private void Aim_적용하기()
    {
        TransferAimValues(true);
    }
    
    /// <summary>
    /// 각종 Player 데이터를 Player_ValueManipulator로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_manipulator"></param>
    /// <param name="_toPlayerManipulator"></param>
    private static void TransferPlayerValues(PlayerManipulator _manipulator, bool _toPlayerManipulator)
    {
        if (_toPlayerManipulator)
        {
            _manipulator.P_HP = P_HP;
            _manipulator.P_StunInvincibleTime = P_StunInvincibleTime;
            _manipulator.P_Speed = P_Speed;
            _manipulator.P_BackSpeedMulti = P_BackSpeedMulti;
            _manipulator.P_RollSpeedMulti = P_RollSpeedMulti;
            _manipulator.P_MeleeSpeedMulti = P_MeleeSpeedMulti;
            _manipulator.P_MeleeDamage = P_MeleeDamage;
            _manipulator.P_MeleeStunValue = P_MeleeStunValue;
            _manipulator.P_JustEvadeStartTime = P_JustEvadeStartTime;
            _manipulator.P_JustEvadeEndTime = P_JustEvadeEndTime;
            _manipulator.P_RollDecelerationSpeed = P_RollDecelerationSpeed;
            _manipulator.P_ReloadSpeed = P_ReloadSpeed;
        }
        else
        {
            P_HP = _manipulator.P_HP;
            P_StunInvincibleTime = _manipulator.P_StunInvincibleTime;
            P_Speed = _manipulator.P_Speed;
            P_BackSpeedMulti = _manipulator.P_BackSpeedMulti;
            P_RollSpeedMulti = _manipulator.P_RollSpeedMulti;
            P_MeleeSpeedMulti = _manipulator.P_MeleeSpeedMulti;
            P_MeleeDamage = _manipulator.P_MeleeDamage;
            P_MeleeStunValue = _manipulator.P_MeleeStunValue;
            P_JustEvadeStartTime = _manipulator.P_JustEvadeStartTime;
            P_JustEvadeEndTime = _manipulator.P_JustEvadeEndTime;
            P_RollDecelerationSpeed = _manipulator.P_RollDecelerationSpeed;
            P_ReloadSpeed = _manipulator.P_ReloadSpeed;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_manipulator);
        #endif
    }

    /// <summary>
    /// 각종 Negotiator 데이터를 Player_ValueManipulator로 넘기거나 가져옵니다.
    /// </summary>
    /// <param name="_manipulator"></param>
    /// <param name="_toPlayerManipulator"></param>
    private static void TransferNegotiatorValues(PlayerManipulator _manipulator, bool _toPlayerManipulator)
    {
        if (_toPlayerManipulator)
        {
            _manipulator.N_Damage = N_Damage;
            _manipulator.N_StunValue = N_StunValue;
            _manipulator.N_MinFireDelay = N_MinFireDelay;
        }
        else
        {
            N_Damage = _manipulator.N_Damage;
            N_StunValue = _manipulator.N_StunValue;
            N_MinFireDelay = _manipulator.N_MinFireDelay;
        }

        #if UNITY_EDITOR
            EditorUtility.SetDirty(_manipulator);
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
