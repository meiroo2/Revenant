using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_UseRange : MonoBehaviour
{
    // Visible Member Variables
    public Enemy_FootMgr p_Enemy_FootMgr;


    // Member Variables
    private BasicEnemy m_Enemy;
    private StairPos m_StairPos;
    private IUseableObj m_UseableObj;
    private Door_LayerRoom _doorLayer;
    private Door_Col_LayerRoom _door;

    private Player _player;

    private List<NormalGang> NormalGangList;

    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
        NormalGangList = FindObjectsOfType<NormalGang>().ToList();
    }

    private void Start()
    {
        _player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }

    private void Update()
    {
        //Debug.Log("Enemy_UseRange) m_Enemy.WayPointsVectorList.Count - " + m_Enemy.WayPointsVectorList.Count);
        //Debug.Log("Enemy_UseRange) m_Enemy.WayPointsIndex -" + m_Enemy.WayPointsIndex);
        //Debug.Log("Enemy_UseRange) _player._currentWaypointIndex -" + _player._currentWaypointIndex);
    }

    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            return;

        _door = col.GetComponent<Door_Col_LayerRoom>();
        if (m_Enemy.m_CurEnemyFSM._enemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            if (Mathf.Abs(m_Enemy.transform.position.x - m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x) <= 0.1f)
            {
                _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);
                m_Enemy.WayPointsIndex++;
            }

            //m_Enemy.bUsedDoor = true;
            //     m_UseableObj = col.GetComponent<IUseableObj>();
            //
            //     switch (m_UseableObj.m_ObjProperty)
            //     {
            //         case UseableObjList.LAYERDOOR:
            //             m_UseableObj.ActivateOutline(true);
            //             break;
            //     }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        _door = other.GetComponent<Door_Col_LayerRoom>();
        //
        if (m_Enemy.m_CurEnemyFSM._enemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            if (Mathf.Abs(m_Enemy.transform.position.x - m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x) <= 0.1f)
            {
                m_Enemy.bMoveToUsedDoor = true;
                if (m_Enemy.bMoveToUsedDoor)
                {
                    _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);
                    m_Enemy.WayPointsIndex++;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        _door = other.GetComponent<Door_Col_LayerRoom>();
        // if (_door)
        // {
        //     m_Enemy.bMoveToUsedDoor = false;
        //     //Debug.Log("Exit bMoveToUseDoor - " + m_Enemy.bMoveToUsedDoor);
        // }

        if (Mathf.Abs(m_Enemy.transform.position.y - _player.transform.position.y) <= 0.5f && _door)
        {
            m_Enemy.bMoveToUsedDoor = false;
            // m_Enemy.WayPointsIndex = 0;
            // m_Enemy.WayPointsVectorList.Clear();
        }

        // m_UseableObj = other.GetComponent<IUseableObj>();
        //
        // switch (m_UseableObj.m_ObjProperty)
        // {
        //     case UseableObjList.LAYERDOOR:
        //         m_UseableObj.ActivateOutline(false);
        //         break;
        // }
    }
}