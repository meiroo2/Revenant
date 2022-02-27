using System.Collections;
using UnityEngine;


public enum DIR
{
    LEFT, RIGHT, UP, DOWN
}
public class EnemyA : MonoBehaviour, IBulletHit
{
    [SerializeField]
    float Hp = 5;

    bool isAlive = true;
    Rigidbody2D rigid;

    EnemyManager enemyManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    private void Update()
    {
        AutoMove(DIR.LEFT);
    }

    public void BulletHit(float _damage, Vector2 _contactPoint, HitPoints _hitPoints)
    {
        float damage = _damage;

        if (_hitPoints == HitPoints.HEAD)
        {
            Debug.Log("Head Hit");
            damage *= 2;
        }
        else if (_hitPoints == HitPoints.BODY)
        {
            Debug.Log("Body Hit");
        }
        if(isAlive)
            Damaged(damage);

    }

    public void Damaged(float damage)
    {
        // »ç¸Á
        if (Hp - damage <= 0)
        {
            Debug.Log(name + " Die");
            isAlive = false;

            enemyManager.PlusDieCount();

            Destroy(gameObject);
        }
        // ÇÇ°Ý
        else
        {
            Debug.Log(name + " damaged: " + damage);
            Hp -= damage;
        }
        
    }

    public void AutoMove(DIR _dir)
    {
        switch(_dir)
        {
            case DIR.LEFT:
                rigid.velocity = new Vector2(-1, 0);
                break;
            case DIR.RIGHT:
                rigid.velocity = new Vector2(1, 0);
                break;
            default:
                Debug.Log("There is No Dir Move Code");
                break;
        }
        
    }
}
