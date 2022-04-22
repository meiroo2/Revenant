using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr_DefenseMap : MonoBehaviour
{
    // 하나의 조가 죽으면 새로운 조를 토글해야 해서
    // 조 단위로 진행되어야 함.
    // 조가 모두 사망할 경우 다음 조를 토글

    // 특정 키를 누르면 다음 스폰을 진행해야 함

    // 배열 관리를 위해 인터페이스화를 해봐도 좋을 듯

    public List<IEnemyType> m_Wave1 { get; set; } = new List<IEnemyType>();

    
    public List<IEnemyType> m_Wave2 { get; set; } = new List<IEnemyType>();

    [SerializeField]
    float m_spawnWaitTime = 3.0f; // 시작 후 스폰 대기 시간

    int m_groupIndex = 0; // 스폰 순서
    public void ToggleEnemyList()
    {

        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("- WAVE 1 -");
            foreach(var e in m_Wave1)
            {
                e.getInfo();
            }
            Debug.Log("- WAVE 2 -");
            foreach(var e in m_Wave2)
            {
                e.getInfo();
            }
        }
    }
}
