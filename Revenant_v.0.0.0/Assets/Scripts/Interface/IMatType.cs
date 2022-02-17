using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatType
{
    Wood,
    Metal
}

public interface IMatType
{
    public MatType m_matType { get; set; }
}