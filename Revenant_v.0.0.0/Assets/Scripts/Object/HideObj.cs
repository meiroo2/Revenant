using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEditor;
using UnityEngine;

public class HideObj : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public HideSlot[] p_HideSlots;
    
    [Space(20f)]
    [Header("Plz Assign")]
    public SpriteOutline m_OutlineScript;
    
    // Member Variables
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
    public bool m_isEnemys { get; set; } = false;



    // Constructors
    private void Awake()
    {

        m_ParentObj = gameObject;
        if (p_HideSlots.Length <= 0)
        {
            Debug.Log(gameObject.name + "에 HideSlot 배정이 되어있지 않음");
            return;
        }
        
        // 각 Slot에 부모 스크립트(this) 넣어줌
        foreach (var ele in p_HideSlots)
        {
            ele.m_HideObj = this;
        }
    }

    private void Start()
    {
        m_OutlineScript.outlineSize = 0;
    }


    // Updates


    // Physics


    // Functions
    public int HitHotBox(IHotBoxParam _param) 
    { 
        return 1; 
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
