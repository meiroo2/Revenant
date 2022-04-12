using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Controller : Enemy
{
    [SerializeField]
    Drone_Animator drone_animator;
    
    Rigidbody2D rigid;

    
    //[SerializeField]
    //float m_stopTime = 0.1f;
    Vector2 m_moveDir;
    int m_dir = 0;
    float fixedvalue = 0.05f;

    [SerializeField]
    float m_moveTime = 0.2f;
    [SerializeField]
    float m_moveSpeed = 1;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        // move -> stop -> change
        //Invoke(nameof(Stop), m_moveTime);
        Invoke(nameof(ChangeDir), m_moveTime);
        m_moveDir = Vector2.up;
    }

    private void FixedUpdate()
    {
        Idle();
    }

    // FSM
    public override void Idle()
    {
        Move();
    }

    public void Move()
    {
        rigid.velocity += m_moveDir * m_moveSpeed * fixedvalue;
    }

    public void Stop()
    {
        // stop -> change
        rigid.velocity = Vector2.zero;
        m_moveDir = Vector2.zero;
        //Invoke(nameof(ChangeDir), m_stopTime);
    }

    //_dir = 0: up, 1: down, 2: left, 3: right
    public void ChangeDir()
    {
        Stop();
        m_dir = (m_dir == 0) ? 1 : 0;

        switch (m_dir)
        {
            case 0:
                m_moveDir = Vector2.up;
                break;
            case 1:
                m_moveDir = Vector2.down;
                break;
            case 2:
                m_moveDir = Vector2.left;
                break;
            case 3:
                m_moveDir = Vector2.right;
                break;
            default:
                Debug.Log("Invalid _dir(int)");
                break;
        }

        // change -> move -> stop
        Invoke(nameof(ChangeDir), m_moveTime);
    }

    // Hit Effects: Body - die
    public void BodyAttacked()
    {
        CancelInvoke(nameof(ChangeDir));
        Stop();
        drone_animator.HitBody();

    }

    // Hit Effects: Target - no die
    public void TargetAttacked()
    {
        drone_animator.HitTarget();

    }
}
