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

public class Player : Human, IBulletHit
{
    // Member Variables
    [field: SerializeField] public playerState m_curPlayerState { get; private set; } = playerState.IDLE;
    [field: SerializeField] public bool m_canMove { get; private set; } = true;
    [field: SerializeField] public bool m_canRoll { get; private set; } = true;
    [field: SerializeField] public bool m_canShot { get; private set; } = true;

    public float m_BackWalkSpeedRatio = 0.7f;
    public float m_RunSpeedRatio = 1.5f;

    public Vector2 m_playerMoveVec { get; private set; } = new Vector2(0f, 0f);

    private PlayerRotation m_playerRotation;
    private Rigidbody2D m_playerRigid;
    private PlayerSoundnAni m_playerSoundnAni;
    private Player_Gun m_playerGun;

    // Constructor
    private void Awake()
    {
        m_playerRigid = GetComponent<Rigidbody2D>();
        m_playerSoundnAni = GetComponent<PlayerSoundnAni>();
        m_playerRotation = GetComponentInChildren<PlayerRotation>();
        m_playerGun = GetComponentInChildren<Player_Gun>();
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
                    if ((m_isRightHeaded ? 1 : -1) == (int)m_playerMoveVec.x)
                    {
                        m_playerRigid.velocity = m_playerMoveVec * m_Speed;
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                            changePlayerFSM(playerState.RUN);
                    }
                    else
                        m_playerRigid.velocity = m_playerMoveVec * m_Speed * m_BackWalkSpeedRatio;
                }
                break;

            case playerState.RUN:
                if (m_isRightHeaded)
                    m_playerRigid.velocity = new Vector2(m_Speed * m_RunSpeedRatio, 0f);
                else
                    m_playerRigid.velocity = new Vector2(-m_Speed * m_RunSpeedRatio, 0f);

                if (Input.GetKeyUp(KeyCode.LeftShift))
                    changePlayerFSM(playerState.WALK);
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
                m_playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                break;

            case playerState.WALK:
                m_curPlayerState = playerState.WALK;
                break;

            case playerState.RUN:
                m_curPlayerState = playerState.RUN;
                m_playerRotation.m_doRotate = false;
                m_playerGun.m_canShot = false;
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
                m_playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;

            case playerState.WALK:
                m_playerRigid.velocity = Vector2.zero;
                break;

            case playerState.RUN:
                m_playerRotation.m_doRotate = true;
                m_playerGun.m_canShot = true;
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
    public void BulletHit(float _damage, int _hitPoint)
    {
        Debug.Log("플레이어에게 " + _damage.ToString() + " 데미지의 총알이 맞음!");
    }
}