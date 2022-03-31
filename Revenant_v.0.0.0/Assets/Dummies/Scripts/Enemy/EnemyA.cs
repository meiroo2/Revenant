using System.Collections;
using UnityEngine;
using TMPro;


public enum DIR
{
    LEFT, RIGHT, UP, DOWN, STOP
}
public class EnemyA : Human, IAttacked
{
    Parts parts;

    [field: SerializeField]
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    public TextMeshProUGUI textForTest;

    bool isAlive = true;
    Rigidbody2D rigid;
    Animator animator;
    
    CircleCollider2D guardHearCollider;

    // 이동 방향
    public DIR defaultDir;
    DIR curDir;

    // 이동 위치
    public Vector2 sensorPos;

    // 경직정도
    [field: SerializeField]
    public float stunStack { get; set; }
    float curStun;
    bool isStun = false;
    public GameObject stunMark;

    // 무방비 선딜
    [field: SerializeField]
    public float m_preIdleTime { get; set; } = 1.0f;

    // 추격 선딜
    [field: SerializeField]
    public float m_preGuardTime { get; set; } = 1.0f;
    // 추격 시야 거리 - 시각
    [field: SerializeField]
    public float guardSightDistance { get; set; }    // 추격 거리

    // 전투 거리
    [field: SerializeField]
    public float fightSightDistance { get; set; } = 1.5f;   // 공격 거리
    public GameObject detectMark;

    // 사격 선딜
    [field: SerializeField]
    public float m_preFightTime { get; set; }
    bool isReady = false; // true 시 사격 불가

    [SerializeField]
    EnemyManager enemyManager;

    [field: SerializeField]
    public EnemyState curEnemyState { get; set; }
    EnemyState nextEnemyState;

    Gun gun;

    private SoundMgr_SFX m_SFXMgr;

    private void Awake()
    {
        setisRightHeaded(false);
        rigid = GetComponent<Rigidbody2D>();
        //enemyAnimator.GetComponent<EnemyAnimatior>();
        animator = GetComponent<Animator>();

        gun = GetComponentInChildren<Gun>();

        curStun = stunStack;

        m_SFXMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();

        parts = GetComponentInChildren<Parts>();
    }
    private void Update()
    {
        if(textForTest)
            textForTest.text = curEnemyState.ToString();
    }

    private void FixedUpdate()
    {
        AI();
    }

    public void Attacked(AttackedInfo _AttackedInfo)
    {
        float damage = _AttackedInfo.m_Damage;
        float stun = _AttackedInfo.m_StunValue;

        if (_AttackedInfo.m_HitPoint == HitPoints.HEAD)
        {
            Debug.Log("Head Hit");
            damage *= 2;
            stun *= 2;

            m_SFXMgr.playAttackedSound(MatType.Target_Head, _AttackedInfo.m_ContactPoint);
        }
        else if (_AttackedInfo.m_HitPoint == HitPoints.BODY)
        {
            Debug.Log("Body Hit");

            m_SFXMgr.playAttackedSound(MatType.Target_Body, _AttackedInfo.m_ContactPoint);
        }
        if(isAlive)
        {
            Damaged(stun, damage);
        }
    }
    
    public void Damaged(float stun, float damage)
    {

        // 피격 센서 자극 위치
        if (enemyManager)
            sensorPos = enemyManager.player.transform.position;
        else Debug.Log("there is no enemyManager");
        GuardAIState();

        // 사망
        if (m_Hp - damage <= 0)
        {
            Debug.Log(name + " Die");
            isAlive = false;

            if(enemyManager)
                enemyManager.PlusDieCount();

            Destroy(gameObject);
        }
        // 경직
        else if(curStun - stun <= 0)
        {
            m_Hp -= damage;
            Debug.Log(name + " stunned ");
            
            StunAIState();
            curStun = stunStack; // 스택 초기화
        }
        // 피격
        else
        {
            //Debug.Log(name + " damaged: " + damage);
            m_Hp -= damage;
            Debug.Log(name + " stun damage: " + stun);
            curStun -= stun;
        }
    }


    // 추격 - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public void AutoMove()
    {
        switch (curDir)
        {
            case DIR.LEFT:
                rigid.velocity = new Vector2(-1, 0);
                break;
            case DIR.RIGHT:
                rigid.velocity = new Vector2(1, 0);
                break;
            default:
                //Debug.Log("There is No Dir Move Code");
                break;
        }
    }
    // 이동 전 몸체 회전
    // 좌우 회전만 있음

    public void Rotation()
    {
        // 음수: 좌, 양수: 우
        if (sensorPos.x - transform.position.x < SAFE_DISTANCE)
        {
            if (m_isRightHeaded)
                setisRightHeaded(false);

        }
        else if (sensorPos.x - transform.position.x > SAFE_DISTANCE)
        {
            if (!m_isRightHeaded)
                setisRightHeaded(true);
        }
        else { }//no rotation
    }

    // 이동

