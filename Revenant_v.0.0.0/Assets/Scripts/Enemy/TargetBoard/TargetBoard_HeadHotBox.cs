using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TargetBoard_HeadHotBox : MonoBehaviour, IHotBox
{
    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; }

    
    TargetBoard_Controller targetBoard;

    private void Awake()
    {
        targetBoard = GetComponentInParent<TargetBoard_Controller>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        targetBoard.m_targetboard_animator.HitHeadAni();
        targetBoard.HotBoxToggle(false);

        targetBoard.Attacked();

        return 1;
    }

}
