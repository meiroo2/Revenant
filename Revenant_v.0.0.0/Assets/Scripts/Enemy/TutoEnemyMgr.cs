using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutoEnemyMgr : MonoBehaviour
{
    TuRoom02_ProgressMgr m_room2ProgressMgr;

    [SerializeField]
    GameObject m_Drone;

    public int m_allDieCount { get; private set; } = 0;

    // trigger counts
    int m_targetCount = 3;
    int m_droneCount = 4;

    private void Awake()
    {
        m_room2ProgressMgr = GameObject.Find("ProgressMgr").GetComponent<TuRoom02_ProgressMgr>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TargetBoardToggle(true);
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DroneToggle(true);
            RespawnTarget();
        }
            
    }

    public List<TargetBoard_Controller> m_TargetList { get; set; }= new List<TargetBoard_Controller>();
    public List<Drone_Controller> m_droneList { get; set; } = new List<Drone_Controller>();
    public List<Turret_Controller> m_turretList { get; set; } = new List<Turret_Controller>();

    // ============================유환진이 필요한 기능==================================

    // 1. 필요한 타겟을 생성하는 함수(5번)
    public void TargetBoardToggle(bool _input)
    {
        foreach (var t in m_TargetList)
        {
            t.gameObject.SetActive(_input);
        }
    }

    // 2. 과녁 위치(Position)을 알 수 있는 get 함수(5번)
    public Transform GetFirstTargetPos()
    {
        Debug.Log(m_TargetList[0].transform);
        return m_TargetList[0].transform;
    }


    // 4. 드론을 생성하는 함수(7번)
    public void DroneToggle(bool _input)
    {
        m_droneList[0].gameObject.SetActive(_input);
    }

    // + 과녁을 부활&자동으로 리스폰을 하게 해주는 함수(7번)
    public void RespawnTarget()
    {
        foreach (var t in m_TargetList)
        {
            t.Respawn();
        }
    }

    // ======================================================================================


    // 3. 과녁 3개 파괴 시 (6번)
    // 5. 드론 파괴 시 (8번)
    public void PlusDieCount()
    {
        m_allDieCount++;
        if (m_allDieCount == m_targetCount)
        {
            
            Debug.Log("Target 3 die");
            foreach(var t in m_TargetList)
            {
                t.m_canRespawn = true;
            }
            m_room2ProgressMgr.SendMessage("NextProgress");
        }
        else if (m_allDieCount == m_droneCount)
        {
            Debug.Log("Drone 1 die");
            m_room2ProgressMgr.SendMessage("NextProgress");
        }

    }
}
