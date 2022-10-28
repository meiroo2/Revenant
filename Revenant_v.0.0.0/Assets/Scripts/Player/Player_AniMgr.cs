using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
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
        SetVisualParts(false, true, true, false);
    }

    
    // Updates
   

    
    // Functions
  
    

    /// <summary>
    /// Player Arm의 스프라이트를 각도별 스프라이트 변경 모드로 바꿉니다.
    /// </summary>
    /// <param name="_toAngle">각도변경 모드</param>
    public void ChangeArmAniToAngleChange(bool _toAngle)
    {
        m_IsFightMode = _toAngle;
        
        // UpperBody 애니메이터 끄고, 팔은 보임
        //p_UpperBody.SetVisible(!_toAngle);
        p_UpperBody.SetAniVisible(!_toAngle);
        p_Arm.SetVisible(_toAngle);
    }

    /// <summary>
    /// Player의 각 VisualPart의 Visible 여부를 제어합니다.
    /// </summary>
    /// <param name="_full"></param>
    /// <param name="_upper"></param>
    /// <param name="_low"></param>
    /// <param name="_arm"></param>
    public void SetVisualParts(bool _full, bool _upper, bool _low, bool _arm)
    {
        p_FullBody.SetVisible(_full);
        p_UpperBody.SetVisible(_upper);
        p_LowerBody.SetVisible(_low);
        p_Arm.SetVisible(_arm);
    }
}