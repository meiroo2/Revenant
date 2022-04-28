using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundPanning : MonoBehaviour
{
    private float Timer = 0.5f;

    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0f)
        {
            Timer = 0.5f;
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Weapons/Bullet_Hit/Normal", gameObject);
        }
    }
}
