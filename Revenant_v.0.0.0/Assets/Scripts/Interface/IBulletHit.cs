using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletHit
{
    public void BulletHit(float _damage, int _hitPoint);
    // _hitPoint : 0 == Head / 1 == Body / 2 == Other
    
}