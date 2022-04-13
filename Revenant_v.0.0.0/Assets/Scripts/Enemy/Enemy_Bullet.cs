using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : BULLET
{
    [SerializeField]
    public float speed { get; set; } = 0.1f;

    public int damage { get; set; } = 10;

    [SerializeField]
    int stun = 3;

    public Vector3 goVector { get; set; } = Vector2.right;

    private void Awake()
    {
        Init();
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
        transform.position += goVector * speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHotBox hotbox = collision.GetComponent<IHotBox>();

        hotbox.HitHotBox(new IHotBoxParam(damage, stun, transform.position, WeaponType.BULLET));

        Destroy(gameObject);
    }
}
