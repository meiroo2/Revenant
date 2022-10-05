using System.Collections;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player_AniMgr : MonoBehaviour
{
    // Visible Member Variables
    public VisualPart p_FullBody;
    public VisualPart p_UpperBody;
    public VisualPart p_LowerBody;
    public VisualPart p_Arm;

    // Member Variables
    [ShowInInspector, ReadOnly] public bool m_IsFightMode { get; private set; } = false;
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private int m_curAnglePhase;

    private Player_ArmMgr m_PlayerArmMgr;
    public int m_curArmIdx { get; private set; } = 0;



    // Constructors
    private void Start()
    {
        m_Player = GetComponent<Player>();
        m_PlayerRotation = m_Player.m_playerRotation;

        p_UpperBody.SetAnim_Float("ReloadSpeed", m_Player.p_ReloadSpeed);
    }

    // Updates
   

    // Functions
    
    /// <summary>
    /// Player의 스프라이트 모드를 전투 모드로 전환합니다.
    /// </summary>
    /// <param name="_toFight">전투 모드 여부</param>
    public void ChangeAniModeToFight(bool _toFight)
    {
        m_IsFightMode = _toFight;
        
        // UpperBody 애니메이터 끄고, 팔은 보임
        p_UpperBody.SetAniVisible(!_toFight);
        p_Arm.SetAniVisible(!_toFight);
        
        p_Arm.SetVisible(_toFight);
    }
    
    
    public void ExitPlayerAnim()
    {
        switch (m_Player.m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                break;

            case PlayerStateName.WALK:
                p_UpperBody.SetAnim_Int("Walk", 0);
                p_LowerBody.SetAnim_Int("Walk", 0);
                break;

            case PlayerStateName.ROLL:
                p_FullBody.SetAnim_Int("Roll", 0);
                break;

            case PlayerStateName.HIDDEN:
                p_FullBody.SetAnim_Int("Hide", 0);
                break;
            
            case PlayerStateName.MELEE:
                p_FullBody.SetAnim_Int("Melee", 0);
                break;

            case PlayerStateName.DEAD:
                SetVisualParts(true, false, false, false);
                break;
        }
    }
    
    public void PlayPlayerAnim()
    {
        switch (m_Player.m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                SetVisualParts(false, true, true, false);
                p_UpperBody.SetAnim_Int("Walk", 0);
                p_LowerBody.SetAnim_Int("Walk", 0);
                break;

            case PlayerStateName.WALK:
                SetVisualParts(false, true, true, false);
                
                if (m_Player.GetIsPlayerWalkStraight())
                {
                    p_UpperBody.SetAnim_Int("Walk", 1);
                    p_LowerBody.SetAnim_Int("Walk", 1);
                }
                else
                {
                    p_UpperBody.SetAnim_Int("Walk", -1);
                    p_LowerBody.SetAnim_Int("Walk", -1);
                }
                break;

            case PlayerStateName.ROLL:
                ChangeAniModeToFight(false);
                SetVisualParts(true, false, false, false);
                p_FullBody.SetAnim_Int("Roll", 1);
                break;

            case PlayerStateName.HIDDEN:
                ChangeAniModeToFight(false);
                SetVisualParts(true, false, false, false);
                p_FullBody.SetAnim_Int("Hide", 1);
                break;
            
            case PlayerStateName.MELEE:
                ChangeAniModeToFight(false);
                SetVisualParts(true, false, false, false);
                p_FullBody.SetAnim_Int("Melee", 1);
                break;

            case PlayerStateName.DEAD:
                SetVisualParts(true, false, false, false);
                break;
        }
    }

    /// <summary>
    /// 재장전 애니메이션을 재생하거나 멈춥니다.
    /// </summary>
    /// <param name="_isPlay"></param>
    public void PlayReloadAnim(bool _isPlay)
    {
        p_UpperBody.SetAnim_Int("Reload", _isPlay ? 1 : 0);
    }
   
    public void SetVisualParts(bool _full, bool _upper, bool _low, bool _arm)
    {
        p_FullBody.SetVisible(_full);
        p_UpperBody.SetVisible(_upper);
        p_LowerBody.SetVisible(_low);
        p_Arm.SetVisible(_arm);
    }
}