using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUIParam
{
    public int m_ToActive { get; private set; }
    public Vector2 m_Position { get; private set; }

    public IUIParam(int _toActive)
    {
        m_ToActive = _toActive;
        m_Position = Vector2.zero;
    }
    public IUIParam(int _toActive, Vector2 _Position)
    {
        m_ToActive = _toActive;
        m_Position = _Position;
    }
}

public interface IUI
{
    public bool m_isOn { get; set; }
    public int ActivateUI(IUIParam _input) { return 0; }
}