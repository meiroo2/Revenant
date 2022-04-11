using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTestCS : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_isPushRight = false;
    private bool m_isPushLeft = false;

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
    }

    private void FixedUpdate()
    {
        if (m_isPushLeft && !m_isPushRight)
        {
            m_Animator.SetBool("isRun", true);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.Translate(Vector2.left * 0.03f);
        }
        else if (m_isPushRight && !m_isPushLeft)
        {
            m_Animator.SetBool("isRun", true);
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.Translate(Vector2.right * 0.03f);
        }
        else
            m_Animator.SetBool("isRun", false);
    }
}
