using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Player,
    Enemy,
    UI,
    Bullet,
    Room,
    Door,
    Floor,
    Stair,
    Portal,
    Objective,
    Item,
    BackGround,
    Other
}

public enum ObjectState
{
    Active,
    Pause
}

public class ObjectDefine : MonoBehaviour
{
    // Member Variables
    public ObjectType m_ObjectType { get; protected set; } = ObjectType.Other;
    public ObjectState m_ObjectState { get; protected set; } = ObjectState.Active;
    public bool m_CanAttacked { get; protected set; } = false;
    
    private bool[] m_PrevMemberVariables = new bool[1];
    
    // Functions
    public void setObjectState(ObjectState _state)
    {
        if (_state == ObjectState.Pause)
        {
            m_PrevMemberVariables[0] = m_CanAttacked;
            m_CanAttacked = false;
            m_ObjectState = _state;
        }
        else
        {
            m_CanAttacked = m_PrevMemberVariables[0];
            m_ObjectState = _state;
        }
    }
}