    public void Move()
    {
        Rotation();
        // 도착할때까지 이동
        if (!Destination())
        {
            // 애니메이션
            animator.SetBool("isWalk", true);
            if (m_isRightHeaded)
                rigid.velocity = new Vector2(1, 0);
            else
                rigid.velocity = new Vector2(-1, 0);
        }
        // 도착
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    // 도착
    bool Destination()
    {
        if(m_isRightHeaded)
        {
            if (sensorPos.x - transform.position.x < SAFE_DISTANCE)
                return true;
        }
        else
        {
            if ((sensorPos.x - transform.position.x) * -1 < SAFE_DISTANCE)
                return true;
        }
        return false;
    }

    // 센서 표시
    // * 자극 받은 위치를 저장
    public void Sensor()
    {
        // 추격 자극
        RaycastHit2D guardRayHit2D;

        // 전투 자극
        RaycastHit2D fightRayHit2D;
        if (m_isRightHeaded)
        {
            // 추격 센서
            Debug.DrawRay(transform.position, Vector3.right * guardSightDistance, Color.magenta);
            guardRayHit2D = Physics2D.Raycast(transform.position, Vector3.right, guardSightDistance, LayerMask.GetMask("Player"));

            // 전투 센서
            Debug.DrawRay(transform.position, Vector3.right * fightSightDistance, Color.yellow);
            fightRayHit2D = Physics2D.Raycast(transform.position, Vector3.right, fightSightDistance, LayerMask.GetMask("Player"));
            
        }
        else
        {

            Debug.DrawRay(transform.position, Vector3.left * guardSightDistance, Color.magenta);
            guardRayHit2D = Physics2D.Raycast(transform.position, Vector3.left, guardSightDistance, LayerMask.GetMask("Player"));


            Debug.DrawRay(transform.position, Vector3.left * fightSightDistance, Color.yellow);
            fightRayHit2D = Physics2D.Raycast(transform.position, Vector3.left, fightSightDistance, LayerMask.GetMask("Player"));

        }
        if(!isStun)
        {
            // 무방비 / 추격-> 전투
            if (fightRayHit2D)
            {
                sensorPos = fightRayHit2D.collider.transform.position;
                
                FightAIState();
            }
            // 무방비 / 전투 -> 추격
            else if (guardRayHit2D)
            {
                sensorPos = guardRayHit2D.collider.transform.position;
                GuardAIState();
            }
        }
            
    }


    void StunAIState()
    {
        Debug.Log("stunState");
        //rigid.velocity = new Vector2(0, 0);// 이동 멈춤
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;// 전투 시 그 자리에 바로 멈춤

        // 애니메이션
        animator.SetTrigger("Stun");
        animator.SetBool("isStun", true);

        isStun = true;
        // ★
        if(detectMark)
            detectMark.SetActive(false);
        if(stunMark)
            stunMark.SetActive(true);

        // 다음 상태: 스턴 먹기 전 상태
        if (curEnemyState != EnemyState.STUN) // 스턴일 때를 저장하면 무한 스턴에 걸릴 것
            nextEnemyState = curEnemyState;
        curEnemyState = EnemyState.STUN;

        Invoke(nameof(StunComplete), m_stunTime);
    }

    void StunComplete()
    {
        Debug.Log("stunComplete");
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// 전투 시 그 자리에 바로 멈춤
        // 애니메이션
        animator.SetBool("isStun", false);

        isStun = false;
        if(stunMark)
            stunMark.SetActive(false);
    }

    public void GuardAIState()
    {
        if(!isStun)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// 전투 시 그 자리에 바로 멈춤

            animator.SetBool("isFight", false);
            animator.SetBool("isWalk", true);
            // 애니메이션
            animator.SetBool("isReady", false);

            // 파츠 끄기
            parts.setSpriteParts(false);

            // 선딜 (전투 상태 돌입 딜레이)
            Invoke(nameof(PreGuardComplete), m_preGuardTime);
        }
        
    }

    void PreGuardComplete()
    {
        // 애니메이션
        animator.SetBool("isReady", true);
        curEnemyState = EnemyState.GUARD;
        
    }

    void FightAIState()
    {
        if (!isStun)
        {

            rigid.constraints = RigidbodyConstraints2D.FreezeAll;// 전투 시 그 자리에 바로 멈춤

            // 선딜 (전투 상태 돌입 딜레이)
            isReady = true;
            Invoke(nameof(ReadyComplete), m_preFightTime);

            // 애니메이션
            animator.SetBool("isWalk", false);
            animator.SetBool("isFight", true);

            // 파츠 틀기
            parts.setSpriteParts(true);

            //Debug.Log("FightAnim");
            // !
            if (detectMark)
                detectMark.SetActive(true);

            // 공격 상태
            curEnemyState = EnemyState.FIGHT;
        }

    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// 대기
                // 플레이어 만나면 추격
                Sensor();
                break;

            case EnemyState.GUARD:// 추격
                Sensor();
                // 이동
                Move();
                break;

            case EnemyState.FIGHT:// 전투
                
                if (isReady == false)    // 준비 동작 끝나면
                {
                    Debug.Log("I'mReady");
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// 멈춤 해제
                    gun.Fire();
                    Sensor();
                }
                    
                break;
            case EnemyState.STUN:// 스턴

                if (isStun == false) // 스턴 완료
                {
                    if (nextEnemyState == EnemyState.FIGHT)
                    {
                        FightAIState();
                    }
                        
                    else
                        curEnemyState = nextEnemyState; // 다음 상태: 스턴 먹기 전 상태
                    
                }
                break;
            case EnemyState.DEAD: // 시체
                break;
        }
    }

    void ReadyComplete()
    {
        //Debug.Log("ReadyComplete");
        isReady = false;
    }

    // 겹침 현상
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            OverLap();
        }
    }

    void OverLap()
    {
        
        

        Debug.Log("Dir = Stop");
        curDir = DIR.STOP; // 현재 이동방향값 STOP
        
    }

    void unOverLap()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    
}
