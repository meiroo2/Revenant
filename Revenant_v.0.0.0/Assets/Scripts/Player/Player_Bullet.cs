using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    // Visible Member Variables
    public SoundMgr_SFX m_SoundMgrSFX;

    // Member Variables
    private float m_Speed = 0f;
    private float m_Timer = 0f;
    private int m_Damage = 0;
    private HitPoints m_HitPoint = HitPoints.OTHER;
    public int m_aimedObjId { get; set; } = 0;
    public GameObject m_HitEffect;

    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {

    }
    public void InitBullet(float _speed, int _damage)
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
        //Debug.Log(m_aimedObjId + " " + collision.gameObject.GetInstanceID());

        if (collision.gameObject.CompareTag("Head"))
            m_HitPoint = HitPoints.HEAD;
        else if (collision.gameObject.CompareTag("Body"))
            m_HitPoint = HitPoints.BODY;
        else
            m_HitPoint = HitPoints.OTHER;

        
        if (m_aimedObjId == collision.gameObject.GetInstanceID() && m_HitPoint != HitPoints.OTHER)
        {
            collision.gameObject.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(true, m_Damage, 1, transform.position, m_HitPoint, WeaponType.BULLET));
            GameObject _effect = Instantiate(m_HitEffect);

            if (m_Speed < 0)
                _effect.transform.localScale = new Vector2(-_effect.transform.localScale.x, _effect.transform.localScale.y);

            _effect.transform.SetPositionAndRotation(transform.position, this.gameObject.transform.rotation);

            Destroy(this.gameObject);
        }
        else if(m_HitPoint == HitPoints.OTHER)
        {
            collision.gameObject.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(true, m_Damage, 1, transform.position, m_HitPoint, WeaponType.BULLET));

            GameObject _effect = Instantiate(m_HitEffect);

            if (m_Speed < 0)
                _effect.transform.localScale = new Vector2(-_effect.transform.localScale.x, _effect.transform.localScale.y);

            _effect.transform.SetPositionAndRotation(transform.position, this.gameObject.transform.rotation);

            Destroy(this.gameObject);
        }
        
    }

    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
