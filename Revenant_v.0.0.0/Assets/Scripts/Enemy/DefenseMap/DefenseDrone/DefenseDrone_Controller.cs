using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDrone_Controller : Enemy, IEnemySpawn
{
    

    DefenseDrone_Status status;

    [SerializeField]
    Drone_Animator drone_animator;

    [SerializeField]
    BoxCollider2D[] hotboxes;

    Rigidbody2D rigid;

    [SerializeField]
    GameObject explosionPrefab;

    public Vector2 m_sensorPos { get; set; } // 감지된 위치
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    Vector2 m_rushVec; // 돌진할 방향
    bool m_rushReady = false; // 돌진 준비

    float fixedvalue = 0.05f;

    [SerializeField]
    float m_moveTime = 0.2f;
    [SerializeField]
    float m_moveSpeed = 1;

    bool m_isClear = false;

    private void Awake()
    {
        
        curEnemyState = EnemyState.GUARD;
        gameObject.SetActive(false);

        rigid = GetComponent<Rigidbody2D>();
        status = GetComponent<DefenseDrone_Status>();

        Rotation();
    }

    private void FixedUpdate()
    {
        AI();
    }
    
    public void AI()
    {
        switch (curEnemyState)
        {
            case EnemyState.GUARD:// 추격
                // 이동
                    Move();
                
                break;

            case EnemyState.FIGHT:// 전투

                if (m_rushReady)    // 준비 동작 끝나면
                {
                    //돌진
                    Rush();
                }

                break;
            case EnemyState.DEAD: // 시체
                break;
        }
    }

    void ReadyToTrue()
    {
        m_rushReady = true;
    }

    void Rush()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.velocity = m_rushVec * status.m_rushSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 충돌
        if(collision.gameObject.layer==14
            && curEnemyState != EnemyState.DEAD)
        {
            
            Stop();
            rigid.bodyType = RigidbodyType2D.Kinematic;

            // 지면 폭발
            drone_animator.GroundBombAni();
            curEnemyState = EnemyState.DEAD;

            // 데미지
            MakeExplosion();
        }
        
    }

    bool Detect()
    {
        m_sensorPos = m_playerTransform.position;

        if (m_isRightHeaded)
        {
            if (m_sensorPos.x - transform.position.x < status.m_detectRange
)
            {
                float x = m_sensorPos.x - transform.position.x;
                float y = m_sensorPos.y - transform.position.y;
                m_rushVec = new Vector2(x, y);
                Invoke(nameof(ReadyToTrue), status.m_rushDelayTime);
                return true;
            }
                
        }
        else
        {
            if ((m_sensorPos.x - transform.position.x) * -1 < status.m_detectRange)
            {
                float x = m_sensorPos.x - transform.position.x;
                float y = m_sensorPos.y - transform.position.y;
                m_rushVec = new Vector2(x, y);
                Invoke(nameof(ReadyToTrue), status.m_rushDelayTime);

                return true;
            }

        }
        return false;
    }
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

    public void Move()
    {
        Rotation();
        // 감지할때까지 이동
        if (!Detect())
        {
            if (m_isRightHeaded)
                rigid.velocity = Vector2.right * status.m_Speed;
            else
                rigid.velocity = Vector2.left * status.m_Speed;
        }
        // 감지
        else
        {
            Stop();
            curEnemyState = EnemyState.FIGHT;
        }

    }

    public void Stop()
    {
        rigid.velocity = Vector2.zero;
    }

    public override void Damaged(float stun, float damage)
    {
        status.m_Hp -= damage;
        if (status.m_Hp <= 0)
        {
            curEnemyState = EnemyState.DEAD;
            foreach (var h in hotboxes)
            {
                if (h)
                    h.enabled = false;
            }
                
        }
    }
    // IHotBox
    public void BodyAttacked()
    {
        if(curEnemyState != EnemyState.DEAD)
        {
            curEnemyState = EnemyState.DEAD;
            EnemyMgr_DefenseMap.Instance.PlusDieCount();
            Stop();

            // 본체 파괴 시 떨어짐
            rigid.bodyType = RigidbodyType2D.Dynamic;
            drone_animator.HitMainBodyAni();
        }
        
    }

    public void BombAttacked()
    {
        if (curEnemyState != EnemyState.DEAD)
        {
            curEnemyState = EnemyState.DEAD;
            
            Stop();
            // 폭발 시 떨어지지 않음
            rigid.bodyType = RigidbodyType2D.Kinematic;
            drone_animator.HitBombAni();
            // 폭발 오브젝트 생성
            MakeExplosion();

        }
    }

    public void MakeExplosion()
    {
        // 드론 사망
        EnemyMgr_DefenseMap.Instance.PlusDieCount();

        // 폭발 데미지
        GameObject prefab = Instantiate(explosionPrefab);
        prefab.transform.position = transform.position;
        prefab.GetComponent<CircleCollider2D>().radius = status.m_explosionRange;
        Drone_Explosion explosion = prefab.GetComponent<Drone_Explosion>();
        explosion.m_damage = status.m_damage;
        explosion.m_delayTime = status.m_explodeDelayTime;
    }
    // IEnemySpawn
    public void setActive()
    {
        m_playerTransform = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
        gameObject.SetActive(true);
    }

    public void getInfo()
    {
        Debug.Log(gameObject.name);
    }
}