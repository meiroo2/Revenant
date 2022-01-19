using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public PlayerAnimController m_PlayerAnimcont;
    public GameObject toMove;
    public bool canMove = true;
    private float m_curDirection;

    public bool m_SetMoveStuck = true;

    private Rigidbody2D m_rigid;

    

    private void Awake()
    {
        m_rigid = toMove.GetComponent<Rigidbody2D>();
        m_curDirection = 0;
    }

    private void Update()
    {
        if (canMove)
            m_curDirection = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (m_curDirection > 0)
        {
            m_rigid.velocity = new Vector2(1, 0);
            m_PlayerAnimcont.setPlayerAnimState(PlayerAnimController.PlayerAnimState.WALK);
        } 
        else if (m_curDirection < 0)
        {
            m_rigid.velocity = new Vector2(-1, 0);
            m_PlayerAnimcont.setPlayerAnimState(PlayerAnimController.PlayerAnimState.WALK);
        }
        else
        {
            m_rigid.velocity = Vector2.zero;
            m_PlayerAnimcont.setPlayerAnimState(PlayerAnimController.PlayerAnimState.IDLE);
        }
    }
}
