using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundPlayer : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    public static EventInstance m_BGMInstance { get; private set; }

    // Constructors
    private void Awake()
    {
        //FMODUnity.RuntimeManager.CoreSystem.set3DSettings(1f, 50f, 1f);
        
        FMOD.Studio.PLAYBACK_STATE state;
        m_BGMInstance.getPlaybackState(out state);

        if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            m_BGMInstance = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Lab");
            m_BGMInstance.start();
        }
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            m_BGMInstance.setVolume(0f);
    }

    // Physics


    // Functions
    public void PlayUISoundOnce(int _num)
    {
        switch (_num)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hitmark_UI");
                break;
            
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hitmark_UI1");
                break;
        }
    }
    
    public EventInstance GetPlayerSoundInstance(int _num)
    {
        EventInstance eventInstance = new EventInstance();
        switch (_num)
        {
            case 0:
                eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Player/P_Bullettime_Start");
                break;
        }

        return eventInstance;
    }
    
    public void PlayPlayerSoundOnce(int _num)
    {
        switch (_num)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Reload");
                break;
            
            case 1:
                // 소음기 소리
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Fire");
                break;
            
            case 2:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Dryfire");
                break;
            
            case 3: // 근접공격
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Slash");
                break;
            
            case 4:
                // Roll
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Roll");
                break;
            
            case 5:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Sit");
                break;
            
            case 6:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Stand");    
                break;
            
            case 7:
                // 저스트회피 시작
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_JustStart");  
                break;
            
            case 8:
                // Timescale 복구 트랜지션 사운드 시작
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Repair");  
                break;
            
            case 9:
                // 우클릭 공격
                  
                break;
        }
    }

    public void PlayCommonSound(int _idx, Vector2 _position)
    {
        EventInstance eventInstance = GetCommonSound(_idx);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
        eventInstance.start();
        eventInstance.release();
    }

    private EventInstance GetCommonSound(int _idx)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_idx)
        {
            case 0:
                eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Object/Shell");
                break;
        }

        return eventInstance;
    }
    
    
    /// <summary>
    /// Enemy에 할당된 사운드를 재생합니다.
    /// </summary>
    /// <param name="_enemyNum">Norm, Melee, Drone, Shield</param>
    /// <param name="_soundNum"></param>
    /// <param name="_position"></param>
    /// <param name="_matType"></param>
    public void PlayEnemySound(int _enemyNum, int _soundNum, Vector2 _position, MatType _matType = MatType.Dirt)
    {
        EventInstance eventInstance;
        
        switch (_enemyNum)
        {
            case 0:
                // NormalGang
                eventInstance = GetNormalGangInstance(_soundNum, _matType);
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
                eventInstance.start();
                eventInstance.release();
                break;
            
            case 1:
                // MeleeGang
                eventInstance = GetMeleeGangInstance(_soundNum, _matType);
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
                eventInstance.start();
                eventInstance.release();
                break;
            
            case 2:
                // DroneGang
                eventInstance = GetDroneGangInstance(_soundNum, _matType);
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
                eventInstance.start();
                eventInstance.release();
                break;
            
            case 3:
                // ShieldGang
                eventInstance = GetShieldGangInstance(_soundNum, _matType);
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
                eventInstance.start();
                eventInstance.release();
                break;
            
            case 4:
                // SpecialForce
                eventInstance = GetSpecialForceInstance(_soundNum, _matType);
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
                eventInstance.start();
                eventInstance.release();
                break;
        }
    }

    private EventInstance GetSpecialForceInstance(int _soundNum, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_soundNum)
        {
            case 0:
                // Walk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/SpecialForce/SF_Walk");
                break;
            
            case 1:
                // Roll
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/SpecialForce/SF_Roll");
                break;
            
            case 2:
                // Atk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/SpecialForce/SF_Atk");
                break;
            
            case 3:
                // Alert
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/Alert_Full");
                break;
        }
        
        return eventInstance;
    }
    
    private EventInstance GetDroneGangInstance(int _soundNum, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_soundNum)
        {
            case 0:
                // Walk
                break;
            
            case 1:
                // Small Boom
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/DroneGang/DG_SBoom");
                break;
            
            case 2:
                // Big Boom
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/DroneGang/DG_BBoom");
                break;
        }
        
        return eventInstance;
    }

    private EventInstance GetNormalGangInstance(int _soundNum, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_soundNum)
        {
            case 0:
                // Walk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_Walk");
                
                switch (_matType)
                {
                    case MatType.Dirt:
                        eventInstance.setParameterByNameWithLabel("Texture", "Dirt");
                        break;
                    
                    case MatType.Stone:
                        eventInstance.setParameterByNameWithLabel("Texture", "Stone");
                        break;
                    
                    case MatType.Metal:
                        eventInstance.setParameterByNameWithLabel("Texture", "Metal");
                        break;
                }
                break;
            
            case 1:
                // Atk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_Atk");
                break;
            
            case 2:
                // Melee Atk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_Melee");
                break;
            
            case 3:
                // Melee Hit
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_MeleeHit");
                break;
            
            case 4:
                // Pickup
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_PickUp");
                break;
            
            case 5:
                // Alert Beep
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                        "event:/SFX/Enemy/Alert_Full");
                break;
            
            case 6:
                // Dead
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/NormalGang/NG_Dead");
                break;

        }
        
        return eventInstance;
    }
    
    private EventInstance GetMeleeGangInstance(int _soundNum, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_soundNum)
        {
            case 0:
                // Walk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/MeleeGang/MG_Walk");
                
                switch (_matType)
                {
                    case MatType.Dirt:
                        eventInstance.setParameterByNameWithLabel("Texture", "Dirt");
                        break;
                    
                    case MatType.Stone:
                        eventInstance.setParameterByNameWithLabel("Texture", "Stone");
                        break;
                    
                    case MatType.Metal:
                        eventInstance.setParameterByNameWithLabel("Texture", "Metal");
                        break;
                }
                break;
            
            case 1:
                // Atk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/MeleeGang/MG_Atk");
                break;
            
            case 2:
                // Bat Hit
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/MeleeGang/MG_AtkHit");
                break;
            
            case 3:
                // Dead
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/MeleeGang/MG_Dead");
                break;
        }
        
        return eventInstance;
    }
    
    private EventInstance GetShieldGangInstance(int _soundNum, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_soundNum)
        {
            case 0:
                // Walk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_Walk");
                break;
            
            case 1:
                // Atk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_Atk");
                break;
            
            case 2:
                // Axe Hit
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_AtkHit");
                break;
            
            case 3:
                // Shield Hit
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_ShieldHit");
                break;
            
            case 4:
                // Shield Break;
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_ShieldBreak");
                break;
            
            case 5:
                // Dead
                eventInstance = FMODUnity.RuntimeManager.CreateInstance(
                    "event:/SFX/Enemy/ShieldGang/SG_Dead");
                break;
        }
        
        return eventInstance;
    }
    
    
    public void PlayCommonSoundByMatType(int _idx, MatType _matType, Vector2 _position)
    {
        EventInstance eventInstance = GetCommonSoundByMatTypeInstance(_idx, _matType);

        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
        eventInstance.start();
        eventInstance.release();
    }

    private EventInstance GetCommonSoundByMatTypeInstance(int _idx, MatType _matType)
    {
        EventInstance eventInstance = new EventInstance();

        switch (_idx)
        {
            case 0:     // Light-Walk
                eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Common/C_LightWalk");
                switch (_matType)
                {
                    case MatType.Dirt:
                        eventInstance.setParameterByNameWithLabel("Texture", "Dirt");
                        break;
            
                    case MatType.Metal:
                        eventInstance.setParameterByNameWithLabel("Texture", "Metal");
                        break;
                }
                break;
            
            case 1:
                eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Common/C_HeavyWalk");
                switch (_matType)
                {
                    case MatType.Dirt:
                        eventInstance.setParameterByNameWithLabel("Texture", "Dirt");
                        break;
            
                    case MatType.Metal:
                        eventInstance.setParameterByNameWithLabel("Texture", "Metal");
                        break;
                }
                break;
            
            case 2:
                eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemy/ShieldGang/SG_Walk");
                break;
        }

        return eventInstance;
    }
    
    public void PlayHitSoundByMatType(MatType _matType, Transform _toAttach)
    {
        EventInstance eventInstance = GetHitSoundInstance(_matType);

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, _toAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayHitSoundByMatType(MatType _matType, Vector2 _position)
    {
        EventInstance eventInstance = GetHitSoundInstance(_matType);

        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_position));
        eventInstance.start();
        eventInstance.release();
    }

    /// <summary>
    /// 특정하게 붙여서 발동되는 사운드를 재생하고 돌려줍니다.
    /// </summary>
    /// <param name="_enemyNum"></param>
    /// <param name="_soundNum"></param>
    /// <param name="_toAttach"></param>
    /// <returns></returns>
    public EventInstance GetAttachedEnemySound(int _enemyNum, int _soundNum, Transform _toAttach)
    {
        EventInstance eventInstance;
        
        switch (_enemyNum)
        {
            case 0:
                // NormalGang
                
                break;
            
            case 1:
                // MeleeGang
                
                break;
            
            case 2:
                // DroneGang
                switch (_soundNum)
                {
                    case 0:
                        eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enemy/DroneGang/DG_Fly");
                        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, _toAttach);
                        return eventInstance;
                        break;
                }
                break;
            
            case 3:
                // ShieldGang
                break;
        }

        eventInstance = FMODUnity.RuntimeManager.CreateInstance(null);
        return eventInstance;
    }

    private EventInstance GetHitSoundInstance(MatType _matType)
    {
        EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Common/C_BulletHit");

        switch (_matType)
        {
            case MatType.Metal:
                eventInstance.setParameterByName("Texture", 2);
                break;
            
            case MatType.Normal:
                eventInstance.setParameterByName("Texture", 1);
                break;
            
            case MatType.Flesh:
                eventInstance.setParameterByName("Texture", 3);
                break;
        }

        return eventInstance;
    }
    

    public void PlayEnemySoundOnce(int _num, GameObject _toAttach)
    {
        switch (_num)
        {
            case 0:
                if (_toAttach)
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Common/C_LightFire", _toAttach);
                else
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Common/C_LightFire");
                break;
            
            case 1:
                if (_toAttach)
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemy/ShieldGang/E_Swing", _toAttach);
                else
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/ShieldGang/E_Swing");
                break;
            
            case 2:
                // Slash Hit
                if (_toAttach)
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemy/Slash_Hit", _toAttach);
                else
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/Slash_Hit");
                break;
        }
    }
}
