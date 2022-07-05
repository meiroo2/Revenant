using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // BombActivate 함수 호출 시 근처 Hotbox에 Damage만큼 타격 가함
    // 플레이어, 적 상관 없이 사용 가능. HotBox List를 빼면 최적화에 도움이 될 듯?
    
    // Visible Member Variables
    public float p_BombRange = 1f;
    public int p_BombDamage = 10;
    
    // Member Variables
    private List<Collider2D> m_Cols = new List<Collider2D>();
    private List<IHotBox> m_Hotboxes = new List<IHotBox>();
    private CircleCollider2D m_CircleCollider;
    
    // Constructors
    private void Awake()
    {
        m_CircleCollider = GetComponent<CircleCollider2D>();
        m_CircleCollider.radius = p_BombRange;
    }
    
    // Updates
    
    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Cols.Add(col);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Cols.Remove(other);
    }

    public void BombActivate()
    {
        m_Hotboxes.Clear();
        foreach (var collider in m_Cols)
        {
            m_Hotboxes.Add(collider.GetComponent<IHotBox>());
        }

        foreach (var hotbox in m_Hotboxes)
        {
            hotbox.HitHotBox(new IHotBoxParam(p_BombDamage, 0, transform.position, WeaponType.GRENADE));
        }
    }
}
