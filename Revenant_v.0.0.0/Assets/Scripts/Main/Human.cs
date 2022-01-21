using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum humanState
{
    Live,
    Dead,
    Stun,
    Pause
}

public class Human : ObjectDefine
{
    // Member Variables
    [field: SerializeField] public int m_Hp { get; set; } = 1;
    [field: SerializeField] public float m_Speed { get; set; } = 1f;
    [field: SerializeField] public float m_StunTime { get; set; } = 1f;
    [field: SerializeField] public bool m_hasStun { get; set; } = false;

    // Constructor
    private void Awake()
    {
        InitObjectDefine(ObjectType.Human, false, false);
    }
}
