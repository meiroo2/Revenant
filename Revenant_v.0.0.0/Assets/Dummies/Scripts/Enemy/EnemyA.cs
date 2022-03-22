using System.Collections;
using UnityEngine;


public enum DIR
{
    LEFT, RIGHT, UP, DOWN, STOP
}
public class EnemyA : Human, IAttacked
{

    bool isAlive = true;
    Rigidbody2D rigid;
    Animator animator;
    

    // 이동 방향
    public DIR defaultDir;
    DIR curDir;

    // 경직정도
    [field: SerializeField]
    public float stunStack { get; set; }
    float curStun;
    bool isStun = false;
    public GameObject stunMark;


    // 추격 거리
    [field: SerializeField]
    public float guardDistance { get; set; }    // 추격 거리

    // 전투 거리
    [field: SerializeField]
    public float attackDistance { get; set; }   // 공격 거리
    public GameObject detectMark;

    // 사격 준비
    [field: SerializeField]
    public float readyTime { get; set; }
    bool isReady = false; // true 시 사격 불가

    EnemyManager enemyManager;

    [SerializeField]
    EnemyState curEnemyState;
    EnemyState nextEnemyState;

    Gun gun;

    private SoundMgr_SFX m_SFXMgr;

    private void Awake()
    {
        setisRightHeaded(false);
        rigid = GetComponent<Rigidbody2D>();
        //enemyAnimator.GetComponent<EnemyAnimatior>();
        animator = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();

        gun = GetComponentInChildren<Gun>();

        curStun = stunStack;

        m_SFXMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();
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



    public void AutoMove()
    {
        switch(curDir)
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

    public void CheckPlayer(float distance)
    {
        RaycastHit2D rayHit2D;
        
        if (m_isRightHeaded)
        {
            Debug.DrawRay(transform.position, Vector3.right * distance, Color.magenta);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.right, distance, LayerMask.GetMask("Player"));
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * distance, Color.magenta);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.left, distance, LayerMask.GetMask("Player"));
            
        }
        if(rayHit2D)
        {
            switch (curEnemyState)
            {
                // 대기 -> 추격
                case EnemyState.IDLE:
                    GuardAIState();
                    break;

                // 추격 -> 공격
                case EnemyState.GUARD:
                    FightAIState();
                    break;
            }
        }
            


    }

    void StunAIState()
    {
        isStun = true;
        // ★
        detectMark.SetActive(false);
        stunMark.SetActive(true);
        curEnemyState = EnemyState.STUN;

        Invoke(nameof(StunComplete), m_stunTime);
    }

    void StunComplete()
    {
        isStun = false;
        stunMark.SetActive(false);
    }

    void GuardAIState()
    {
        // 추격 상태
        curEnemyState = EnemyState.GUARD;
    }

    void FightAIState()
    {
        // 애니메이션
        animator.SetBool("isFight", true);

        // !
        detectMark.SetActive(true);

        // 준비 동작
        isReady = true;
        Invoke(nameof(ReadyComplete), readyTime);
        
        // 공격 상태
        curEnemyState = EnemyState.FIGHT;

    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// 대기
                // 플레이어 만나면 추격
                CheckPlayer(guardDistance);
                break;

            case EnemyState.GUARD:// 추격
                // 왼쪽으로 쭉감
                AutoMove();
                CheckPlayer(attackDistance);
                break;

            case EnemyState.FIGHT:// 전투
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;// 전투 시 그 자리에 바로 멈춤
                if (isReady == false)    // 준비 동작 끝나면
                    gun.Fire();
                break;
            case EnemyState.STUN:// 스턴
                if (isStun == false)
                {
                    curEnemyState = EnemyState.IDLE;
                }
                break;
            case EnemyState.DEAD: // 시체
                break;
        }
    }

    void ReadyComplete()
    {
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
