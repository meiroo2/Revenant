using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : BULLET
{
    [SerializeField]
    float speed = 0.1f;

    public int damage { get; set; } = 10;

    [SerializeField]
    int stun = 3;


    private void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        //transform.position = new Vector2(transform.position.x- speed, transform.position.y);
    }

    public void Init()
    {
        m_isPlayers = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHotBox hotbox = collision.GetComponent<IHotBox>();
        
        hotbox.HitHotBox(new IHotBoxParam(damage, stun, transform.position, WeaponType.BULLET));

        Destroy(gameObject);
    }
}
