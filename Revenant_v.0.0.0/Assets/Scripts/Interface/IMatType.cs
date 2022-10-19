using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatType
{
    Normal,
    Dirt,
    Wood,
    Metal,
    Flesh
}

public interface IMatType
{
    public MatType m_matType { get; set; }
}