using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr_DefenseMap : MonoBehaviour
{
    // 하나의 조가 죽으면 새로운 조를 토글해야 해서
    // 조 단위로 진행되어야 함.
    // 조가 모두 사망할 경우 다음 조를 토글


    // 배열 관리를 위해 인터페이스화를 해봐도 좋을 듯
    public List<Enemy00> m_enemy00List { get; set; } = new List<Enemy00>();
    public List<Enemy01> m_enemy01List { get; set; } = new List<Enemy01>();
    public List<Enemy02> m_enemy02List { get; set; } = new List<Enemy02>();


    [SerializeField]
    float m_spawnWaitTime = 3.0f; // 시작 후 스폰 대기 시간

    int m_groupIndex = 0; // 스폰 순서
    public void ToggleEnemyList() // 맨 처음엔 0, 1을 둘다 스폰
                                  // 그 다음엔 하나씩 스폰(리스폰)
    {
        switch(m_groupIndex)
        {
            case 0:
                foreach(var e in m_enemy00List)
                {
                    e.enabled = true;
                }
                break;
            case 1:
                foreach (var e in m_enemy01List)
                {
                    e.enabled = true;
                }
                break;
            case 2:
                foreach (var e in m_enemy02List)
                {
                    e.enabled = true;
                }
                break;
            default:
                Debug.Log("<Error> Group Index");
                break;
        }
    }
}
