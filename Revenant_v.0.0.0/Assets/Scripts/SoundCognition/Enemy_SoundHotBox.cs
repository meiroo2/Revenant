using System;
using UnityEngine;


public class Enemy_SoundHotBox : SoundHotBox
{
    // Member Variables
    private BasicEnemy m_Enemy;
    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    public override void HitSoundHotBox(SoundHotBoxParam _param)
    {
        Debug.Log("사운드 감지 " + _param.m_SoundType + " " + _param.m_SoundOriginPos);
        m_Enemy.StartPlayerCognition();
    }
}