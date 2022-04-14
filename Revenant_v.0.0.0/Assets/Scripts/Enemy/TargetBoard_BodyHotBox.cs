using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard_BodyHotBox : MonoBehaviour, IHotBox
{
    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; }
    TargetBoard targetBoard;

    private void Awake()
    {
        targetBoard = GetComponentInParent<TargetBoard>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        targetBoard.hitParts = PARTS.BODY;
        targetBoard.Attacked();
    }
}