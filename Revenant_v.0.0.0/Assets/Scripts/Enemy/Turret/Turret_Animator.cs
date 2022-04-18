using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Animator : MonoBehaviour
{
    TutoRoom03EnemyMgr m_tutoEnemyMgr;

    Animator animator;

    private void Awake()
    {
        m_tutoEnemyMgr = GameObject.Find("TutoRoom3EnemyMgr").GetComponent<TutoRoom03EnemyMgr>();
        animator = GetComponent<Animator>();
    }

    public void AttackAnim()
    {
        animator.SetTrigger("isAttack");
    }
    public void ExitAnim()
    {
        animator.SetBool("isAlive", false);
    }

    //역재생 때문에 정상작동 안되는 것 같아 보류
    //public void AnimOverToProgress() // 애니메이션이 끝나 프로그래스 호출
    //{
    //    Debug.Log("AniOver");
    //    m_tutoEnemyMgr.SendToProgress();
    //}
}
