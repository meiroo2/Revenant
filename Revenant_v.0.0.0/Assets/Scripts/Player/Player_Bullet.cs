using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : BULLET
{
    // Visible Member Variables
    private int m_Damage = 0;
    private float m_Speed = 0f;
    private int m_stunValue = 0;

    // Member Variables
    private float m_Timer = 0f;
    public int m_aimedObjId { get; set; } = 0;
    private HitSFXMaker m_HitSFXMaker;
    private SoundMgr_SFX m_SoundMgrSFX;
    private PlayerRotation m_PlayerRotation;

    private IHotBox m_BulletHitHotBox;

    private bool m_ShouldDestroy = false;

    // Constructors
    private void Start()
    {
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
        m_PlayerRotation = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerRotation;
    }
    public void InitBullet(float _speed, int _damage)
    {
        m_Speed = _speed;
        m_Damage = _damage;
    }

    // Updates
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(m_Speed * Time.deltaTime, 0f));

        if (m_Timer >= 3f)
            Destroy(this.gameObject);
        else
            m_Timer += Time.deltaTime;
    }

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_BulletHitHotBox = collision.GetComponent<IHotBox>();

        switch (m_BulletHitHotBox.m_hotBoxType)
        {
            case 0:
                if (m_aimedObjId != collision.gameObject.GetInstanceID())
                {
                    Debug.Log("지나감 : " + collision.name + " " + collision.gameObject.GetInstanceID());
                    break;
                }

                Debug.Log("충돌 : " + collision.name + " " + collision.gameObject.GetInstanceID());
                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, m_stunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_Speed > 0f) ? true : false);
                m_ShouldDestroy = true;
                break;

            case 1:
                Debug.Log("반충돌 : " + collision.name + " " + collision.gameObject.GetInstanceID());

                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, m_stunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_Speed > 0f) ? true : false);
                m_ShouldDestroy = true;
                break;
        }

        if (m_ShouldDestroy)
            Destroy(gameObject);
    }

    // Functions
    // m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_Speed > 0f) ? true : false);

    // 기타 분류하고 싶은 것이 있을 경우
}
