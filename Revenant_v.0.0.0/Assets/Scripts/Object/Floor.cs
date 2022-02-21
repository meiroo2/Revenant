using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IBulletHit
{
    // Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Wood;

    // Constructors
    private void Awake()
    {
        InitObjectDefine(ObjectType.Floor, true, false);
    }

    // Functions
    public void BulletHit(float _damage, HitPoints _hitPoints)
    {
        Debug.Log(m_matType.ToString() + " ������ �ٴڿ� " + _damage.ToString() + "�������� �Ѿ��� ����!");
    }
}