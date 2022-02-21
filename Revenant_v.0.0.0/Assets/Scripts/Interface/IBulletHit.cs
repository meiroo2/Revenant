using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitPoints
{
    HEAD,
    BODY,
    OBJECT,
    OTHER
}

public interface IBulletHit
{
    public void BulletHit(float _damage, HitPoints hitPoints);
}