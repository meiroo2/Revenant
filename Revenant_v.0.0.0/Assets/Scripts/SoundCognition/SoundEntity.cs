using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SoundEntity : MonoBehaviour
{
    // Member Variables
    private BoxCollider2D m_Collider;
    
    private SoundHotBoxParam m_SoundHotBoxParam;
    private Coroutine m_Coroutine;

    private bool m_CanCheck = false;
    
    
    // Constructors
    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
    }

    private void OnDisable()
    {
        if (!ReferenceEquals(m_Coroutine, null))
            StopCoroutine(m_Coroutine);
    }


    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!m_CanCheck)
            return;

        if (m_SoundHotBoxParam.m_IsPlayers)
        {
            if (!col.CompareTag("Enemy"))
                return;

            if (col.TryGetComponent(out SoundHotBox soundHotbox))
            {
                soundHotbox.HitSoundHotBox(m_SoundHotBoxParam);
            }
        }
        else
        {
            
        }
    }


    // Functions
    public void InitSound(SoundHotBoxParam _param)
    {
        m_SoundHotBoxParam = _param;
        transform.position = m_SoundHotBoxParam.m_SoundPos;
        m_Collider.size = _param.m_SoundSize;
        m_Coroutine = StartCoroutine(SoundHit());
    }

    private IEnumerator SoundHit()
    {
        m_CanCheck = true;
        yield return new WaitForSecondsRealtime(m_SoundHotBoxParam.m_LifeTime);
        m_CanCheck = false;
        gameObject.SetActive(false);
        
        yield break;
    }
}