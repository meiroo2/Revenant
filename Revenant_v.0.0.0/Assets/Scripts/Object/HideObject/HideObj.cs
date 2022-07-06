using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEditor;
using UnityEngine;

public class HideObj : MonoBehaviour
{
    // Visible Member Variables
    [Tooltip("0으로 설정시 파괴되지 않습니다.")]
    public int m_Hp = 0;

    [Space(20f)]
    [Header("Plz assign")] 
    public HideSlot[] p_HideSlots;
    public SpriteOutline p_Outline;
    public HideObjCollider p_Collider;
    

    // Member Variables
    private bool m_Destructable = false;

    
    // Constructors
    private void Awake()
    {
        p_Outline.outlineSize = 0;
        
        // 0보다 클 경우 파괴되는 엄폐물
        m_Destructable = m_Hp > 0;
        
        // 콜라이더에 부모 대입
        p_Collider.m_HideObj = this;
        
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


    // Updates


    // Physics


    // Functions
    public int GetHit(IHotBoxParam _param)
    {
        switch (m_Destructable)
        {
            case true when m_Hp <= 0:
                // 파괴 가능한 물체가 이미 Hp가 0 이하일 경우
                return 0;
            
            case true:
                // 파괴 가능한 물체
                m_Hp -= _param.m_Damage;

                if (m_Hp <= 0)
                {
                    // 파괴 사운드 재생됨
                    
                    // Gameobject Disable 
                    gameObject.SetActive(false);
                }

                return 1;
                break;
            
            case false:
                // 파괴 불가능 물체
                return 1;
                break;
            
            default:
                return 0;
        }
    }

    public void UpdateHideSlotInfo()
    {
        // 슬롯 전체 꺼짐 상태로 가정
        var isSlotsFullOff = true;
        
        foreach (var element in p_HideSlots)
        {
            // 슬롯 한개라도 켜져있으면 변수 수정하고 반복문 탈출
            if (element.m_isOn)
            {
                isSlotsFullOff = false;
                break;
            }
        }

        // 콜라이더 켜집 걸정
        p_Collider.SetHideObjCollider(!isSlotsFullOff);
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
