using System.Collections;
using UnityEngine;


public enum DIR
{
    LEFT, RIGHT, UP, DOWN
}
public class EnemyA : Human, IBulletHit
{

    bool isAlive = true;
    Rigidbody2D rigid;

    // ��������
    [field: SerializeField]
    public float stunStack { get; set; }
    float curStun;
    bool isStun = false;
    public GameObject stunMark;

    // �����Ÿ�
    //[field: SerializeField]
    //public float detectDistance { get; set; }

    // �����Ÿ�(����)
    [field: SerializeField]
    public float attackDistance { get; set; }
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

    private void Awake()
    {
        setisRightHeaded(false);
        rigid = GetComponent<Rigidbody2D>();
        enemyManager = GetComponentInParent<EnemyManager>();

        gun = GetComponentInChildren<Gun>();

        curStun = stunStack;
    }

    private void FixedUpdate()
    {
        AI();
    }

    public void BulletHit(BulletHitInfo _bulletHitInfo)
    {
        float damage = _bulletHitInfo.m_Damage;
        float stun = _bulletHitInfo.m_StunValue;

        if (_bulletHitInfo.m_HitPoint == HitPoints.HEAD)
        {
            Debug.Log("Head Hit");
            damage *= 2;
            stun *= 2;
        }
        else if (_bulletHitInfo.m_HitPoint == HitPoints.BODY)
        {
            Debug.Log("Body Hit");
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



    public void AutoMove(DIR _dir)
    {
        switch(_dir)
        {
            case DIR.LEFT:
                rigid.velocity = new Vector2(-1, 0);
                break;
            case DIR.RIGHT:
                rigid.velocity = new Vector2(1, 0);
                break;
            default:
                Debug.Log("There is No Dir Move Code");
                break;
        }
    }

    public void CheckPlayer()
    {
        RaycastHit2D rayHit2D;
        if (m_isRightHeaded)
        {
            Debug.DrawRay(transform.position, Vector3.right * attackDistance, Color.magenta);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.right, attackDistance, LayerMask.GetMask("Player"));
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * attackDistance, Color.magenta);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.left, attackDistance, LayerMask.GetMask("Player"));
            
        }
        if(rayHit2D)
        {
            FightAIState();
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

    void FightAIState()
    {
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
            case EnemyState.IDLE:// �����
                // �÷��̾� ������ ����
                CheckPlayer();

                // �������� �߰�
                AutoMove(DIR.LEFT);
                break;
            case EnemyState.GUARD:// ���
                break;
            case EnemyState.FIGHT:// ����
                if(isReady == false)    // �غ� ���� ������
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
}
