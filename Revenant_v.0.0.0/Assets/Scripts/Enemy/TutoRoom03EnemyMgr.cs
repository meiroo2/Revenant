using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRoom03EnemyMgr : MonoBehaviour
{
    public List<Turret_Controller> m_turretList { get; set; } = new List<Turret_Controller>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TurretToggle(true);
        }
    }

    // 1. ÅÍ·¿ »ý¼º
    public void TurretToggle(bool _input)
    {
        m_turretList[0].gameObject.SetActive(_input);
    }



}
