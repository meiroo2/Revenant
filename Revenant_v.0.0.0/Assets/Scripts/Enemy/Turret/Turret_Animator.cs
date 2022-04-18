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

    //����� ������ �����۵� �ȵǴ� �� ���� ����
    //public void AnimOverToProgress() // �ִϸ��̼��� ���� ���α׷��� ȣ��
    //{
    //    Debug.Log("AniOver");
    //    m_tutoEnemyMgr.SendToProgress();
    //}
}
