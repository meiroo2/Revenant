using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenseEnemy : Enemy
{

    [SerializeField]
    Parts parts;

    [field: SerializeField]
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    TextMeshProUGUI textForTest;

    bool isAlive = true;
    Rigidbody2D rigid;
    Animator animator;

    CircleCollider2D guardHearCollider;

    public bool m_canSensor { get; set; } = false;

   // �̵� ��ġ
   [field: SerializeField]
    public Vector2 m_sensorPos { get; set; }
    Vector2 originalPos { get; set; }

    public Transform m_playerTransform { get; set; }


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
    public float guardSightDistance { get; set; } = 2.0f;    // �߰� �Ÿ�

    // ���� �Ÿ�
    [field: SerializeField]
    public float fightSightDistance { get; set; } = 1.5f;   // ���� �Ÿ�
    public GameObject detectMark;

    // ��� ����
    [field: SerializeField]
    public float m_preFightTime { get; set; }
    bool isReady = false; // true �� ��� �Ұ�


    
    EnemyState nextEnemyState;

    [field: SerializeField]
    public Enemy_Gun m_gun { get; set; }

    private SoundMgr_SFX m_SFXMgr;

    private void Awake()
    {
        setisRightHeaded(false);
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        curStun = stunStack;
        originalPos = transform.position;
    }
    private void Update()
    {
        if (textForTest)
            textForTest.text = curEnemyState.ToString();
    }

    private void FixedUpdate()
    {
        if (isAlive)
            AI();
    }

    public override void Damaged(float stun, float damage)
    {
        
        GuardAIState();

        // ���
        if (m_Hp - damage <= 0)
        {
            //Debug.Log(name + " Die");
            isAlive = false;

            EnemyMgr_DefenseMap.Instance.PlusDieCount();

            gameObject.SetActive(false);
            isAlive = false;
        }
        // ����
        else if (curStun - stun <= 0)
        {
            m_Hp -= damage;
            //Debug.Log(name + " stunned ");

            StunAIState();
            curStun = stunStack; // ���� �ʱ�ȭ
        }
        // �ǰ�
        else
        {
            //Debug.Log(name + " damaged: " + damage);
            m_Hp -= damage;
            //Debug.Log(name + " stun damage: " + stun);
            curStun -= stun;
        }
    }

    // �̵� �� ��ü ȸ��
    // �¿� ȸ���� ����

    public void Rotation()
    {
        // ����: ��, ���: ��
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

    // �̵�

    public void Move()
    {
        //Rotation();
        // �����Ҷ����� �̵�
        if (!Destination())
        {
            // �ִϸ��̼�
            animator.SetBool("isWalk", true);
            if (m_isRightHeaded)
                rigid.velocity = new Vector2(1, 0);
            else
                rigid.velocity = new Vector2(-1, 0);
        }
        // ����
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    // ����
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

    // ���� ǥ��
    // * �ڱ� ���� ��ġ�� ����
    public void Sensor()
    {
        if(m_canSensor)
        {
            m_sensorPos = m_playerTransform.position;
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
            if (!isStun)
            {
                // ���� �Ÿ��� �÷��̾�
                // ����� / �߰�-> ����
                if (fightRayHit2D)
                {
                    //m_sensorPos = fightRayHit2D.collider.transform.position;

                    // ���� ������(0 ~ 2��)
                    // ���� �ʼ� �Ŀ� FightAIState �ϼ�

                    if (curEnemyState != EnemyState.FIGHT)
                    {
                        Invoke(nameof(FightAIState), Random.Range(0f, 2.0f));
                    }
                        
                }
                // ���� -> �߰�
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
        Debug.Log("stunState");
        //rigid.velocity = new Vector2(0, 0);// �̵� ����
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;// ���� �� �� �ڸ��� �ٷ� ����

        // �ִϸ��̼�
        animator.SetTrigger("Stun");
        animator.SetBool("isStun", true);

        isStun = true;
        // ��
        if (detectMark)
            detectMark.SetActive(false);
        if (stunMark)
            stunMark.SetActive(true);

        // ���� ����: ���� �Ա� �� ����
        if (curEnemyState != EnemyState.STUN) // ������ ���� �����ϸ� ���� ���Ͽ� �ɸ� ��
            nextEnemyState = curEnemyState;
        curEnemyState = EnemyState.STUN;

        Invoke(nameof(StunComplete), m_stunTime);
    }

    void StunComplete()
    {
        Debug.Log("stunComplete");
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// ���� �� �� �ڸ��� �ٷ� ����
        // �ִϸ��̼�
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
            // �ִϸ��̼�
            animator.SetBool("isReady", false);

            // ���� ����
            parts.setSpriteParts(false);

            // ���� (���� ���� ���� ������)
            Invoke(nameof(PreGuardComplete), m_preGuardTime);
        }

    }

    void PreGuardComplete()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        // �ִϸ��̼�
        animator.SetBool("isReady", true);
        curEnemyState = EnemyState.GUARD;

    }

    void FightAIState()
    {
        if (!isStun)
        {
            Rotation();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;// ���� �� �� �ڸ��� �ٷ� ����

            // ���� (���� ���� ���� ������)
            isReady = true;
            Invoke(nameof(ReadyComplete), m_preFightTime);

            // �ִϸ��̼�
            animator.SetBool("isWalk", false);
            animator.SetBool("isFight", true);

            // ���� Ʋ��
            parts.setSpriteParts(true);

            //Debug.Log("FightAnim");
            // !
            if (detectMark)
                detectMark.SetActive(true);

            // ���� ����
            curEnemyState = EnemyState.FIGHT;
        }

    }

    public void AI()
    {
        switch (curEnemyState)
        {
            case EnemyState.IDLE:// ���
                // �÷��̾� ������ �߰�
                Sensor();
                break;

            case EnemyState.GUARD:// �߰�
                Sensor();
                // �̵�
                Move();
                break;

            case EnemyState.FIGHT:// ����

                if (isReady == false)    // �غ� ���� ������
                {
                    //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;// ���� ����
                    m_gun.Fire();
                    Sensor();

                }

                break;
            case EnemyState.STUN:// ����

                if (isStun == false) // ���� �Ϸ�
                {
                    if (nextEnemyState == EnemyState.FIGHT)
                    {
                        FightAIState();
                    }

                    else
                        curEnemyState = nextEnemyState; // ���� ����: ���� �Ա� �� ����

                }
                break;
            case EnemyState.DEAD: // ��ü
                break;
        }
    }

    void ReadyComplete()
    {
        //Debug.Log("ReadyComplete");
        isReady = false;
    }

}
