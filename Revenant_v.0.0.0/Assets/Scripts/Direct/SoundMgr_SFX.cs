using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundMgr_SFX : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables


    // Constructors

    // Updates

    
    // Physics


    // Functions
    public void playPlayerSFXSound(int _num)
    {
        switch (_num)
        {
            case 0:     // 플레이어 구르기
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Roll/Player_Roll");
                break;
            
            case 1:     // 총알 없음
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/GunTic/Player_GunTic");
                break;
            
            case 2:     // 재장전
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/GunReload/Player_GunReload");
                break;
            
            case 3:     // 플레이어 피격
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemy/BulletHit/Enemy_BulletHit_Body");
                break;
            
            case 4:     // 플레이어 걷기
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Move/Walk/Walk_Dirt");
                break;
            
            case 5:     // 플레이어 앉음
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Sit Roll/Player_Sit");
                break;
            
            case 6:     // 플레이어 일어남
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Sit Roll/Player_Stand");
                break;
        }
    }
    public void playGunFireSound(int _num, GameObject _position)
    {
        switch (_num)
        {
            case 0:     // 플레이어 권총
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Player/Gunshot/Player_Gunshot", _position);
                break;
            
            case 1:     // 3점사용
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemy/Gunshot_3bursts/Enemy_Gunshot_3bursts", _position);
                break;
            
            case 2:     // 근거리용
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

    public void playUISound(int _input)
    {
        switch (_input)
        {
            case 0:     // 헤드 히트마크
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hitmark_UI");
                break;
            
            case 1:
                //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/HitMarker/Player_HitMark_Body");
                break;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
