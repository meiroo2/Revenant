using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    [field : SerializeField] public float p_RollCountRecoverSpeed { get; private set; } = 1f;
    [field : SerializeField] public float p_RollSpeedRatio { get; private set; } = 1.3f;
    [field : SerializeField] public float p_BackWalkSpeedRatio { get; private set; } = 0.7f;
    [field : SerializeField] public float p_RunSpeedRatio { get; private set; } = 1.5f;
    [field : SerializeField] public float p_RollRecoverTime { get; private set; } = 2f;
    [field : SerializeField] public int p_MaxRollCount { get; private set; } = 3;

    [Space(30f)] 
    [Header("Don't 땃쥐")]
    [field : SerializeField] public Transform p_Player_RealPos;
    
    

    // Member Variables
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
    public LocationSensor m_PlayerLocationSensor { get; private set; }
    public Player_InputMgr m_InputMgr { get; private set; }

    
    private bool m_isRecoveringRollCount = false;
    public float m_LeftRollCount { get; set; }
    public bool m_canRoll { get; private set; } = true;
    public bool m_canChangeWeapon { get; private set; } = false;
    public bool m_isPlayerBlinking { get; private set; } = false;


    private Player_IDLE m_IDLE;
    private Player_WALK m_WALK;
    private Player_ROLL m_ROLL;
    private Player_HIDDEN m_HIDDEN;
    private Player_DEAD m_DEAD;

    private List<HideSlot> m_HideSlotList = new List<HideSlot>();
    private HideSlot m_CurHideSlot;
    
    private RotationMatScriptMgr m_RotationMatMgr;
    public SoundMgr_SFX m_SFXMgr { get; private set; }
    private PlayerFSM m_CurPlayerFSM;
    private NoiseMaker m_NoiseMaker;
    private IEnumerator m_FootStep;

    private Vector2 m_PlayerPosVec;
    public RaycastHit2D m_FootRay { get; private set; }

    private Coroutine m_WalkSoundCoroutine;



    // For Player_Managers

    
    
    // Constructor
    private void Awake()
    {
        m_RotationMatMgr = GetComponent<RotationMatScriptMgr>();
        m_PlayerAnimator = GetComponent<Animator>();
        m_PlayerRigid = GetComponent<Rigidbody2D>();
        m_PlayerLocationSensor = GetComponentInChildren<LocationSensor>();
        m_PlayerAniMgr = GetComponentInChildren<Player_AniMgr>();
        m_PlayerHotBox = GetComponentInChildren<Player_HotBox>();
        m_PlayerStairMgr = GetComponentInChildren<Player_StairMgr>();
        m_playerRotation = GetComponentInChildren<PlayerRotation>();
        m_WeaponMgr = GetComponentInChildren<Player_WeaponMgr>();
        m_useRange = GetComponentInChildren<Player_UseRange>();


        m_IDLE = new Player_IDLE(this);
        m_WALK = new Player_WALK(this);
        m_ROLL = new Player_ROLL(this);
        m_HIDDEN = new Player_HIDDEN(this);
        m_DEAD = new Player_DEAD(this);
        
        
        m_ObjectType = ObjectType.Human;
        m_ObjectState = ObjectState.Active;
        m_LeftRollCount = p_MaxRollCount;
        m_CanAttacked = true;
    }
    private void Start()
    {
        var tempInstance = InstanceMgr.GetInstance();
        m_PlayerUIMgr = tempInstance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_InputMgr = tempInstance.GetComponentInChildren<Player_InputMgr>();
        m_NoiseMaker = tempInstance.GetComponentInChildren<NoiseMaker>();
        m_SFXMgr = tempInstance.GetComponentInChildren<SoundMgr_SFX>();
        
        m_CurPlayerFSM = m_IDLE;
        m_CurPlayerFSM.StartState();
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
        p_RollCountRecoverSpeed = _input.RollCountRecoverSpeed;

        m_WeaponMgr.m_CurWeapon.p_MaxBullet = _input.BulletCount;
        m_WeaponMgr.m_CurWeapon.p_MaxMag = _input.MagCount;
        
        m_WeaponMgr.m_CurWeapon.m_LeftRounds = _input.BulletCount;
        m_WeaponMgr.m_CurWeapon.m_LeftMags = _input.MagCount;
        
        // 강제 Player_UIMgr 할당
        m_PlayerUIMgr = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_PlayerUIMgr.SetMaxHp(_input.Hp);
        m_PlayerUIMgr.SetLeftRoundsNMag(_input.BulletCount, _input.MagCount);
        m_PlayerUIMgr.m_ReloadSpeed = _input.ReloadSpeed;
        m_PlayerUIMgr.m_HitmarkRemainTime = _input.HitmarkRemainTime;
        
        // 스폰과 동시에 InitPlayerValue가 호출되기 때문에 Prefab 데이터를 저장하진 않음.
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(m_PlayerUIMgr);
        #endif
    }



    // Update
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause)       // Pause면 중지
            return;

        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        PlayerRayFunc();

        if (gameObject.layer == 12 && m_FootRay)
        {
            m_PlayerStairMgr.ChangePlayerNormal(m_FootRay.normal);
        }

        // 픽셀퍼펙트 공식 서비스 종료 (Noice)(22.06.13)
        // transform.position = StaticMethods.getPixelPerfectPos(m_PlayerPosVec);
    }

    private void FixedUpdate()
    {
        m_CurPlayerFSM.UpdateState();
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
                m_CurPlayerFSM = m_IDLE;
                break;
            
            case PlayerStateName.WALK:
                m_CurPlayerFSM = m_WALK;
                break;
            
            case PlayerStateName.ROLL:
                m_CurPlayerFSM = m_ROLL;
                break;
            
            case PlayerStateName.HIDDEN:
                m_CurPlayerFSM = m_HIDDEN;
                break;
            
            case PlayerStateName.DEAD:
                m_CurPlayerFSM = m_DEAD;
                break;
            
            default:
                Debug.Log("Player->ChangePlayerFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurPlayerFSM.StartState();
        m_PlayerAniMgr.playplayerAnim();
    }
    

    
    // Functions
    public void SetWalkSoundCoroutine(bool _isOn)
    {
        if (_isOn)
        {
            m_WalkSoundCoroutine = StartCoroutine(PlayWalkSound());
        }
        else
        {
            StopCoroutine(m_WalkSoundCoroutine);
        }
        
    }

    private IEnumerator PlayWalkSound()
    {
        while (true)
        {
            m_SFXMgr.playPlayerSFXSound(4);
            yield return new WaitForSeconds(0.5f);
        }
    }
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

    public void UseRollCount()
    {
        m_LeftRollCount -= 1f;
        m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);

        if (!m_isRecoveringRollCount)
            StartCoroutine(RecoverRollCount());
    }
    public IEnumerator RecoverRollCount()
    {
        m_isRecoveringRollCount = true;
        while (m_LeftRollCount < p_MaxRollCount)
        {
            m_LeftRollCount += Time.deltaTime * p_RollCountRecoverSpeed;
            m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);
            yield return null;
        }

        m_LeftRollCount = 3f;
        m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);
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
        
        m_RotationMatMgr.FlipAllNormalsToRight(m_IsRightHeaded);
        
        m_PlayerAniMgr.playplayerAnim();
    }
    public void setPlayerHp(int _value)
    {
        p_Hp = _value;
        m_PlayerUIMgr.SetHp(p_Hp);
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
    public bool GetIsPlayerWalkStraight()
    {
        return (m_IsRightHeaded && m_InputMgr.m_IsPushRightKey) || (!m_IsRightHeaded && m_InputMgr.m_IsPushLeftKey);
    }
    private void PlayerRayFunc()
    {
        m_PlayerPosVec = transform.position;

        if (gameObject.layer == 12)
            m_FootRay = Physics2D.Raycast(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), -transform.up, 0.5f, LayerMask.GetMask("Floor"));
        else if (gameObject.layer == 10)
            m_FootRay = Physics2D.Raycast(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), -transform.up, 0.5f, LayerMask.GetMask("Stair"));

        Debug.DrawRay(new Vector2(m_PlayerPosVec.x, m_PlayerPosVec.y - 0.36f), Vector2.down * 0.5f, new Color(0, 1, 0));

        //Debug.Log((m_PlayerPosVec.y - m_FootRay.point.y));

        /*
        if (m_FootRay && (m_PlayerPosVec.y - 0.36f) - m_FootRay.point.y >= 0.29f)
            m_PlayerPosVec.y = m_FootRay.point.y + 0.26f + 0.36f;
            */
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "HideSlot":
                m_HideSlotList.Add(col.GetComponent<HideSlot>());
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // HideSlotList 크기가 1 이상일 경우에만 작동
        if (m_HideSlotList.Count <= 0)
            return;


        int nearSlotIdx = -1;
        float nearSlotDistance = 999f;
        
        
        // 1. 가장 가까운 슬롯 Idx 찾기
        foreach (HideSlot ele in m_HideSlotList)
        {
            float distanceBetPlayer = (transform.position - ele.transform.position).sqrMagnitude;
            if (distanceBetPlayer < nearSlotDistance)
            {
                nearSlotDistance = distanceBetPlayer;
                nearSlotIdx = m_HideSlotList.IndexOf(ele);
            }
        }

        // 2. 가장 가까운 것만 하이라이트 처리
        foreach (HideSlot ele in m_HideSlotList)
        {
            ele.ActivateOutline(false);
        }
        if (nearSlotIdx == -1)
            return;
        else 
            m_HideSlotList[nearSlotIdx].ActivateOutline(true);
        
        // 3. 키다운 or 키업 처리
        if (m_InputMgr.m_IsPushHideKey && m_CurPlayerFSMName != PlayerStateName.HIDDEN)
        {
          m_CurHideSlot = m_HideSlotList[nearSlotIdx];
          ChangePlayerFSM(PlayerStateName.HIDDEN);
        }
        else if(!m_InputMgr.m_IsPushHideKey && m_CurPlayerFSMName == PlayerStateName.HIDDEN)
        {
            ChangePlayerFSM(PlayerStateName.IDLE);
            m_CurHideSlot = null;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // HideSlot Outline 제거 후 Remove
        switch (other.tag)
        {
            case "HideSlot":
                HideSlot willRemoveSlot = other.GetComponent<HideSlot>();
                willRemoveSlot.ActivateOutline(false);
                m_HideSlotList.Remove(willRemoveSlot);
                break;
        }

        if (m_HideSlotList.Count <= 0)
            m_CurHideSlot = null;
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