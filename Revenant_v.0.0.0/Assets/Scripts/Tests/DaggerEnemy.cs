using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DaggerEnemyState
{
   IDLE,
   FOLLOW,
   FIGHT,
   DEAD
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
    public Transform m_PlayerTransform { get; private set; } = null;
    public float m_DistanceBetweenPlayer { get; set; } = 0;
    public RaycastHit2D m_VisionHit { get; private set; }
    public EnemyStates m_CurState { get; private set; }

    private void Awake()
    {
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_SuperArmorMgr = GetComponentInChildren<SuperArmorMgr>();
    }

    private void Start()
    {
        if (m_PlayerTransform is null)
            m_PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;

        ChangeFSMState(new IDLE_Dagger());
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
        if (m_isRightHeaded)
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
            if (!m_isRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = Vector2.right * m_Speed;
        }
        else if(_direction == -1)
        {
            if (m_isRightHeaded)
                setisRightHeaded(false);

            m_EnemyRigid.velocity = -Vector2.right * m_Speed;
        }
    }

    public void ChangeFSMState(EnemyStates _input)
    {
        m_CurState = _input;
        m_CurState.StartState(this);
    }

    public void GetHit(int _inputDamage)
    {
        m_Hp -= _inputDamage;
        if(m_Hp <= 0)
        {
            m_CurState.ExitState();
            ChangeFSMState(new Dead_Dagger());
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


    IEnumerator Internal_Attack(float _AttackTime) 
    {
        p_Bullet.SetActive(true);
        yield return new WaitForSeconds(_AttackTime);
        p_Bullet.SetActive(false);
        m_CurState.NextPhase();
    }
}