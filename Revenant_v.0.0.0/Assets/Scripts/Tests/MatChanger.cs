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
    public Material p_ToChangeMat;
    
    // Member Variables
    private Dictionary<SpriteRenderer, Material> m_SpriteDic = new Dictionary<SpriteRenderer, Material>();

    // Constructor
    private void Start()
    {
        var spriteRenArr = GameObject.FindObjectsOfType<SpriteRenderer>();

        for (int i = 0; i < spriteRenArr.Length; i++)
        {
            if (!spriteRenArr[i].gameObject.CompareTag("Player") && !spriteRenArr[i].gameObject.CompareTag("UI"))
            {
                m_SpriteDic.Add(spriteRenArr[i], spriteRenArr[i].material);
            }
        }
    }
    
    // Functions
    [Button(ButtonSizes.Medium)]
    public void ChangeMat()
    {
        foreach (var variable in m_SpriteDic)
        {
            variable.Key.material = p_ToChangeMat;
        }
    }

    [Button(ButtonSizes.Medium)]
    public void ResotreMat()
    {
        foreach (var variable in m_SpriteDic)
        {
            variable.Key.material = variable.Value;
        }
    }
}