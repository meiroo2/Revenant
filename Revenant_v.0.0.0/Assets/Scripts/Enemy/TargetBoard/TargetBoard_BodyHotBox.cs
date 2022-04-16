using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard_BodyHotBox : MonoBehaviour, IHotBox
{
    TargetBoard_Controller targetBoard;

    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; } = 0;

    private void Awake()
    {
        targetBoard = GetComponentInParent<TargetBoard_Controller>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        targetBoard.m_targetboard_animator.HitBodyAni();
        targetBoard.HotBoxToggle(false);


        targetBoard.Attacked();
    }

}
