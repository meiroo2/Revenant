using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;



public class MatChanger : MonoBehaviour
{
    // Visible Member Variables
    public Material[] p_ChangeMatArr;
    
    // Member Variables
    // 원본 보관용
    private Dictionary<SpriteRenderer, Material> m_PlayerMatDic = new Dictionary<SpriteRenderer, Material>();
    private Dictionary<SpriteRenderer, Material> m_EnemyMatDic = new Dictionary<SpriteRenderer, Material>();
    private Dictionary<SpriteRenderer, Material> m_UIMatDic = new Dictionary<SpriteRenderer, Material>();
    private Dictionary<SpriteRenderer, Material> m_OtherMatDic = new Dictionary<SpriteRenderer, Material>();
    private static readonly int unscaledTime = Shader.PropertyToID("UnscaledTime");

    private float m_Timer = 0f;

    private bool m_ChangeEnemy = false;
    private static readonly int manualTimer = Shader.PropertyToID("_ManualTimer");

    // Constructor
    private void Start()
    {
        m_Timer = 0f;
        
        var spriteRenArr = GameObject.FindObjectsOfType<SpriteRenderer>();
        for (int i = 0; i < spriteRenArr.Length; i++)
        {
            if (spriteRenArr[i].gameObject.CompareTag("Player"))
            {
                m_PlayerMatDic.Add(spriteRenArr[i], spriteRenArr[i].material);
            }
            else if (spriteRenArr[i].gameObject.CompareTag("Enemy"))
            {
                m_EnemyMatDic.Add(spriteRenArr[i], spriteRenArr[i].material);
            }
            else if (spriteRenArr[i].gameObject.CompareTag("UI"))
            {
                m_UIMatDic.Add(spriteRenArr[i], spriteRenArr[i].material);
            }
            else
            {
                m_OtherMatDic.Add(spriteRenArr[i], spriteRenArr[i].material);
            }
        }
    }
    
    // Updates
    public void Update()
    {
        
        if (!m_ChangeEnemy)
            return;
        
        m_Timer += Time.unscaledDeltaTime;
        
        foreach (var VARIABLE in m_EnemyMatDic)
        {
            VARIABLE.Key.material.SetFloat(manualTimer, m_Timer);
        }
        
        /*
        var enumerator = m_EnemyMatDic.GetEnumerator();
        enumerator.MoveNext();
        enumerator.Current.Key.sharedMaterial.SetFloat(unscaledTime, Time.unscaledDeltaTime);
        */
    }

    // Functions
    
    /// <summary>
    /// 특정한 SpriteRenderer의 머터리얼을 변경합니다.
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_renderer"></param>
    /// <param name="_idx"></param>
    public void ChangeMat(ObjectType _type, SpriteRenderer _renderer, int _idx)
    {
        switch (_type)
        {
            case ObjectType.Enemy:
                if (m_EnemyMatDic.ContainsKey(_renderer))
                {
                    Debug.Log("Sans");
                    _renderer.material = p_ChangeMatArr[_idx];
                }
                break;
        }
    }
    
    /// <summary>
    /// 머터리얼을 원하는 것으로 변경합니다.
    /// </summary>
    /// <param name="_type">Player, Enemy, UI, Other만 사용바람</param>
    /// <param name="_idx">0 = 흑백, 1 = 홀로그램, 2 = 사라지는거</param>
    [Button(ButtonSizes.Medium)]
    public void ChangeMat(ObjectType _type, int _idx)
    {
        switch (_type)
        {
            case ObjectType.Player:
                foreach (var variable in m_PlayerMatDic)
                    variable.Key.material = p_ChangeMatArr[_idx];
                break;
            
            case ObjectType.Enemy:
                foreach (var variable in m_EnemyMatDic)
                    variable.Key.material = p_ChangeMatArr[_idx];

                m_ChangeEnemy = true;
                break;
            
            case ObjectType.UI:
                foreach (var variable in m_UIMatDic)
                    variable.Key.material = p_ChangeMatArr[_idx];
                break;
            
            case ObjectType.Other:
                foreach (var variable in m_OtherMatDic)
                    variable.Key.material = p_ChangeMatArr[_idx];
                break;
        }
    }

    /// <summary>
    /// 머터리얼을 되돌립니다.
    /// </summary>
    /// <param name="_type">원하는 타입</param>
    [Button(ButtonSizes.Medium)]
    public void RestoreMat(ObjectType _type)
    {
        switch (_type)
        {
            case ObjectType.Player:
                foreach (var variable in m_PlayerMatDic)
                    variable.Key.material = variable.Value;
                break;
            
            case ObjectType.Enemy:
                foreach (var variable in m_EnemyMatDic)
                    variable.Key.material = variable.Value;
                break;
            
            case ObjectType.UI:
                foreach (var variable in m_UIMatDic)
                    variable.Key.material = variable.Value;
                break;
            
            case ObjectType.Other:
                foreach (var variable in m_OtherMatDic)
                    variable.Key.material = variable.Value;
                break;
        }
    }
}