using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private float m_Speed = 0f;
    private float m_Timer = 0f;
    private float m_Damage = 0f;
    public int m_aimedObjId { get; set; } = 0;

    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {

    }
    public void InitBullet(float _speed, float _damage)
    {
        m_Speed = _speed;
        m_Damage = _damage;
    }

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(m_Speed * Time.deltaTime, 0f));

        if (m_Speed > 0)
            Debug.DrawRay(transform.position, transform.right * 0.5f, Color.red);
        else
            Debug.DrawRay(transform.position, -transform.right * 0.5f, Color.red);

        if (m_Timer >= 3f)
            Destroy(this.gameObject);
        else
            m_Timer += Time.deltaTime;
    }

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_aimedObjId == collision.gameObject.GetInstanceID())
        {
            HitPoints hitPoints = HitPoints.OTHER;

            if (collision.gameObject.tag == "Head")
                hitPoints = HitPoints.HEAD;
            else if (collision.gameObject.tag == "Body")
                hitPoints = HitPoints.BODY;

            collision.gameObject.GetComponentInParent<IBulletHit>().BulletHit(m_Damage, hitPoints);
            Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
