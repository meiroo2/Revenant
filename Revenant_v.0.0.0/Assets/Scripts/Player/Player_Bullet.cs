using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private float m_Speed = 0f;
    private float m_Timer = 0f;
    private float m_Damage = 0f;

    public int m_aimedObjId = 0;

    // Constructors
    public void InitBullet(float _speed, float _damage)
    {
        m_Speed = _speed;
        m_Damage = _damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(m_Speed * Time.fixedDeltaTime, 0f, 0f)); // Translate로 이동. RigidBody 이동할 필요 없을 듯?

        if (m_Speed > 0)
            Debug.DrawRay(transform.position, transform.right * 0.5f, Color.red);
        else
            Debug.DrawRay(transform.position, -transform.right * 0.5f, Color.red);

        if (m_Timer > 3f)
            Destroy(this.gameObject);
        else
            m_Timer += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_aimedObjId == collision.gameObject.GetInstanceID())
        {
            int i = 2;
            if (collision.gameObject.tag == "Head")
                i = 0;
            else if (collision.gameObject.tag == "Body")
                i = 1;

            collision.gameObject.GetComponentInParent<IBulletHit>().BulletHit(m_Damage, i);
            Destroy(this.gameObject);
        }
    }

}
