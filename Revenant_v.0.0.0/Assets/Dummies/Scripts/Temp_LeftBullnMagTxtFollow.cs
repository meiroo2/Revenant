using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temp_LeftBullnMagTxtFollow : MonoBehaviour
{
    private Player_WeaponMgr m_WeaponMgr;
    private Text m_Text;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    private void Start()
    {
        m_WeaponMgr = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_WeaponMgr;
    }

    private void FixedUpdate()
    {
        m_Text.text = m_WeaponMgr.m_CurWeapon.m_LeftRounds + " / " + m_WeaponMgr.m_CurWeapon.m_LeftMags;
    }
}