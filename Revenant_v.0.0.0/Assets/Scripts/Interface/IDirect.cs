using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirect
{
    public int m_Idx { get; set; }
    public int NextDirect();
}
