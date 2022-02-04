using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private float m_Speed;

    // Constructors
    public void InitBullet(float _speed)
    {
        m_Speed = _speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(m_Speed * Time.fixedDeltaTime, 0f, 0f)); // Translate로 이동. RigidBody 이동할 필요 없을 듯?
        Debug.DrawRay(transform.position, transform.right * 0.5f, Color.red);
    }
}
