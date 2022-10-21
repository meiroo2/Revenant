using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class SpriteController : MonoBehaviour
{
    // Visible Member Variables
    public SpriteRenderer[] m_Arr00;
    public SpriteRenderer[] m_Arr01;


    // Functions
    private int[] m_SpriteLengthArr = new int[2];
    private int m_Idx;
    public int m_AssignedSpriteListLimit { get; private set; } = 0;


    // Constructors


    // Functions
    public void SetSpriteAlpha(int _idx, float _alpha)
    {
        Color color = Color.white;
        color.a = _alpha;
        
        switch (_idx)
        {
            case 0:
                if (m_Arr00.Length <= 0)
                    return;

                for (int i = 0; i < m_Arr00.Length; i++)
                {
                    m_Arr00[i].color = color;
                }
                break;
            
            case 1:
                if (m_Arr01.Length <= 0)
                    return;
                
                for (int i = 0; i < m_Arr01.Length; i++)
                {
                    m_Arr01[i].color = color;
                }
                break;
        }
    }
}
