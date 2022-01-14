using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public GameObject toMove;
    public bool canMove = true;
    private float m_curDirection;

    public bool m_SetMoveStuck = true;

    private Rigidbody2D m_rigid;

    private bool m_AKey = false;
    private bool m_DKey = false;
    private int keyState = 0;
    

    private void Awake()
    {
        m_rigid = toMove.GetComponent<Rigidbody2D>();
        m_curDirection = 0;
    }

    private void Update()
    {
        if (m_SetMoveStuck)
            m_curDirection = Input.GetAxisRaw("Horizontal");
        else
        {
            if (Input.GetKey(KeyCode.A))
                m_AKey = true;
            else if (!Input.GetKey(KeyCode.A))
                m_AKey = false;

            if (Input.GetKey(KeyCode.D))
                m_DKey = true;
            else if (!Input.GetKey(KeyCode.D))
                m_DKey = false;

            if (keyState == 0 && m_AKey == true && m_DKey == false)
                keyState = 1;   // ¿ÞÂÊ
            else if (keyState == 0 && m_DKey == true && m_AKey == false)
                keyState = 2;   // ¿À¸¥ÂÊ
            else if (keyState == 2 && m_DKey == true && m_AKey == true)
                keyState = 3; // ¿À¸¥->¿ÞÂÊ
            else if (keyState == 1 && m_AKey == true && m_DKey == true)
                keyState = 4; // ¿Þ->¿À¸¥ÂÊ

            // ¿À¸¥->¿ÞÂÊ »óÅÂ
            else if (keyState == 3)
            {
                if (m_AKey == true && m_DKey == false)
                    keyState = 1;
                else if (m_AKey == false && m_DKey == true)
                    keyState = 2;
            }

            // ¿Þ->¿À¸¥ »óÅÂ
            else if (keyState == 4)
            {
                if (m_AKey == false && m_DKey == true)
                    keyState = 2;
                else if (m_AKey == true && m_DKey == false)
                    keyState = 1;
            }

            else if (m_AKey == false && m_DKey == false)
                keyState = 0;

            if (keyState == 1 || keyState == 3)
                m_curDirection = -1;
            else if (keyState == 2 || keyState == 4)
                m_curDirection = 1;
            else if (keyState == 0)
                m_curDirection = 0;
        }
    }

    private void FixedUpdate()
    {
        if (m_curDirection > 0)
            m_rigid.velocity = new Vector2(1, 0);
        else if (m_curDirection < 0)
            m_rigid.velocity = new Vector2(-1, 0);
        else
            m_rigid.velocity = Vector2.zero;
    }
}
