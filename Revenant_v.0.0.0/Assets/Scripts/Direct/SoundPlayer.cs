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
    public static EventInstance m_BGMInstance;

    // Constructors
    private void Awake()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        m_BGMInstance.getPlaybackState(out state);

        if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            m_BGMInstance = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Shield");
            m_BGMInstance.start();
        }
    }

    // Updates

    
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Fire");
                break;
            
            case 2:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Dryfire");
                break;
            
            case 3: // 근접공격
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Roll");
                break;
            
            case 4:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Roll");
                break;
            
            case 5:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Sit");
                break;
            
            case 6:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Stand");    
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
        }
    }


    /*
    public void playPlayerSFXSound(int _num)
    {
        switch (_num)
        {
            case 0:  
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Roll/Player_Roll");
                break;
            
            case 1: 
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/GunTic/Player_GunTic");
                break;
            
            case 2:   
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/GunReload/Player_GunReload");
                break;
            
            case 3:   
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/BulletHit/Enemy_BulletHit_Body");
                break;
            
            case 4:    
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Move/Walk/Walk_Dirt");
                break;
            
            case 5:   
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Sit Roll/Player_Sit");
                break;
            
            case 6:   
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Sit Roll/Player_Stand");
                break;
        }
    }
    public void playGunFireSound(int _num, GameObject _position)
    {
        switch (_num)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Player/P_Fire", _position);
                break;
            
            case 1:
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemy/Gunshot_3bursts/Enemy_Gunshot_3bursts", _position);
                break;
            
            case 2:
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemy/AttackClose/Enemy_AttackClose", _position);
                break;
        }
    }
    public void playGunFireSound(int _num, Vector2 _position)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Gunshot/Player_Gunshot", _position);
    }
    public void playAttackedSound(MatType _matType, GameObject _position)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Weapons/Bullet_Hit/Normal", _position);
    }
    public void playAttackedSound(MatType _matType, Vector2 _position)
    {
        switch (_matType)
        {
            case MatType.Normal:
                //Debug.Log("사운드재생");
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/BulletHit/Enemy_BulletHit_Metal", _position);
                break;

            case MatType.Target_Head:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/BulletHit/Enemy_BulletHit_Head", _position);
                break;

            case MatType.Target_Body:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/BulletHit/Enemy_BulletHit_Body", _position);
                break;
        }
    }
    public void playSFXSound(int _input, Vector2 _position)
    {
        switch (_input)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Shell/Player_Shell", _position);
                break;
        }
    }

    public void PlayUISound(int _input)
    {
        switch (_input)
        {
            case 0:  
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hitmark_UI");
                break;
            
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hitmark_UI1");
                break;
        }
    }

    // ??? ??????? ???? ???? ???? ???
    */
}
