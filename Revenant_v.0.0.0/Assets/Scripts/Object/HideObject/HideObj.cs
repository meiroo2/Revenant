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

    [Space(20f)] [Header("Plz assign")]
    public SpriteRenderer m_Renderer;

    [field: SerializeField] public HideSlot p_LSlot { get; private set; }
    [field: SerializeField] public HideSlot p_RSlot { get; private set; }

    public HideObjCollider p_Collider;


    // Member Variables
    private bool m_Destructable = false;


    // Constructors
    private void Awake()
    {
        // 0보다 클 경우 파괴되는 엄폐물
        m_Destructable = m_Hp > 0;
        
        // 콜라이더에 부모 대입
        p_Collider.m_HideObj = this;
     
        
        // 각 Slot에 부모 스크립트(this) 넣어줌
        p_LSlot.m_HideObj = this;
        p_LSlot.m_isLeftSlot = true;
        p_RSlot.m_HideObj = this;
        p_RSlot.m_isLeftSlot = false;
    }


    // Updates


    // Physics


    // Functions
    public int GetSlotsInfo()
    {
        return p_LSlot.m_isOn switch
        {
            false when !p_RSlot.m_isOn => 0,
            true when !p_RSlot.m_isOn => 1,
            false when p_RSlot.m_isOn => 2,
            false when !p_RSlot.m_isOn => 3,
            _ => -1
        };
    }
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
        bool isSlotsFullOff = !(p_LSlot.m_isOn || p_RSlot.m_isOn);

        // 콜라이더 켜집 걸정
        p_Collider.SetHideObjCollider(!isSlotsFullOff);
        SetHideObjSpriteLayer(!isSlotsFullOff);
    }

    private void SetHideObjSpriteLayer(bool _toFrontObj)
    {
        m_Renderer.sortingLayerName = _toFrontObj ? "FrontObject" : "Object";
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
