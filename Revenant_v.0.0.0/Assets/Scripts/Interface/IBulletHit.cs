using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitPoints
{
    HEAD,
    BODY,
    OTHER
}

public interface IBulletHit
{ 
    public void BulletHit(float _damage, HitPoints hitPoints);
}