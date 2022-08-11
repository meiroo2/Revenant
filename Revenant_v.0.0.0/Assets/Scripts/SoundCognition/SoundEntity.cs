using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundEntity : MonoBehaviour
{
    // Member Variables
    private SoundHotBoxParam m_SoundHotBoxParam;
    private Coroutine m_Coroutine;
    private List<SoundHotBox> m_SoundHotBoxList;

    private bool m_CanCheck = false;
    
    
    // Constructors
    private void Awake()
    {
        m_SoundHotBoxList = new List<SoundHotBox>();
    }
    private void OnEnable()
    {
        m_Coroutine = StartCoroutine(SoundHit());
    }
    private void OnDisable()
    {
        StopCoroutine(m_Coroutine);
        
        // 비활성화 시 리스트 비우고 메모리 반납
        m_SoundHotBoxList.Clear();
        m_SoundHotBoxList.TrimExcess();
    }


    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!m_CanCheck || !col.CompareTag("Enemy"))
            return;

        var instance = col.GetComponent<SoundHotBox>();
        m_SoundHotBoxList.Add(instance);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!m_CanCheck || !other.CompareTag("Enemy"))
            return;
        
        var instance = other.GetComponent<SoundHotBox>();
        m_SoundHotBoxList.Remove(instance);
    }


    // Functions
    public void InitSound(SoundHotBoxParam _param)
    {
        m_SoundHotBoxParam = _param;
    }

    private IEnumerator SoundHit()
    {
        m_CanCheck = true;
        yield return new WaitForSecondsRealtime(0.1f);
        m_CanCheck = false;

        foreach (var element in m_SoundHotBoxList)
        {
            element.HitSoundHotBox(m_SoundHotBoxParam);
        }
        
        gameObject.SetActive(false);
        
        yield break;
    }
}