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
        m_Enemy.SoundCognition(_param);
    }
}