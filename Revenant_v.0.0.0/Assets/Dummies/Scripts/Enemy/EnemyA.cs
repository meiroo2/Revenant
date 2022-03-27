using System.Collections;
using UnityEngine;
using TMPro;


public enum DIR
{
    LEFT, RIGHT, UP, DOWN, STOP
}
public class EnemyA : Human, IAttacked
{
    [field: SerializeField]
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    public GameObject textForTest;

    bool isAlive = true;
    Rigidbody2D rigid;
    Animator animator;
    CircleCollider2D guardHearCollider;

    // �̵� ����
    public DIR defaultDir;
    DIR curDir;

    // �̵� ��ġ
    public Vector2 sensorPos;

    // ��������
    [field: SerializeField]
    public float stunStack { get; set; }
    float curStun;
    bool isStun = false;
    public GameObject stunMark;

    // ����� ����
    [field: SerializeField]
    public float m_preIdleTime { get; set; } = 1.0f;

    // �߰� ����
    [field: SerializeField]
    public float m_preGuardTime { get; set; } = 1.0f;
    // �߰� �þ� �Ÿ� - �ð�
    [field: SerializeField]
    public float guardSightDistance { get; set; }    // �߰� �Ÿ�

    // ���� �Ÿ�
    [field: SerializeField]
    public float fightSightDistance { get; set; } = 1.5f;   // ���� �Ÿ�
    public GameObject detectMark;

    // ��� ����
    [field: SerializeField]
    public float m_preFightTime { get; set; }
    bool isReady = false; // true �� ��� �Ұ�

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
        enemyManager = GetComponentInParent<EnemyManager>();

        gun = GetComponentInChildren<Gun>();

        curStun = stunStack;

        m_SFXMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();
    }
    private void Update()
    {
        textForTest.GetComponent<TextMeshProUGUI>().text = curEnemyState.ToString();
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

        // ���
        if (m_Hp - damage <= 0)
        {
            Debug.Log(name + " Die");
            isAlive = false;

            if(enemyManager)
                enemyManager.PlusDieCount();

            Destroy(gameObject);
        }
        // ����
        else if(curStun - stun <= 0)
        {
            m_Hp -= damage;
            Debug.Log(name + " stunned ");
            
            StunAIState();
            curStun = stunStack; // ���� �ʱ�ȭ
        }
        // �ǰ�
        else
        {
            //Debug.Log(name + " damaged: " + damage);
            m_Hp -= damage;
            Debug.Log(name + " stun damage: " + stun);
            curStun -= stun;
        }
        
    }


    // �߰� - - - - - - - - - - - - - - - - - - - - - - - - - - -
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
    // �̵� �� ��ü ȸ��
    // �¿� ȸ���� ����

    public void Rotation()
    {
        // ����: ��, ���: ��
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

    // �̵�

    public void Move()
    {
        Rotation();
        // �����Ҷ����� �̵�
        if (Destination() != true)
        {
            //Debug.Log("destination = false");
            
            if (m_isRightHeaded)
                rigid.velocity = new Vector2(1, 0);
            else
                rigid.velocity = new Vector2(-1, 0);
        }
    }

    // ����
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

    // ���� ǥ��
    // * �ڱ� ���� ��ġ�� ����
    public void Sensor()
    {
        // �߰� �ڱ�
        RaycastHit2D guardRayHit2D;

        // ���� �ڱ�
        RaycastHit2D fightRayHit2D;
        if (m_isRightHeaded)
        {
            // �߰� ����
            Debug.DrawRay(transform.position, Vector3.right * guardSightDistance, Color.magenta);
            guardRayHit2D = Physics2D.Raycast(transform.position, Vector3.right, guardSightDistance, LayerMask.GetMask("Player"));

            // ���� ����
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
        // ����� / �߰�-> ����
        if (fightRayHit2D)
        {
            sensorPos = fightRayHit2D.collider.transform.position;
            FightAIState();
        }
        // ����� / ���� -> �߰�
        else if(guardRayHit2D)
        {
            sensorPos = guardRayHit2D.collider.transform.position;
            GuardAIState();
        }
        
    }


    void StunAIState()
    {
        isStun = true;
        // ��
        detectMark.SetActive(false);
        stunMark.SetActive(true);

        // ���� ����: ���� �Ա� �� ����
        if (curEnemyState != EnemyState.STUN) // ������ ���� �����ϸ� ���� ���Ͽ� �ɸ� ��
            nextEnemyState = curEnemyState;
        curEnemyState = EnemyState.STUN;

        Invoke(nameof(StunComplete), m_stunTime);
    }

    void StunComplete()
    {
        isStun = false;
        stunMark.SetActive(false);
    }

    public void GuardAIState()
    {
        // ���� (���� ���� ���� ������)
        Invoke(nameof(PreGuardComplete), m_preGuardTime);
    }

    void PreGuardComplete()
    {
        curEnemyState = EnemyState.GUARD;
    }

    void FightAIState()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;// ���� �� �� �ڸ��� �ٷ� ����

        // ���� (���� ���� ���� ������)
        isReady = true;
        Invoke(nameof(ReadyComplete), m_preFightTime);

        // �ִϸ��̼�
        animator.SetBool("isFight", true);

        // !
        detectMark.SetActive(true);

        // ���� ����
        curEnemyState = EnemyState.FIGHT;

    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// ���
                // �÷��̾� ������ �߰�
                Sensor();
                break;

            case EnemyState.GUARD:// �߰�
                // �������� �߰�
                //AutoMove();
                Move();
                Sensor();
                break;

            case EnemyState.FIGHT:// ����
                
                if (isReady == false)    // �غ� ���� ������
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// ���� ����
                    gun.Fire();
                    Sensor();
                }
                    
                break;
            case EnemyState.STUN:// ����
                if (isStun == false)
                {
                    curEnemyState = nextEnemyState; // ���� ����: ���� �Ա� �� ����
                    //Sensor();
                    //curEnemyState = EnemyState.IDLE;
                }
                break;
            case EnemyState.DEAD: // ��ü
                break;
        }
    }

    void ReadyComplete()
    {
        isReady = false;
    }

    // ��ħ ����
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
        curDir = DIR.STOP; // ���� �̵����Ⱚ STOP
        
    }

    void unOverLap()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
