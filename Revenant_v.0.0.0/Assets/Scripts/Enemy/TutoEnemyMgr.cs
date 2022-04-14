using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutoEnemyMgr : MonoBehaviour
{

    GameObject[] m_TargetBoard;

    [SerializeField]
    GameObject m_Drone;


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
        //foreach (var i in m_TargetBoards)
        //{
        //    i.SetActive(_input);
        //}
                
        
    }

    
}
