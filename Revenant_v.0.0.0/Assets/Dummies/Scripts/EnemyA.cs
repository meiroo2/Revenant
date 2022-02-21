using System.Collections;
using UnityEngine;

public class EnemyA : MonoBehaviour, IBulletHit
{
    
    float Hp = 4;
    SpriteRenderer[] spriteRenderers;
    bool isAlive = true;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
            Damaged(damage);
    }

    public void Damaged(float damage)
    {
        
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

    public void Move()
    {
        
    }
}
