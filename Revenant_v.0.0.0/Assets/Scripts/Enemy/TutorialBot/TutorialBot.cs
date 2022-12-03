using System;
using Sirenix.OdinInspector;
using UnityEngine;


public class TutorialBot : BasicEnemy
{
    // Visible Member Variables
    [BoxGroup("TutorialBot Values")] public float p_AtkSpeed = 1f;
    [BoxGroup("TutorialBot Values")] public float p_AttackDistance = 1f;
    [BoxGroup("TutorialBot Values")] public float p_PointAtkTime = 0.5f;
    [BoxGroup("TutorialBot Values")] public TutorialBot_HotBox p_BodyBox;
    [field: SerializeField, BoxGroup("TutorialBot Values")] private Animator p_Animator;
    [BoxGroup("TutorialBot Values")] public TutorialBot_Weapon m_Weapon;
    
    // Member Variables
    private TutorialBot_IDLE m_IDLE;
    private TutorialBot_ATK m_ATK;
    private TutorialBot_DEAD m_DEAD;
    public CoroutineHandler m_CoroutineHandler { get; private set; }


    // Constructors
    private void Awake()
    {
        m_Animator = p_Animator;

        m_IDLE = new TutorialBot_IDLE(this);
        m_ATK = new TutorialBot_ATK(this);
        m_DEAD = new TutorialBot_DEAD(this);

        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;
    }

    private void Start()
    {
		setisRightHeaded(false);
		m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        m_CurEnemyFSM.StartState();
    }
    
    
    // Updates
    private void FixedUpdate()
    {
		m_CurEnemyFSM.UpdateState();
    }
    
    
    // Functions
    public override void SetHotBoxesActive(bool _isOn)
    {
        p_BodyBox.gameObject.SetActive(_isOn);
    }
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        Debug.Log(_damage);
        p_Hp -= _damage;

        if (p_Hp <= 0)
        {
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }
    }
    
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        m_CurEnemyStateName = _name;

        m_CurEnemyFSM.ExitState();

        switch (m_CurEnemyStateName)
        {
            case EnemyStateName.IDLE:
                m_CurEnemyFSM = m_IDLE;
                break;
            
            case EnemyStateName.ATTACK:
                m_CurEnemyFSM = m_ATK;
                break;

            case EnemyStateName.DEAD:
                m_CurEnemyFSM = m_DEAD;
                break;

            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }

        m_CurEnemyFSM.StartState();
    }
}