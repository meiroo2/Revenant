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

    public Vector2 m_sensorPos { get; set; } // ������ ��ġ
    public float SAFE_DISTANCE { get; set; } = 0.1f;

    Vector2 m_rushVec; // ������ ����
    bool m_rushReady = false; // ���� �غ�

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
            case EnemyState.GUARD:// �߰�
                // �̵�
                    Move();
                
                break;

            case EnemyState.FIGHT:// ����

                if (m_rushReady)    // �غ� ���� ������
                {
                    //����
                    Rush();
                }

                break;
            case EnemyState.DEAD: // ��ü
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
        // �ٴ� �浹
        if(collision.gameObject.layer==14
            && curEnemyState != EnemyState.DEAD)
        {
            
            Stop();
            rigid.bodyType = RigidbodyType2D.Kinematic;

            // ���� ����
            drone_animator.GroundBombAni();
            curEnemyState = EnemyState.DEAD;

            // ������
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

    public void Move()
    {
        Rotation();
        // �����Ҷ����� �̵�
        if (!Detect())
        {
            if (m_isRightHeaded)
                rigid.velocity = Vector2.right * status.m_Speed;
            else
                rigid.velocity = Vector2.left * status.m_Speed;
        }
        // ����
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

            // ��ü �ı� �� ������
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
            // ���� �� �������� ����
            rigid.bodyType = RigidbodyType2D.Kinematic;
            drone_animator.HitBombAni();
            // ���� ������Ʈ ����
            MakeExplosion();

        }
    }

    public void MakeExplosion()
    {
        // ��� ���
        EnemyMgr_DefenseMap.Instance.PlusDieCount();

        // ���� ������
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