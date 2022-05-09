using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Explosion : MonoBehaviour
{
    private void Awake()
    {
        // 딜레이 후 데미지
        Invoke(nameof(Damage), m_delayTime);
    }
    // 수류탄 참조하여 구성
    public int m_damage { get; set; } = 10;
    public float m_delayTime { get; set; } = 1f;

    List<GameObject> hits = new List<GameObject>();

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hits.Add(collision.gameObject);
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i] == collision)
                hits.RemoveAt(i);
            break;
        }
            
    }

    void Damage()
    {
        IHotBoxParam param = new IHotBoxParam(m_damage, 0, transform.position, WeaponType.BULLET);
        foreach(var h in hits)
        {
            if(h.CompareTag("Body")||h.CompareTag("Player"))
            h.GetComponent<IHotBox>().HitHotBox(param);
        }
        Destroy(gameObject);
    }
}
