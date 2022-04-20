using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [SerializeField]
    GameObject m_enemyPrefab;

    [SerializeField]
    EnemyMgr_DefenseMap m_enemyMgr;

    SpriteRenderer m_sprite;

    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_sprite.enabled = false;
    }
    public void Init()
    {
        // ÇÁ¸®ÆÕ »ý¼º
        GameObject prefab = Instantiate(m_enemyPrefab);
        prefab.transform.position = transform.position;

        switch (prefab.GetComponent<EnemyType_0>().m_index)
        {
            case 0:
                m_enemyMgr.m_enemy00List.Add(prefab.GetComponent<Enemy00>());

                break;
            case 1:
                m_enemyMgr.m_enemy01List.Add(prefab.GetComponent<Enemy01>());
                break;
            case 2:
                m_enemyMgr.m_enemy02List.Add(prefab.GetComponent<Enemy02>());
                break;
        }
        
    }
}
