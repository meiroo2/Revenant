using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed = 0.1f;

    [field: SerializeField]
    public int damage { get; set; }

    [SerializeField]
    int stun = 3;

    HitPoints hitPoint = HitPoints.OTHER;

    // Start is called before the first frame update

    private void Awake()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.position = new Vector2(transform.position.x- speed, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Player>())
        {

            if (collision.CompareTag("Head"))
            {
                hitPoint = HitPoints.HEAD;
                damage *= 2;
                collision.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(false, damage, stun, transform.position, hitPoint, WeaponType.BULLET));
                Destroy(gameObject);
                
            }
            else if (collision.CompareTag("Body"))
            {
                hitPoint = HitPoints.BODY;
                collision.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(false, damage, stun, transform.position, hitPoint, WeaponType.BULLET));
                Destroy(gameObject);
                
            }
            else
            {
                hitPoint = HitPoints.OTHER;
            }
            
        }
        else
        {
            hitPoint = HitPoints.OTHER;
            if (collision.CompareTag("Object"))
            {
                collision.GetComponentInParent<IAttacked>().Attacked(new AttackedInfo(false, damage, stun, transform.position, hitPoint, WeaponType.BULLET));
                Destroy(gameObject);
            }
        }



    }
}
