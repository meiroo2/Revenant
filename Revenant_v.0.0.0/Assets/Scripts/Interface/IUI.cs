using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUIParam
{
    public bool[] m_IsModified { get; private set; } = { false, false, false, false, false };

    public bool m_ToActive { get; private set; } = true;
    public string m_AnimatorParam { get; private set; } = "";
    public int m_AnimatorParamValue { get; private set; } = 0;
    public Transform m_Position { get; private set; }



    public IUIParam(bool _toActive)
    {
        m_ToActive = _toActive;
        m_IsModified[0] = true;
    }
    public IUIParam(string _aniParam, int _aniParamValue)
    {
        m_AnimatorParam = _aniParam;
        m_AnimatorParamValue = _aniParamValue;
        m_IsModified[2] = true;
        m_IsModified[3] = true;
    }
    public IUIParam(Transform _pinPos)
    {
        m_Position = _pinPos;
        m_IsModified[4] = true;
    }
}

public interface IUI
{
    public bool m_isActive { get; set; }
}