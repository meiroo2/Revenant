using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPos : MonoBehaviour
{
    private Player m_Player;
    private Stair m_ParentStair;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_ParentStair = GetComponentInParent<Stair>();
    }

    public void StairDetectorDetected(int _objID)
    {
        m_ParentStair.isOnStair(_objID);
    }
}
