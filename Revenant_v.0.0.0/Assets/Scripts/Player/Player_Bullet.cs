using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    // Visible Member Variables

    // Member Variables
    private float m_Speed = 0f;
    private float m_Timer = 0f;
    private int m_Damage = 0;
    private HitPoints m_HitPoint = HitPoints.OTHER;
    public int m_aimedObjId { get; set; } = 0;
    private HitSFXMaker m_HitSFXMaker;
    private SoundMgr_SFX m_SoundMgrSFX;
    private PlayerRotation m_PlayerRotation;

    // Constructors
    private void Start()
    {
        m_HitSFXMaker = GameManager.GetInstance().GetComponentInChildren<HitSFXMaker>();
        m_SoundMgrSFX = GameManager.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
        m_PlayerRotation = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerRotation;
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
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Head"))
                m_HitPoint = HitPoints.HEAD;
            else if (collision.gameObject.CompareTag("Body"))
                m_HitPoint = HitPoints.BODY;
            else
                m_HitPoint = HitPoints.OTHER;


            if (m_aimedObjId == collision.gameObject.GetInstanceID() && m_HitPoint != HitPoints.OTHER)
            {
                collision.gameObject.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(true, m_Damage, 1, transform.position, m_HitPoint, WeaponType.BULLET));

                if (m_HitPoint == HitPoints.HEAD)
                    m_HitSFXMaker.EnableNewObj(0, transform.position, transform.rotation, (m_Speed > 0f) ? true : false);
                else if (m_HitPoint == HitPoints.BODY)
                    m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_Speed > 0f) ? true : false);

                Destroy(this.gameObject);
            }
            else if (m_HitPoint == HitPoints.OTHER)
            {
                collision.gameObject.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(true, m_Damage, 1, transform.position, m_HitPoint, WeaponType.BULLET));

                m_HitSFXMaker.EnableNewObj(Random.Range(1, 3), transform.position, transform.rotation, (m_Speed > 0f) ? true : false);

                Destroy(this.gameObject);
            }
        }
    }

    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
