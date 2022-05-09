using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Animator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HitBodyAni()
    {
        animator.SetTrigger("hitBody");
    }
    public void HitTargetAni()
    {
        animator.SetTrigger("hitTarget");
    }

    public void HitMainBodyAni()
    {
        Debug.Log("no ��ü �ı� Animation File");
        //animator.SetTrigger("hitBody");
    }
    public void HitBombAni()
    {
        Debug.Log("no ���� ���� Animation File");
        HitBodyAni();
        //animator.SetTrigger("hitBomb");
    }
    public void GroundBombAni() 
    {
        Debug.Log("no ���� ���� Animation File");
        HitBodyAni();
    }
}

