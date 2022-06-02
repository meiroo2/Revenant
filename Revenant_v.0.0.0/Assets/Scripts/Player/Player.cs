using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateName
{
    IDLE,
    WALK,
    ROLL,
    HIDDEN,
    DEAD
}

public class Player : Human
{
    // Visable Member Variables
    [field : SerializeField] public float p_RollSpeedRatio { get; private set; } = 1.3f;
    [field : SerializeField] public float p_BackWalkSpeedRatio { get; private set; } = 0.7f;
    [field : SerializeField] public float p_RunSpeedRatio { get; private set; } = 1.5f;
    [field : SerializeField] public float p_RollRecoverTime { get; private set; } = 2f;
    [field : SerializeField] public int p_MaxRollCount { get; private set; } = 3;

    [Space(30f)] 
    [Header("Don't 땃쥐")]
    [field : SerializeField] public Transform p_Player_RealPos;
    
    

    // Member Variables
    public Player_AniMgr m_Player_AniMgr { get; private set; }
    public PlayerRotation m_playerRotation { get; private set; }
    public Player_WeaponMgr m_WeaponMgr { get; private set; }
    public Player_UseRange m_useRange { get; private set; }
    public Player_AniMgr m_PlayerAniMgr { get; private set; }
    public PlayerStateName m_CurPlayerFSMName { get; private set; }
    public Rigidbody2D m_PlayerRigid { get; private set; }
    public Animator m_PlayerAnimator { get; private set; }
    public Player_StairMgr m_PlayerStairMgr { get; private set; }
    public Player_HotBox m_PlayerHotBox { get; private set; }
    public Player_UI m_PlayerUIMgr { get; private set; }
    public LocationInfo m_PlayerLocationInfo { get; private set; }
    public LocationSensor m_PlayerLocationSensor { get; private set; }
    public EnemyMgr m_EnemyMgr { get; private set; }
    

    private SoundMgr_SFX m_SFXMgr;
    private Player_InputMgr m_InputMgr;
    private PlayerFSM m_CurPlayerFSM;
    private NoiseMaker m_NoiseMaker;
    
    
    public int m_LeftRollCount { get; set; }
    public bool m_canRoll { get; private set; } = true;
    public bool m_canChangeWeapon { get; private set; } = false;
    public bool m_isPlayerBlinking { get; private set; } = false;
    public bool m_isRecoveringRollCount { get; private set; } = false;
    private IEnumerator m_FootStep;
    private Vector2 m_PlayerPosVec;
    private RaycastHit2D m_FootRay;
    


    // For Player_Managers

    
    
    // Constructor
    private void Awake()
    {
        m_PlayerLocationSensor = GetComponentInChildren<LocationSensor>();
        m_PlayerLocationInfo = GetComponentInChildren<LocationInfo>();
        m_PlayerAniMgr = GetComponentInChildren<Player_AniMgr>();
        m_PlayerHotBox = GetComponentInChildren<Player_HotBox>();
        m_PlayerStairMgr = GetComponentInChildren<Player_StairMgr>();
        m_Player_AniMgr = GetComponentInChildren<Player_AniMgr>();
        m_playerRotation = GetComponentInChildren<PlayerRotation>();
        m_WeaponMgr = GetComponentInChildren<Player_WeaponMgr>();
        m_useRange = GetComponentInChildren<Player_UseRange>();
        m_PlayerAnimator = GetComponent<Animator>();
        m_PlayerRigid = GetComponent<Rigidbody2D>();

        m_ObjectType = ObjectType.Human;
        m_ObjectState = ObjectState.Active;
        m_LeftRollCount = p_MaxRollCount;
        m_CanAttacked = true;
    }
    public void InitPlayerValue(Player_ValueManipulator _input)
    {
        p_Hp = _input.Hp;
        p_stunTime = _input.StunInvincibleTime;
        p_Speed = _input.Speed;
        p_BackWalkSpeedRatio = _input.BackSpeedRatio;
        p_RunSpeedRatio = _input.RunSpeedRatio;
        p_RollSpeedRatio = _input.RollSpeedRatio;
        p_MaxRollCount = _input.RollCountMax;
        m_LeftRollCount = p_MaxRollCount;
        p_RollRecoverTime = _input.RollRecoverTime;

        m_PlayerUIMgr.setLeftBulletUI(_input.BulletCount, _input.MagCount, 0);
    }
    private void Start()
    {
        var tempInstance = InstanceMgr.GetInstance();
        m_CurPlayerFSM = new PlayerIDLE(this, m_InputMgr);
        
        m_PlayerUIMgr = tempInstance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_InputMgr = tempInstance.GetComponentInChildren<Player_InputMgr>();
        m_NoiseMaker = tempInstance.GetComponentInChildren<NoiseMaker>();
        m_SFXMgr = tempInstance.GetComponentInChildren<SoundMgr_SFX>();
        m_EnemyMgr = tempInstance.GetComponentInChildren<EnemyMgr>();
    }
    
    
    
