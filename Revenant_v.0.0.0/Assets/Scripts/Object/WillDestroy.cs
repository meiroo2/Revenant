using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillDestroy : MonoBehaviour
{
    private float Timer = 3f;
    private SoundMgr_SFX m_SoundSFXMgr;

    private void Start()
    {
        m_SoundSFXMgr = GameManager.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_SoundSFXMgr.playSFXSound(0, transform.position);
    }
}
