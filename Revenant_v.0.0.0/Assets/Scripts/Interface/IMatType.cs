using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatType
{
    Normal,
    Wood,
    Metal,
    Target_Head,
    Target_Body
}

public interface IMatType
{
    public MatType m_matType { get; set; }
}