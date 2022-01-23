using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState
{
    IDLE,
    WALK,
    RUN,
    ROLL,
    HIDDEN,
    HIDDEN_STAND,
    INPORTAL,
    DEAD
}

public class Player : Human
{
    // Member Variables
    [field: SerializeField] public playerState m_curPlayerState { get; private set; } = playerState.IDLE;
    [field: SerializeField] public bool m_canMove { get; private set; } = true;
    [field: SerializeField] public bool m_canRoll { get; private set; } = true;
    [field: SerializeField] public bool m_canShot { get; private set; } = true;

    public Vector2 m_playerMoveVec { get; private set; } = new Vector2(0f, 0f);

    private Rigidbody2D m_playerRigid;
    public PlayerSoundnAni m_playerSoundnAni;

    // Constructor
    private void Awake()
    {
        m_playerRigid = GetComponent<Rigidbody2D>();
        m_playerSoundnAni = GetComponent<PlayerSoundnAni>();
    }

    // Update
    private void Update()
    {
        updatePlayerFSM();
    }
    private void FixedUpdate()
    {
        
    }


    // Player FSM Functions
    private void updatePlayerFSM()
    {
        switch (m_curPlayerState)
        {
            case playerState.IDLE:
                m_playerMoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), m_playerMoveVec.y);
                if (m_playerMoveVec.x != 0)
                    changePlayerFSM(playerState.WALK);
                break;

            case playerState.WALK:
                m_playerMoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), m_playerMoveVec.y);
                if (m_playerMoveVec.x == 0)
                    changePlayerFSM(playerState.IDLE);
                else
                {
                    m_playerRigid.velocity = m_playerMoveVec * m_Speed;
                }
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
                break;

            case playerState.HIDDEN:
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }
    public void changePlayerFSM(playerState _inputPlayerState)
    {
        exitPlayerFSM();

        switch (_inputPlayerState)
        {
            case playerState.IDLE:
                m_curPlayerState = playerState.IDLE;
                break;

            case playerState.WALK:
                m_curPlayerState = playerState.WALK;
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
                break;

            case playerState.HIDDEN:
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }

        m_playerSoundnAni.playplayerAnim();
    }
    private void exitPlayerFSM()
    {
        switch (m_curPlayerState)
        {
            case playerState.IDLE:
                break;

            case playerState.WALK:
                m_playerRigid.velocity = Vector2.zero;
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
                break;

            case playerState.HIDDEN:
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }

    // Functions
}