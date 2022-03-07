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

    // �����Ÿ�
    //[field: SerializeField]
    //public float detectDistance { get; set; }

    // �����Ÿ�(����)
    [field: SerializeField]
    public float attackDistance { get; set; }
    public GameObject Mark;

    // ��� �غ�
    [field: SerializeField]
    public float readyTime { get; set; }

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


    }

    private void FixedUpdate()
    {
        AI();
    }

    public void BulletHit(float _damage, Vector2 _contactPoint, HitPoints _hitPoints)
    {
        float damage = _damage;

        if (_hitPoints == HitPoints.HEAD)
        {
            Debug.Log("Head Hit");
            damage *= 2;
        }
        else if (_hitPoints == HitPoints.BODY)
        {
            Debug.Log("Body Hit");
        }
        if(isAlive)
            Damaged(damage);

    }
    
    public void Damaged(float damage)
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
        // �ǰ�
        else
        {
            Debug.Log(name + " damaged: " + damage);
            m_Hp -= damage;
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

    void FightAIState()
    {
        Mark.SetActive(true);
        curEnemyState = EnemyState.FIGHT;
    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// �����
                // �������� �߰�
                AutoMove(DIR.LEFT);
                // �÷��̾� ������ ����
                CheckPlayer();
                break;
            case EnemyState.GUARD:// ���
                break;
            case EnemyState.FIGHT:// ����
                gun.Fire();
                break;
            case EnemyState.DEAD: // ��ü
                break;
        }
    }
}
