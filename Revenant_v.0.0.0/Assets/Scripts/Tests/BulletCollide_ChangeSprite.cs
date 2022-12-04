using System;
using UnityEngine;


public class BulletCollide_ChangeSprite : MonoBehaviour
{
    // Visible Member Variables
    public int m_Hp = 2;
    public Sprite m_ChangeSprite;
    public SpriteRenderer m_Renderer;
    public Rigidbody2D m_Rigid;

    // Member Variables
    private bool m_IsHit = false;
    
    // Constructors
    private void Awake()
    {
        m_Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    // Functions
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (m_IsHit || !col.gameObject.CompareTag("Ball"))
            return;

        m_Hp -= 1;
        if (m_Hp > 0)
            return;
        
        if (col.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        
        m_IsHit = true;
        m_Renderer.sprite = m_ChangeSprite;
        m_Rigid.constraints = RigidbodyConstraints2D.None;
    }
}