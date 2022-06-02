using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : ForObjPull_Once
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_SoundSFXMgr.playSFXSound(0, transform.position);
    }
}
