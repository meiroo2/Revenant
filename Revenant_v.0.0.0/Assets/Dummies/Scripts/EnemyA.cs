using System.Collections;
using UnityEngine;

public class EnemyA : MonoBehaviour, IBulletHit
{
    
    float Hp = 50;
    SpriteRenderer[] spriteRenderers;
    Color originHead;
    Color originBody;

    bool isAlive = true;
    Rigidbody2D rigid;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        originHead = spriteRenderers[0].color;
        originBody = spriteRenderers[1].color;
    }
    private void Update()
    {
        //Move();
    }

    public void BulletHit(float _damage, int _hitPoint)
    {
        float damage = _damage;

        if (_hitPoint == 0)
        {
            damage *= 2;
        }
            
        else if (_hitPoint == 1)
        {
            
        }
        if(isAlive && _hitPoint < 2)
            Damaged(damage, _hitPoint);
    }

    public void Damaged(float damage, int hitPoint)
    {
        CancelInvoke(nameof(ColorOrigin));
        ColorRed(hitPoint);

        if (Hp - damage < 0)
        {
            Debug.Log(name + " Die");
            isAlive = false;
            foreach(var i in spriteRenderers)
            {
                i.sprite = null;
            }
        }
        else
        {
            // Turn Red
            Debug.Log(name + " damaged: " + damage);
            Hp -= damage;
        }
        
    }

    public void ColorRed(int hitPoint)
    {
        spriteRenderers[hitPoint].color = Color.red;
        Invoke(nameof(ColorOrigin), 1.5f);
    }

    public void ColorOrigin()
    {
        spriteRenderers[0].color = originHead;
        spriteRenderers[1].color = originBody;
    }


    public void Move()
    {
        rigid.velocity = new Vector2(-1, 0);
    }
}
