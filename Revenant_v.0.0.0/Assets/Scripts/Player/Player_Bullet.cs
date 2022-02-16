using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private float m_Speed = 0f;
    private float m_Timer = 0f;
    private float m_Damage = 0f;

    // Constructors
    public void InitBullet(float _speed, float _damage)
    {
        m_Speed = _speed;
        m_Damage = _damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(m_Speed * Time.fixedDeltaTime, 0f, 0f)); // Translate�� �̵�. RigidBody �̵��� �ʿ� ���� ��?

        if (m_Speed > 0)
            Debug.DrawRay(transform.position, transform.right * 0.5f, Color.red);
        else
            Debug.DrawRay(transform.position, -transform.right * 0.5f, Color.red);

        if (m_Timer > 3f)
            Destroy(this.gameObject);
        else
            m_Timer += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<IBulletHit>().BulletHit(m_Damage);
        Destroy(this.gameObject);
    }
}
