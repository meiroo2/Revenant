using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBullet
{
    public bool isPlayers { get; set; }
    public int m_Damage { get; set; }

    public void setIBullet(bool _isPlayers, int _Damage) { }
}