    // Update
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause)       // Pause면 중지
            return;

        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        m_CurPlayerFSM.UpdateState();
        PlayerRayFunc();

        if (gameObject.layer == 12 && m_FootRay)
        {
            m_PlayerStairMgr.ChangePlayerNormal(m_FootRay.normal);
        }

        transform.position = StaticMethods.getPixelPerfectPos(m_PlayerPosVec);
    }

    
    
    // Player FSM Functions
    // ReSharper disable Unity.PerformanceAnalysis
    public void ChangePlayerFSM(PlayerStateName _name)
    {
        //Debug.Log("상태 전이" + _name);
        m_PlayerAniMgr.exitplayerAnim();
        m_CurPlayerFSMName = _name;
        
        m_CurPlayerFSM.ExitState();
        switch (m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                m_CurPlayerFSM = new PlayerIDLE(this, m_InputMgr);
                break;
            
            case PlayerStateName.WALK:
                m_CurPlayerFSM = new PlayerWALK(this, m_InputMgr);
                break;
            
            case PlayerStateName.ROLL:
                m_CurPlayerFSM = new PlayerROLL(this, m_InputMgr);
                break;
            
            case PlayerStateName.HIDDEN:
                m_CurPlayerFSM = new PlayerHIDDEN(this, m_InputMgr);
                break;
            
            case PlayerStateName.DEAD:
                m_CurPlayerFSM = new PlayerDEAD(this, m_InputMgr);
                break;
            
            default:
                Debug.Log("Player->ChangePlayerFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurPlayerFSM.StartState();
        m_PlayerAniMgr.playplayerAnim();
    }
    

    
    // Functions
    public bool IsPlayerOnStair()
    {
        /*
        if (gameObject.layer == 10)
        {
            m_PlayerLocationSensor
            return true;
        }

        return false;
        */
        return false;
    }
    public void GoToStairLayer(bool _input, Vector2 _movePos, Vector2 _normal)
    {
        if (_input)
        {
            gameObject.layer = 10;
            m_PlayerStairMgr.ChangePlayerNormal(_normal);
        }
        else
        {
            gameObject.layer = 12;
            m_PlayerStairMgr.ChangePlayerNormal(Vector2.up);
        }
    }
    public IEnumerator RecoverRollCount()
    {
        m_PlayerUIMgr.UpdateRollTimer(p_RollRecoverTime);

        m_isRecoveringRollCount = true;
        yield return new WaitForSeconds(p_RollRecoverTime);

        m_LeftRollCount += 1;
        m_PlayerUIMgr.UpdateRollCount(m_LeftRollCount);

        if (m_LeftRollCount < p_MaxRollCount)
            StartCoroutine(RecoverRollCount());
        else if (m_LeftRollCount == p_MaxRollCount)
            m_isRecoveringRollCount = false;
    }
    private IEnumerator MakePlayerNoise(NoiseType _noiseType, Vector2 _size, LocationInfo _location)
    {
        while (true)
        {
            m_NoiseMaker.MakeNoise(_noiseType, _size, _location, true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void setisRightHeaded(bool _isRightHeaded)
    {
        m_IsRightHeaded = _isRightHeaded;
        var TempLocalScale = transform.localScale;

        if ((m_IsRightHeaded && TempLocalScale.x < 0) || (!m_IsRightHeaded && TempLocalScale.x > 0))
            transform.localScale = new Vector3(-TempLocalScale.x, TempLocalScale.y, 1);
        
        m_Player_AniMgr.playplayerAnim();
    }
    public void setPlayerHp(int _value)
    {
        p_Hp = _value;
    }
    public void DoPlayerBlink() 
    {
        if(p_stunTime > 0f)
        {
            m_isPlayerBlinking = true;
            CancelInvoke(nameof(setPlayerBlinkFalse));
            Invoke(nameof(setPlayerBlinkFalse), p_stunTime);
        }
    }
    private void setPlayerBlinkFalse() { m_isPlayerBlinking = false; }
    public bool getIsPlayerWalkStraight()
    {
        if ((m_IsRightHeaded && m_HumanFootNormal.x > 0) || (!m_IsRightHeaded && m_HumanFootNormal.x < 0))
            return true;
        else
            return false;
    }
    private void PlayerRayFunc()
    {
        m_PlayerPosVec = transform.position;

        if (gameObject.layer == 12)
            m_FootRay = Physics2D.Raycast(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), -transform.up, 0.5f, LayerMask.GetMask("Floor"));
        else if (gameObject.layer == 10)
            m_FootRay = Physics2D.Raycast(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), -transform.up, 0.5f, LayerMask.GetMask("Stair"));

        Debug.DrawRay(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), Vector2.down * 0.5f, new Color(0, 1, 0));



        if (m_FootRay && (m_PlayerPosVec.y - 0.36f) - m_FootRay.point.y >= 0.29f)
            m_PlayerPosVec.y = m_FootRay.point.y + 0.26f + 0.36f;
    }
    
    
    
    // Legacy
    /*
    private void updatePlayerFSM()
    {
        switch (m_curPlayerState)
        {
            case playerState.IDLE:
                m_HumanMoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), m_HumanMoveVec.y);
                if (m_HumanMoveVec.x != 0)
                    changePlayerFSM(playerState.WALK);
                if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                    changePlayerFSM(playerState.ROLL);
                break;

            case playerState.WALK:
                m_HumanMoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), m_HumanMoveVec.y);
                if (m_HumanMoveVec.x == 0)
                    changePlayerFSM(playerState.IDLE);
                else
                {
                    if ((m_isRightHeaded ? 1 : -1) == (int)m_HumanMoveVec.x)
                    {
                        if (m_isRightHeaded)
                            m_PlayerRigid.velocity = -new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_Speed;
                        else
                            m_PlayerRigid.velocity = new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_Speed;

                        if (Input.GetKeyDown(KeyCode.LeftShift))
                            changePlayerFSM(playerState.RUN);
                        else if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                            changePlayerFSM(playerState.ROLL);
                    }
                    else
                    {
                        if (m_isRightHeaded)
                        {
                            m_PlayerRigid.velocity = StaticMethods.getLPerpVec(m_PlayerStairMgr.m_PlayerNormal) * m_Speed * m_BackWalkSpeedRatio;
                            if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                            {
                                setisRightHeaded(false);
                                changePlayerFSM(playerState.ROLL);
                            }
                        }
                        else
                        {
                            m_PlayerRigid.velocity = -StaticMethods.getLPerpVec(m_PlayerStairMgr.m_PlayerNormal) * m_Speed * m_BackWalkSpeedRatio;
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
                    m_PlayerRigid.velocity = new Vector2(m_Speed * m_RunSpeedRatio, 0f);
                else
                    m_PlayerRigid.velocity = new Vector2(-m_Speed * m_RunSpeedRatio, 0f);

                if (Input.GetKeyUp(KeyCode.LeftShift))
                    changePlayerFSM(playerState.WALK);
                else if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                    changePlayerFSM(playerState.ROLL);
                break;

            case playerState.ROLL:
                if (m_isRightHeaded)
                {
                    m_PlayerRigid.velocity = -new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_RollSpeedRatio;
                }
                else
                {
                    m_PlayerRigid.velocity = new Vector2(-m_PlayerStairMgr.m_PlayerNormal.y, m_PlayerStairMgr.m_PlayerNormal.x) * m_RollSpeedRatio;
                }
                if (m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    changePlayerFSM(playerState.WALK);
                break;

            case playerState.HIDDEN:
                if (Input.GetKeyDown(KeyCode.Space) && m_LeftRollCount > 0)
                {
                    if (Input.GetKey(KeyCode.A))
                        setisRightHeaded(false);
                    else if (Input.GetKey(KeyCode.D))
                        setisRightHeaded(true);
                    else if (m_playerRotation.getIsMouseRight())
                        setisRightHeaded(true);
                    else if (!m_playerRotation.getIsMouseRight())
                        setisRightHeaded(false);

                    m_useRange.ForceExitFromHiddenSlot();
                    changePlayerFSM(playerState.ROLL);
                }
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }
    public void _changePlayerFSM(playerState _inputPlayerState)
    {
        exitPlayerFSM();

        switch (_inputPlayerState)
        {
            case playerState.IDLE:
                m_curPlayerState = playerState.IDLE;
                m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_CanAttacked = true;
                break;

            case playerState.WALK:
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(0.8f, 0.1f), m_curLocation);
                StartCoroutine(m_FootStep);
                m_curPlayerState = playerState.WALK;
                break;

            case playerState.RUN:
                
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(1f, 0.3f));
                StartCoroutine(m_FootStep);
                m_curPlayerState = playerState.RUN;
                m_playerRotation.m_doRotate = false;
                m_canShot = false;
                
                break;

            case playerState.ROLL:
                m_FootStep = MakePlayerNoise(NoiseType.WALK, new Vector2(1f, 0.3f), m_curLocation);
                StartCoroutine(m_FootStep);

                m_PlayerHotBox.setPlayerHotBoxCol(false);

                m_LeftRollCount -= 1;
                m_PlayerUIMgr.UpdateRollCount(m_LeftRollCount);

                if (!m_isRecoveringRollCount)
                    StartCoroutine(RecoverRollCount());

                m_curPlayerState = playerState.ROLL;
                m_playerRotation.m_doRotate = false;
                m_canShot = false;
                m_canMove = false;

                m_Player_AniMgr.setSprites(true, false, false, false, false);
                break;

            case playerState.HIDDEN:
                m_curPlayerState = playerState.HIDDEN;
                m_Player_AniMgr.setSprites(true, false, false, false, false);
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

        m_Player_AniMgr.playplayerAnim();
    }
    private void exitPlayerFSM()
    {
        m_Player_AniMgr.exitplayerAnim();
        switch (m_curPlayerState)
        {
            case playerState.IDLE:
                m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;

            case playerState.WALK:
                StopCoroutine(m_FootStep);
                m_PlayerRigid.velocity = Vector2.zero;
                break;

            case playerState.RUN:
                StopCoroutine(m_FootStep);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                break;

            case playerState.ROLL:
                m_PlayerHotBox.setPlayerHotBoxCol(true);
                StopCoroutine(m_FootStep);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                m_canMove = true;
                m_PlayerRigid.velocity = Vector2.zero;
                m_Player_AniMgr.setSprites(false, true, true, true, true);
                break;

            case playerState.HIDDEN:
                m_Player_AniMgr.setSprites(false, true, true, true, true);
                m_playerRotation.m_doRotate = true;
                m_canShot = true;
                m_canMove = true;
                m_Player_AniMgr.setSprites(false, true, true, true, true);
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }
    */
}