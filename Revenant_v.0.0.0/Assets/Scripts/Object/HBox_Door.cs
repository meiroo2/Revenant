using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBox_Door : MonoBehaviour, IHotBox
{
    private void Awake()
    {
        GetComponentInParent<Door>().m_DoorCollision = GetComponent<BoxCollider2D>();
    }

    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = false;

    public int HitHotBox(IHotBoxParam _param)
    {
        return 1;
    }
}
