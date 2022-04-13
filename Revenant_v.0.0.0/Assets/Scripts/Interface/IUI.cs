using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUIParam
{
    public bool[] m_Modifiedcount { get; private set; } = new bool[5];

    public bool m_ToActive { get; private set; } = true;
    public string m_AnimatorParam { get; private set; } = "";
    public int m_AnimatorParamValue { get; private set; } = 0;
    public Vector2 m_Position { get; private set; } = Vector2.zero;
    public bool m_isPinned { get; private set; } = false;

    public IUIParam(bool _toActive)
    {
        for (int i = 0; i < m_Modifiedcount.Length; i++)
        {
            m_Modifiedcount[i] = false;
        }
        m_Modifiedcount[0] = true;

        m_ToActive = _toActive;
    }
    public IUIParam(string _aniParam, int _aniParamValue)
    {

            m_AnimatorParam = _aniParam;
            m_AnimatorParamValue = _aniParamValue;

    }
    public IUIParam(Vector2 _pinPos, bool _isPin)
    {
        if (m_Modifiedcount == 0)
        {
            m_ToActive = true;
            m_AnimatorParam = "";
            m_AnimatorParamValue = 0;
            m_Position = _pinPos;
            m_isPinned = _isPin;
        }
        else
        {
            m_Position = _pinPos;
            m_isPinned = _isPin;
        }

        m_Modifiedcount++;
    }
    public IUIParam(bool _toActive, string _aniParam, int _aniParamValue, Vector2 _pinPos, bool _isPin)
    {
        m_ToActive = _toActive;
        m_AnimatorParam = _aniParam;
        m_AnimatorParamValue = _aniParamValue;
        m_Position = _pinPos;
        m_isPinned = _isPin;

        m_Modifiedcount++;
    }
}

public interface IUI
{
    public bool m_isActive { get; set; }
    public int ActivateUI(IUIParam _input) { return 0; }
}