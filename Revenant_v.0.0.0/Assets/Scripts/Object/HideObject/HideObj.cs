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
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
