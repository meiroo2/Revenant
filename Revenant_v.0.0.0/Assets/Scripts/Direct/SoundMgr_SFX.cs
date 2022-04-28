using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr_SFX : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables


    // Constructors

    // Updates

    // Physics


    // Functions
    public void playGunFireSound(int _num, GameObject _position)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Weapons/Rifle/Rifle_Shot", _position);
    }
    public void playGunFireSound(int _num, Vector2 _position)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Rifle/Rifle_Shot", _position);
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Normal", _position);
                break;

            case MatType.Target_Head:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Target_Head", _position);
                break;

            case MatType.Target_Body:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Target_Body", _position);
                break;
        }
    }

    public void playSFXSound(int _input, Vector2 _position)
    {
        switch (_input)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Shell/Shell_Fall", _position);
                break;
        }
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
