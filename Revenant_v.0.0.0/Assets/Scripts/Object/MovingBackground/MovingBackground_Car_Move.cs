using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground_Car_Move : MovingBackground_Move
{
    enum ECarState
    {
        Appear = 0,
        Break,
        Open,
        MAX
    }
    
    [SerializeField] private float Speed = 2.0f;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, EndTransform.position, Speed * Time.deltaTime);
    }

    private void Brake()
    {
        if (GetCurrentDistanceToEndTransform <= 1.2f && Speed > 0f)
        {
            Speed -= Time.deltaTime * (Speed - 0.5f);
        }
    }

    private void PlayBreakAnimation()
    {
        if (GetCurrentDistanceToEndTransform < 0.2f)
        {
            _animator.SetInteger("CarState", (int)ECarState.Break);
        }
    }
    
    protected override IEnumerator MoveEnumerator()
    {
        while (true)
        {
            Move();
            Brake();
            PlayBreakAnimation();

            if(IsReachedEndTransform())
            {
                ArrivedEndTransform();
                break;
            }
            
            yield return null;
        }
    }
}
