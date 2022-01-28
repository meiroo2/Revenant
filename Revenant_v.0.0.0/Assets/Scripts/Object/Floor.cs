using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, MatTypeInterface
{
    // Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Wood;

    // Constructor
    private void Awake()
    {
        InitObjectDefine(ObjectType.Floor, true, false);
    }
}