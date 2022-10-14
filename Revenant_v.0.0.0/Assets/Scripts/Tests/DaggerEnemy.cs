using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DaggerEnemyState
{
   IDLE,
   FOLLOW,
   ATTACK,
   DEAD,
   ALERT
}

public class DaggerEnemy : Human
{
    // Visible Variables
    public float p_VisionDistance = 1f;
    public float p_StickDistance = 0.2f;

    public GameObject p_Bullet;

    // Member Variables
    private Rigidbody2D m_EnemyRigid;
    private SuperArmorMgr m_SuperArmorMgr;

    public LocationInfo m_NoiseSourceLocation { get; private set; }
    public LocationInfo m_curLocation { get; private set; }

    public Transform m_PlayerTransform { get; private set; } = null;
    public float m_DistanceBetweenPlayer { get; set; } = 0;
    public RaycastHit2D m_VisionHit { get; private set; }
    public EnemyStates m_CurState { get; private set; }

    private void Awake()
    {
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_SuperArmorMgr = GetComponentInChildren<SuperArmorMgr>();

        m_CurState = new IDLE_Dagger();
        m_CurState.StartState(this);

        //m_curLocation = new LocationInfo(0, 0, 0, Vector2.zero);
    }

    private void Start()
    {
        if (m_PlayerTransform is null)
            m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
    }

    private void Update()
    {
        m_CurState.UpdateState();
    }
    private void FixedUpdate()
    {
        RaycastVisionCheck();
    }

    private void RaycastVisionCheck()
    {
        if (m_IsRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.4f), Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.4f), Vector2.right * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.4f), -Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.4f), -Vector2.right * p_VisionDistance, Color.red);
        }
    }
    public void MoveByDirection(int _direction)
    {
        if (_direction == 1)
        {
            if (!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = Vector2.right * p_MoveSpeed;
        }
        else if(_direction == -1)
        {
            if (m_IsRightHeaded)
                setisRightHeaded(false);

            m_EnemyRigid.velocity = -Vector2.right * p_MoveSpeed;
        }
    }

    public void ChangeFSMState(EnemyStates _input)
    {
        m_CurState.ExitState();
        m_CurState = _input;
        m_CurState.StartState(this);
    }

    public void GetHit(int _inputDamage)
    {
        p_Hp -= _inputDamage;
        if(p_Hp <= 0)
        {
            ChangeFSMState(new Dead_Dagger());
        }
        else
        {
            ChangeFSMState(new FOLLOW_Dagger());
        }
    }

    public void Attack()
    {
        StartCoroutine(Internal_Attack(0.1f));
    }

    public void doSuperArmor()
    {
        m_SuperArmorMgr.doSuperArmor();
        m_SuperArmorMgr.SetCallback(Attack, true);
    }

    public void setNoiseSourceLocation(LocationInfo _location)
    {
        m_NoiseSourceLocation = _location;
        if (m_CurState.m_StateEnum == DaggerEnemyState.IDLE)
        {
            ChangeFSMState(new Alert_Dagger());
        }
        else
        {
            //Debug.Log("asda");
        }
    }

    IEnumerator Internal_Attack(float _AttackTime) 
    {
        p_Bullet.SetActive(true);
        yield return new WaitForSeconds(_AttackTime);
        p_Bullet.SetActive(false);
        m_CurState.NextPhase();
    }

    public void setEntityLocation(LocationInfo _location)
    {
        //m_curLocation.setLocation(_location);
    }
}