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
    

    // �̵� ����
    public DIR defaultDir;
    DIR curDir;

    // ��������
    [field: SerializeField]
    public float stunStack { get; set; }
    float curStun;
    bool isStun = false;
    public GameObject stunMark;


    // �߰� �Ÿ�
    [field: SerializeField]
    public float guardDistance { get; set; }    // �߰� �Ÿ�

    // ���� �Ÿ�
    [field: SerializeField]
    public float attackDistance { get; set; }   // ���� �Ÿ�
    public GameObject detectMark;

    // ��� �غ�
    [field: SerializeField]
    public float readyTime { get; set; }
    bool isReady = false; // true �� ��� �Ұ�

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
                // ��� -> �߰�
                case EnemyState.IDLE:
                    GuardAIState();
                    break;

                // �߰� -> ����
                case EnemyState.GUARD:
                    FightAIState();
                    break;
            }
        }
            


    }

    void StunAIState()
    {
        isStun = true;
        // ��
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
        // �߰� ����
        curEnemyState = EnemyState.GUARD;
    }

    void FightAIState()
    {
        // �ִϸ��̼�
        animator.SetBool("isFight", true);

        // !
        detectMark.SetActive(true);

        // �غ� ����
        isReady = true;
        Invoke(nameof(ReadyComplete), readyTime);
        
        // ���� ����
        curEnemyState = EnemyState.FIGHT;

    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// ���
                // �÷��̾� ������ �߰�
                CheckPlayer(guardDistance);
                break;

            case EnemyState.GUARD:// �߰�
                // �������� �߰�
                AutoMove();
                CheckPlayer(attackDistance);
                break;

            case EnemyState.FIGHT:// ����
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;// ���� �� �� �ڸ��� �ٷ� ����
                if (isReady == false)    // �غ� ���� ������
                    gun.Fire();
                break;
            case EnemyState.STUN:// ����
                if (isStun == false)
                {
                    curEnemyState = EnemyState.IDLE;
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
