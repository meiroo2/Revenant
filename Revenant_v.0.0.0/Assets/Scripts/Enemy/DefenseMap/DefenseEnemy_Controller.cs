using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenseEnemy_Controller : Enemy
{
    EnemyState nextEnemyState;

    [SerializeField]
    Parts parts;

    DefenseEnemy_Status status;


    // 위치 도달을 위한 오차 범위
    [field: SerializeField]
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    bool isAlive = true;
    Rigidbody2D rigid;
    Animator animator;

    CircleCollider2D guardHearCollider; // 청음 범위

    [field: SerializeField]
    public Enemy_Gun m_gun { get; set; }

    private SoundMgr_SFX m_SFXMgr;


    // 이동
   [field: SerializeField]
    public Vector2 m_sensorPos { get; set; } // 감지된 위치
    Vector2 originalPos { get; set; } // 원 위치


    // 전투
    bool isReady = false; // true 시 사격 불가

    // 경직
    float curStun = 0;
    bool isStun = false;

    

    // UI
    public GameObject stunMark;
    public GameObject detectMark;

    // 무방비 선딜
    //[field: SerializeField]
    //public float m_preIdleTime { get; set; } = 1.0f;

    // 추격 선딜
    //[field: SerializeField]
    //public float m_preChaseTime { get; set; } = 1.0f;



    private void Awake()
    {
        setisRightHeaded(false);
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        status = GetComponent<DefenseEnemy_Status>();

        curStun = status.m_stunStack;
        originalPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (isAlive)
            AI();
    }

    public override void Damaged(float stun, float damage)
    {
        
        GuardAIState();

        // 사망
        if (status.m_Hp - damage <= 0)
        {
            //Debug.Log(name + " Die");
            isAlive = false;

            EnemyMgr_DefenseMap.Instance.PlusDieCount();

            gameObject.SetActive(false);
            isAlive = false;
        }
        // 경직
        else if (curStun - stun <= 0)
        {
            status.m_Hp -= damage;
            //Debug.Log(name + " stunned ");

            StunAIState();
            curStun = status.m_stunStack; // 스택 초기화
        }
        // 피격
        else
        {
            //Debug.Log(name + " damaged: " + damage);
            status.m_Hp -= damage;
            //Debug.Log(name + " stun damage: " + stun);
            curStun -= stun;
        }
    }

    // 이동 전 몸체 회전
    // 좌우 회전만 있음

    public void Rotation()
    {
        // 음수: 좌, 양수: 우
        if (m_sensorPos.x - transform.position.x < SAFE_DISTANCE)
        {
            if (m_isRightHeaded)
                setisRightHeaded(false);

        }
        else if (m_sensorPos.x - transform.position.x > SAFE_DISTANCE)
        {
            if (!m_isRightHeaded)
                setisRightHeaded(true);
        }
        else { }//no rotation
    }

    // 이동

    public void Move()
    {
        //Rotation();
        // 도착할때까지 이동
        if (!Destination())
        {
            // 애니메이션
            animator.SetBool("isWalk", true);
            if (m_isRightHeaded)
                rigid.velocity = new Vector2(1, 0) * status.m_Speed;
            else
                rigid.velocity = new Vector2(-1, 0) * status.m_Speed;
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
        if (m_isRightHeaded)
        {
            if (m_sensorPos.x - transform.position.x < SAFE_DISTANCE)
                return true;
        }
        else
        {
            if ((m_sensorPos.x - transform.position.x) * -1 < SAFE_DISTANCE)
                return true;
        }
        return false;
    }

    // 센서 표시
    // * 자극 받은 위치를 저장
    public void Sensor()
    {
        if(m_canSensor)
        {
            if(m_playerTransform)
                m_sensorPos = m_playerTransform.position;
            // 추격 자극
            RaycastHit2D guardRayHit2D;

            // 전투 자극
            RaycastHit2D fightRayHit2D;
            if (m_isRightHeaded)
            {
                // 추격 센서
                Debug.DrawRay(transform.position, Vector3.right * status.m_guardSightDistance, Color.magenta);
                guardRayHit2D = Physics2D.Raycast(transform.position, Vector3.right, status.m_guardSightDistance, LayerMask.GetMask("Player"));

                // 전투 센서
                Debug.DrawRay(transform.position, Vector3.right * status.m_fightSightDistance, Color.yellow);
                fightRayHit2D = Physics2D.Raycast(transform.position, Vector3.right, status.m_fightSightDistance, LayerMask.GetMask("Player"));

            }
            else
            {

                Debug.DrawRay(transform.position, Vector3.left * status.m_guardSightDistance, Color.magenta);
                guardRayHit2D = Physics2D.Raycast(transform.position, Vector3.left, status.m_guardSightDistance, LayerMask.GetMask("Player"));


                Debug.DrawRay(transform.position, Vector3.left * status.m_fightSightDistance, Color.yellow);
                fightRayHit2D = Physics2D.Raycast(transform.position, Vector3.left, status.m_fightSightDistance, LayerMask.GetMask("Player"));

            }
            if (!isStun)
            {
                // 사정 거리에 플레이어
                // 무방비 / 추격-> 전투
                if (fightRayHit2D)
                {
                    //m_sensorPos = fightRayHit2D.collider.transform.position;

                    // 랜덤 돌리기(0 ~ 2초)
                    // 랜덤 초수 후에 FightAIState 하셈

                    if (curEnemyState != EnemyState.FIGHT)
                    {
                        Invoke(nameof(FightAIState), Random.Range(0f, 2.0f));
                    }
                        
                }
                // 전투 -> 추격
                else
                {
                    if (curEnemyState == EnemyState.FIGHT)
                    {
                        //Rotation();
                        //PreGuardComplete();
                        GuardAIState();
                    }
                }
                //else if(guardRayHit2D)
                //{
                    //m_sensorPos = guardRayHit2D.collider.transform.position;
                    //if (curEnemyState == EnemyState.IDLE)
                    //{
                        //GuardAIState();
                    //}
                    
                //}
                
            }
        }
        

    }


    void StunAIState()
    {
        //Debug.Log("stunState");
        //rigid.velocity = new Vector2(0, 0);// 이동 멈춤
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;// 전투 시 그 자리에 바로 멈춤

        // 애니메이션
        animator.SetTrigger("Stun");
        animator.SetBool("isStun", true);

        isStun = true;
        // ★
        if (detectMark)
            detectMark.SetActive(false);
        if (stunMark)
            stunMark.SetActive(true);

        // 다음 상태: 스턴 먹기 전 상태
        if (curEnemyState != EnemyState.STUN) // 스턴일 때를 저장하면 무한 스턴에 걸릴 것
            nextEnemyState = curEnemyState;
        curEnemyState = EnemyState.STUN;

        Invoke(nameof(StunComplete), status.m_stunTime);
    }

    void StunComplete()
    {
        //Debug.Log("stunComplete");
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// 전투 시 그 자리에 바로 멈춤
        // 애니메이션
        animator.SetBool("isStun", false);

        isStun = false;
        if (stunMark)
            stunMark.SetActive(false);
    }

    public void GuardAIState()
    {
        if (!isStun)
        {
            Rotation();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

            animator.SetBool("isFight", false);
            animator.SetBool("isWalk", true);
            // 애니메이션
            animator.SetBool("isReady", false);

            // 파츠 끄기
            parts.setSpriteParts(false);

            // 선딜 (전투 상태 돌입 딜레이)
            Invoke(nameof(PreGuardComplete), status.m_preChaseTime);
        }

    }

    void PreGuardComplete()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        // 애니메이션
        animator.SetBool("isReady", true);
        curEnemyState = EnemyState.GUARD;

    }

    void FightAIState()
    {
        if (!isStun)
        {
            Rotation();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;// 전투 시 그 자리에 바로 멈춤

            // 선딜 (전투 상태 돌입 딜레이)
            isReady = true;
            Invoke(nameof(ReadyComplete), status.m_preFightTime);

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
        switch (curEnemyState)
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
                    //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// 멈춤 해제
                    m_gun.Fire();
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
        GetComponent<SuperArmorMgr>().doSuperArmor();
        //Debug.Log("ReadyComplete");
        isReady = false;
    }

}
