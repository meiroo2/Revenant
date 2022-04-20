using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : BULLET
{
    [SerializeField]
    public float m_speed { get; set; } = 0.1f;

    public int m_damage { get; set; } = 10;

    [SerializeField]
    int stun = 3;

    public Vector3 goVector { get; set; } = Vector2.right;

    [SerializeField]
    Enemy_Bullet_Animator m_animator;

    private HitSFXMaker m_HitSFXMaker;

    private void Awake()
    {
        
        Init();
    }
    private void Start()
    {
        m_HitSFXMaker = GameManager.GetInstance().GetComponentInChildren<HitSFXMaker>();
    }
    void FixedUpdate()
    {
        Move();
        
    }

    public void Init()
    {
        m_isPlayers = false;
    }
    
    void Move()
    {
        transform.position += goVector * m_speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHotBox hotbox = collision.GetComponent<IHotBox>();

        if (!hotbox.m_isEnemys)
        {
            hotbox.HitHotBox(new IHotBoxParam(m_damage, stun, transform.position, WeaponType.BULLET));
            m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_speed > 0f) ? true : false);
            m_animator.Fade();
            Destroy(gameObject);
        }

    }

}
