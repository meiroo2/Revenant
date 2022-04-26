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
    // Visable Member Variables
    [field: SerializeField] public float m_RollSpeedRatio { get; private set; } = 1.3f;
    [field: SerializeField] public float m_BackWalkSpeedRatio { get; private set; } = 0.7f;
    [field: SerializeField] public float m_RunSpeedRatio { get; private set; } = 1.5f;
    [field: SerializeField] public float m_RollRecoverTime { get; private set; } = 2f;
    [field: SerializeField] public int m_MaxRollCount { get; private set; } = 3;
    public int m_LeftRollCount { get; private set; }

    //[Space(20f)]
    //[Header("Public Player Scripts")]

    public playerState m_curPlayerState { get; private set; } = playerState.IDLE;
    public bool m_canMove { get; private set; } = true;
    public bool m_canRoll { get; private set; } = true;
    public bool m_canShot { get; private set; } = true;
    public bool m_canChangeWeapon { get; private set; } = false;
    public Vector2 m_playerMoveVec { get; private set; } = new Vector2(0f, 0f);

    public PlayerSoundnAni m_playerSoundnAni { get; private set; }
    public PlayerRotation m_playerRotation { get; private set; }
    public Player_Gun m_playerGun { get; private set; }
    public Player_UseRange m_useRange { get; private set; }

    private Player_UIMgr m_PlayerUIMgr;
    private UIMgr m_UIMgr;
    private NoiseMaker m_NoiseMaker;
    private Rigidbody2D m_playerRigid;
    private SoundMgr_SFX m_SFXMgr;
    private Animator m_PlayerAnimator;
    private Player_StairMgr m_PlayerStairMgr;

    private bool m_isRecoveringRollCount = false;
    private IEnumerator m_FootStep;

    private bool m_isStairLerping = false;
    private float m_StairLerpTimer = 0.5f;
    private Vector2 m_StairTelePos;

    private Vector2 m_PlayerPosVec;

    private RaycastHit2D m_FootRay;

    // For Player_Managers

    // Constructor
    private void Awake()
    {
        m_PlayerStairMgr = GetComponentInChildren<Player_StairMgr>();
        m_playerSoundnAni = GetComponentInChildren<PlayerSoundnAni>();
        m_playerRotation = GetComponentInChildren<PlayerRotation>();
        m_playerGun = GetComponentInChildren<Player_Gun>();
        m_useRange = GetComponentInChildren<Player_UseRange>();

        m_PlayerAnimator = GetComponent<Animator>();
        m_playerRigid = GetComponent<Rigidbody2D>();

        m_LeftRollCount = m_MaxRollCount;
    }
    private void Start()
    {
        m_NoiseMaker = GameManager.GetInstance().GetComponentInChildren<NoiseMaker>();
        m_UIMgr = GameManager.GetInstance().GetComponentInChildren<UIMgr>();
        m_PlayerUIMgr = GameManager.GetInstance().GetComponentInChildren<Player_UIMgr>();
        m_SFXMgr = GameManager.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }
    public void InitPlayerValue(Player_ValueManipulator _input)
    {
        m_Hp = _input.Hp;
        m_stunTime = _input.StunInvincibleTime;
        m_Speed = _input.Speed;
        m_BackWalkSpeedRatio = _input.BackSpeedRatio;
        m_RunSpeedRatio = _input.RunSpeedRatio;
        m_RollSpeedRatio = _input.RollSpeedRatio;
        m_MaxRollCount = _input.RollCountMax;
        m_LeftRollCount = m_MaxRollCount;
        m_RollRecoverTime = _input.RollRecoverTime;
    }


    // Update
    private void Update()
    {
        updatePlayerFSM();
        if (m_isStairLerping)
        {
            m_StairLerpTimer -= 0.1f;
            transform.position = Vector2.Lerp(transform.position, m_StairTelePos, Time.deltaTime * 3f);
            if (m_StairLerpTimer <= 0f)
            {
                m_StairLerpTimer = 0.5f;
                m_isStairLerping = false;
            }
        }

        m_PlayerPosVec = transform.position;

        //m_PlayerPosVec = StaticMethods.returnPixelPerfectPos(m_PlayerPosVec);
        if (gameObject.layer == 12)
            m_FootRay = Physics2D.Raycast(m_PlayerPosVec, -transform.up, 0.5f, LayerMask.GetMask("Floor"));
        else if(gameObject.layer == 10)
            m_FootRay = Physics2D.Raycast(m_PlayerPosVec, -transform.up, 0.5f, LayerMask.GetMask("Stair"));



        Debug.DrawRay(m_PlayerPosVec, Vector2.down * 0.5f, new Color(0, 1, 0));


        if (gameObject.layer == 12)
        {
            if (m_PlayerPosVec.y - m_FootRay.point.y >= 0.33f)
                m_PlayerPosVec.y = m_FootRay.point.y + 0.32f;
        }
        else if (gameObject.layer == 10)
        {
            //m_PlayerPosVec.y = m_FootRay.point.y + 0.39f;
        }

        transform.position = StaticMethods.getPixelPerfectPos(m_PlayerPosVec);
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
                if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                    changePlayerFSM(playerState.ROLL);
                break;

            case playerState.WALK:
                m_playerMoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), m_playerMoveVec.y);
                if (m_playerMoveVec.x == 0)
                    changePlayerFSM(playerState.IDLE);
                else
                {
                    if ((m_isRightHeaded ? 1 : -1) == (int)m_playerMoveVec.x)
                    {
                        if (m_isRightHeaded)
                            m_playerRigid.velocity = -new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_Speed;
                        else
                            m_playerRigid.velocity = new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_Speed;

                        if (Input.GetKeyDown(KeyCode.LeftShift))
                            changePlayerFSM(playerState.RUN);
                        else if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                            changePlayerFSM(playerState.ROLL);
                    }
                    else
                    {
                        if (m_isRightHeaded)
                        {
                            m_playerRigid.velocity = StaticMethods.getLPerpVec(m_PlayerStairMgr.m_PlayerNormal) * m_Speed * m_BackWalkSpeedRatio;
                            if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                            {
                                setisRightHeaded(false);
                                changePlayerFSM(playerState.ROLL);
                            }
                        }
                        else
                        {
                            m_playerRigid.velocity = -StaticMethods.getLPerpVec(m_PlayerStairMgr.m_PlayerNormal) * m_Speed * m_BackWalkSpeedRatio;
                            if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                            {
                                setisRightHeaded(true);
                                changePlayerFSM(playerState.ROLL);
                            }
                        }
                    }
                       
                }
                break;

            case playerState.RUN:
                if (m_isRightHeaded)
                    m_playerRigid.velocity = new Vector2(m_Speed * m_RunSpeedRatio, 0f);
                else
                    m_playerRigid.velocity = new Vector2(-m_Speed * m_RunSpeedRatio, 0f);

                if (Input.GetKeyUp(KeyCode.LeftShift))
                    changePlayerFSM(playerState.WALK);
                else if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                    changePlayerFSM(playerState.ROLL);
                break;

            case playerState.ROLL:
                if (m_isRightHeaded)
                {
                    m_playerRigid.velocity = -new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_RollSpeedRatio;
                }
                else
                {
                    m_playerRigid.velocity = new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_RollSpeedRatio;
                }
                if (m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    changePlayerFSM(playerState.WALK);
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
                m_canAttacked = true;
                break;

            case playerState.WALK:
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(0.8f, 0.1f));
                StartCoroutine(m_FootStep);
                m_curPlayerState = playerState.WALK;
                break;

            case playerState.RUN:
                /*
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(1f, 0.3f));
                StartCoroutine(m_FootStep);
                m_curPlayerState = playerState.RUN;
                m_playerRotation.m_doRotate = false;
                m_canShot = false;
                */
                break;

            case playerState.ROLL:
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(1f, 0.3f));
                StartCoroutine(m_FootStep);

                m_LeftRollCount -= 1;
                if (!m_isRecoveringRollCount)
                    StartCoroutine(RecoverRollCount());

                if (!m_canAttacked)
                    CancelInvoke(nameof(SetPlayerCanAttacked));
                m_canAttacked = false;

                m_curPlayerState = playerState.ROLL;
                m_playerRotation.m_doRotate = false;
                m_canShot = false;
                m_canMove = false;

                m_playerSoundnAni.setSprites(true, false, false, false, false);
                break;

            case playerState.HIDDEN:
                m_curPlayerState = playerState.HIDDEN;
                m_playerSoundnAni.setSprites(true, false, false, false, false);
                m_playerRotation.m_doRotate = false;
                m_canShot = false;
                m_canMove = false;
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                m_curPlayerState = playerState.DEAD;
                m_playerRotation.m_doRotate = false;
                m_canMove = false;
                m_canShot = false;
                break;
        }

        m_playerSoundnAni.playplayerAnim();
    }
    private void exitPlayerFSM()
    {
        m_playerSoundnAni.exitplayerAnim();
        switch (m_curPlayerState)
        {
            case playerState.IDLE:
                m_playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;

            case playerState.WALK:
                StopCoroutine(m_FootStep);
                m_playerRigid.velocity = Vector2.zero;
                break;

            case playerState.RUN:
                StopCoroutine(m_FootStep);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                break;

            case playerState.ROLL:
                StopCoroutine(m_FootStep);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                m_canMove = true;
                m_playerSoundnAni.setSprites(false, true, true, true, true);
                break;

            case playerState.HIDDEN:
                m_playerSoundnAni.setSprites(false, true, true, true, true);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                m_canMove = true;
                m_playerSoundnAni.setSprites(false, true, true, true, true);
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
    public void GoToStairLayer(bool _input, Vector2 _movePos, Vector2 _normal)
    {
        if (_input)
        {
            gameObject.layer = 10;
            //m_isStairLerping = true;
            //m_StairTelePos = _movePos;
            m_PlayerStairMgr.ChangePlayerNormal(_normal);
        }
        else if (_input == false)
        {
            gameObject.layer = 12;
            m_PlayerStairMgr.ChangePlayerNormal(Vector2.up);
        }
    }
    public void Attacked(IHotBoxParam _param)
    {
        if (m_canAttacked && m_curPlayerState != playerState.DEAD)
        {
            m_canAttacked = false;
            Invoke(nameof(SetPlayerCanAttacked), m_stunTime);

            Debug.Log(_param.m_Damage + "데미지 총알이 플레이어한테 박힘");

            m_Hp -= _param.m_Damage;

            if (m_Hp <= 0)
            {
                changePlayerFSM(playerState.DEAD);
                m_UIMgr.m_GameOverUI.SetActive(true);
            }

            m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
            m_PlayerUIMgr.UpdatePlayerHp(Mathf.RoundToInt(m_Hp / 10f));
        }
    }
    private IEnumerator RecoverRollCount()
    {
        m_isRecoveringRollCount = true;
        yield return new WaitForSeconds(m_RollRecoverTime);
        m_LeftRollCount += 1;
        if (m_LeftRollCount < m_MaxRollCount)
            StartCoroutine(RecoverRollCount());
        else if (m_LeftRollCount == m_MaxRollCount)
            m_isRecoveringRollCount = false;
    }
    private IEnumerator MakePlayerNoise(NoiseType _noiseType, Vector2 _size)
    {
        while (true)
        {
            m_NoiseMaker.MakeNoise(_noiseType, _size, transform.position, true);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void SetPlayerCanAttacked() { m_canAttacked = true; } 


    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Stair"))
        {
            //m_PlayerStairMgr.m_PlayerNormal = collision.contacts[0].normal;

            Debug.Log( collision.gameObject.name+"직접 측정 "+m_PlayerStairMgr.m_PlayerNormal);
        }
        else
        {
            Debug.Log(collision.gameObject.name + "직접 측정 " + m_PlayerStairMgr.m_PlayerNormal);
            m_PlayerStairMgr.m_PlayerNormal = Vector2.up;
        }
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Stair"))
        {
            GoToStairLayer(false, Vector2.zero, Vector2.zero);
        }
        */
    }
}