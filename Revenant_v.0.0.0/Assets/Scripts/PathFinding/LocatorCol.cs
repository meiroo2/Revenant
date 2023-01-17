using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocatorCol : MonoBehaviour
{
    [ReadOnly]
    public int m_LocatorNum = 0;

    public List<Transform> m_MovePointList = new List<Transform>();
}