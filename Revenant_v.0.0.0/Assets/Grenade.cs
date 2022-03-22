using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public float m_BoomTime { get; private set; } = 3f;
    [field: SerializeField] public int m_Damage { get; private set; } = 3;
    [field: SerializeField] public int m_Stunvalue { get; private set; } = 1;

    // Member Variables
    private List<GameObject> m_HitBoxes = new List<GameObject>();
    private AttackedInfo m_AttackedInfo;

    // Constructors
    private void Start()
    {
        Invoke(nameof(Explode), m_BoomTime);
        m_AttackedInfo = new AttackedInfo(true, m_Damage, m_Stunvalue, transform.position, HitPoints.BODY, WeaponType.GRENADE);
    }
    public void InitGrenade(float _BoomTime, int _Damage, int _Stunvalue)
    {
        m_BoomTime = _BoomTime;
        m_Damage = _Damage;
        m_Stunvalue = _Stunvalue;
        m_AttackedInfo.m_Damage = m_Damage;
        m_AttackedInfo.m_StunValue = m_Stunvalue;
    }

    // Updates

    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_HitBoxes.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_HitBoxes.Count > 0)
        {
            for (int i = 0; i < m_HitBoxes.Count; i++)
            {
                if (m_HitBoxes[i] == collision.gameObject)
                {
                    m_HitBoxes.RemoveAt(i);
                    break;
                }
            }
        }
    }

    // Functions
    private void Explode()
    {
        for (int i = 0; i < m_HitBoxes.Count; i++)
        {
            if (m_HitBoxes[i].CompareTag("Body"))
            {
                m_HitBoxes[i].GetComponentInParent<IAttacked>().Attacked(m_AttackedInfo);
            }
        }
        Destroy(transform.parent.gameObject);
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
