using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTestCS : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_isPushRight = false;
    private bool m_isPushLeft = false;

    private float m_KeyTimer = 0.15f;

    private int m_curDirection = 0;
    private bool m_HaveToChangeScale = false;

    private bool m_canMove = false;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_isPushRight = true;

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            m_isPushRight = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_isPushLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            m_isPushLeft = false;
        }

        if (m_HaveToChangeScale)
        {
            if (m_curDirection == 1)
            {
                m_Animator.SetInteger("isTurn", 1);
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
                    if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        Debug.Log("오른쪽->왼쪽 방향전환완료");
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        m_Animator.SetInteger("isTurn", 0);
                        m_curDirection = -1;
                        m_HaveToChangeScale = false;
                    }
            }
            else if (m_curDirection == -1)
            {
                m_Animator.SetInteger("isTurn", 1);
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
                    if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        Debug.Log("왼쪽->어ㅗ른쪽 방향전환완료");
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        m_Animator.SetInteger("isTurn", 0);
                        m_curDirection = 1;
                        m_HaveToChangeScale = false;
                    }
            }
        }
    }
    private void FixedUpdate()
    {
        if (m_isPushLeft && !m_isPushRight)
        {
            m_KeyTimer = 0.15f;
            if (!m_HaveToChangeScale)
            {
                switch (m_curDirection)
                {
                    case 0:
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        m_Animator.SetBool("isRun", true);
                        m_curDirection = -1;
                        break;
                    case -1:
                        moveChar(false);
                        break;
                    case 1:
                        m_HaveToChangeScale = true;
                        break;
                }
            }
        }
        else if (m_isPushRight && !m_isPushLeft)
        {
            m_KeyTimer = 0.15f;
            if (!m_HaveToChangeScale)
            {
                switch (m_curDirection)
                {
                    case 0:
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        m_Animator.SetBool("isRun", true);
                        m_curDirection = 1;
                        break;
                    case -1:
                        m_HaveToChangeScale = true;
                        break;
                    case 1:
                        moveChar(true);
                        break;
                }
            }
        }
        else if (m_curDirection != 0)
        {
            m_KeyTimer -= Time.deltaTime;
            if (m_KeyTimer <= 0f)
            {
                Debug.Log("멈춰");
                m_Animator.SetBool("isRun", false);
                m_KeyTimer = 0.15f;
                m_curDirection = 0;
            }
        }
    }


    private void moveChar(bool _isRight)
    {
            if (_isRight)
            {
                transform.Translate(Vector2.right * 0.03f);
            }
            else
            {
                transform.Translate(Vector2.left * 0.03f);
            }
    }
}