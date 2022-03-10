using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed = 0.1f;

    [field: SerializeField]
    public float damage { get; set; }

    [SerializeField]
    float stun = 3.0f;

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
                collision.GetComponentInParent<IBulletHit>().BulletHit(new BulletHitInfo(false, damage, stun, transform.position, hitPoint));
                Destroy(gameObject);
                
            }
            else if (collision.CompareTag("Body"))
            {
                hitPoint = HitPoints.BODY;
                collision.GetComponentInParent<IBulletHit>().BulletHit(new BulletHitInfo(false, damage, stun, transform.position, hitPoint));
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
        }



    }
}
