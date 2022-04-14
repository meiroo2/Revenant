using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutoEnemyMgr : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_TargetBoards;

    [SerializeField]
    GameObject m_Drone;

    [SerializeField]
    GameObject m_Turret;


    private void Awake()
    {

    }
    private void Update()
    {
        
    }

    public void DroneToggle(bool _input)
    {
        m_Drone.SetActive(_input);
    }

    public void TargetBoardToggle(bool _input)
    {
        foreach (var i in m_TargetBoards)
        {
            i.SetActive(_input);
        }
                
        
    }

    public void TurretToggle(bool _input)
    {
        m_Turret.SetActive(_input);
    }
}
