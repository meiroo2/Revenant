using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Human,
    Bullet,
    Room,
    Door,
    Floor,
    Stair,
    Portal,
    Objective,
    Item,
    Other
}

public class ObjectDefine : MonoBehaviour
{
    // Member Variables
    public ObjectType m_objectType { get; private set; } = ObjectType.Other;
    public bool m_canAttacked { get; set; } = false;
    public bool m_canUse { get; set; } = false;


    // Constructor
    public void InitObjectDefine(ObjectType _objectType, bool _canAttacked, bool _canUse)
    {
        m_objectType = _objectType;
        m_canAttacked = _canAttacked;
        m_canUse = _canUse;
    }
}