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

    // 경직정도
    [field: SerializeField]
    public float stunStack { get; set; }

    // 감지거리
    //[field: SerializeField]
    //public float detectDistance { get; set; }

    // 사정거리(감지)
    [field: SerializeField]
    public float attackDistance { get; set; }

    // 사격 준비 
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
        // 사망
        if (m_Hp - damage <= 0)
        {
            Debug.Log(name + " Die");
            isAlive = false;

            if(enemyManager)
                enemyManager.PlusDieCount();

            Destroy(gameObject);
        }
        // 피격
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
        //Vector3 purposePos;
        if (m_isRightHeaded)
        {
            Debug.DrawRay(transform.position, Vector3.right * attackDistance, Color.magenta);
            //purposePos = new Vector2(transform.position.x + detectDistance, transform.position.y + detectDistance);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.right, attackDistance, LayerMask.GetMask("Player"));
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * attackDistance, Color.magenta);
            //purposePos = new Vector2(transform.position.x - detectDistance, transform.position.y - detectDistance);
            rayHit2D = Physics2D.Raycast(transform.position, Vector3.left, attackDistance, LayerMask.GetMask("Player"));
            
        }
        //Ray2D ray2d = new Ray2D(transform.position, purposePos);
        //Debug.DrawRay(transform.position, Vector2.right * detectDistance, Color.magenta);
        if(rayHit2D)
            curEnemyState = EnemyState.FIGHT;
        
        //Debug.Log(purposePos);

    }

    public void AI()
    {
        switch(curEnemyState)
        {
            case EnemyState.IDLE:// 무방비
                // 왼쪽으로 쭉감
                AutoMove(DIR.LEFT);
                // 플레이어 만나면 전투
                CheckPlayer();
                break;
            case EnemyState.GUARD:// 경계
                break;
            case EnemyState.FIGHT:// 전투
                gun.Fire();
                break;
            case EnemyState.DEAD: // 시체
                break;
        }
    }
}
