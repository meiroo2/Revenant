using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TargetBoard_HeadHotBox : MonoBehaviour, IHotBox
{

    TutoEnemyMgr m_tutoEnemyMgr;
    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; }
    TargetBoard_Controller targetBoard;

    private void Awake()
    {
        targetBoard = GetComponentInParent<TargetBoard_Controller>();
        m_tutoEnemyMgr = GameObject.Find("TutoEnemyMgr").GetComponent<TutoEnemyMgr>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        targetBoard.m_targetboard_animator.HitHeadAni();
        m_tutoEnemyMgr.PlusDieCount();
    }

}
