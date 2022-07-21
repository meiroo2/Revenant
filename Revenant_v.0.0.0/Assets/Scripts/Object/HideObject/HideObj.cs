using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEditor;
using UnityEngine;

public class HideObj : MonoBehaviour
{
    // Visible Member Variables
    [Tooltip("0���� ������ �ı����� �ʽ��ϴ�.")]
    public int m_Hp = 0;

    [Space(20f)] [Header("Plz assign")] 
    public HideSlot[] p_HideSlots;
    public HideObjCollider p_Collider;
    [field : SerializeField]public Sprite p_DefaultSprite { get; private set; }
    [field : SerializeField]public Sprite p_HighlightSprite { get; private set; }


    // Member Variables
    private bool m_Destructable = false;
    public SpriteRenderer m_Renderer { get; private set; }


    // Constructors
    private void Awake()
    {
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        
        // 0���� Ŭ ��� �ı��Ǵ� ����
        m_Destructable = m_Hp > 0;
        
        // �ݶ��̴��� �θ� ����
        p_Collider.m_HideObj = this;
        
        if (p_HideSlots.Length <= 0)
        {
            Debug.Log(gameObject.name + "�� HideSlot ������ �Ǿ����� ����");
            return;
        }
        
        // �� Slot�� �θ� ��ũ��Ʈ(this) �־���
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
                // �ı� ������ ��ü�� �̹� Hp�� 0 ������ ���
                return 0;
            
            case true:
                // �ı� ������ ��ü
                m_Hp -= _param.m_Damage;

                if (m_Hp <= 0)
                {
                    // �ı� ���� �����
                    
                    // Gameobject Disable 
                    gameObject.SetActive(false);
                }

                return 1;
                break;
            
            case false:
                // �ı� �Ұ��� ��ü
                return 1;
                break;
            
            default:
                return 0;
        }
    }

    public void UpdateHideSlotInfo()
    {
        // ���� ��ü ���� ���·� ����
        var isSlotsFullOff = true;
        
        foreach (var element in p_HideSlots)
        {
            // ���� �Ѱ��� ���������� ���� �����ϰ� �ݺ��� Ż��
            if (element.m_isOn)
            {
                isSlotsFullOff = false;
                break;
            }
        }

        // �ݶ��̴� ���� ����
        p_Collider.SetHideObjCollider(!isSlotsFullOff);
        SetHideObjSpriteLayer(!isSlotsFullOff);
    }

    private void SetHideObjSpriteLayer(bool _toStair)
    {
        m_Renderer.sortingLayerName = _toStair ? "Stair" : "Object";
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
