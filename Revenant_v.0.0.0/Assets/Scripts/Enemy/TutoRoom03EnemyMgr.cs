using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRoom03EnemyMgr : MonoBehaviour
{
    [SerializeField]
    GameObject m_Turret;
    
    public void TurretToggle(bool _input)
    {
        m_Turret.SetActive(_input);
    }
}
