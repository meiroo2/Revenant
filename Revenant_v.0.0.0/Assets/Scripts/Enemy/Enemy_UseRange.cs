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

    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    private void Start()
    {
        _player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            return;

        col.TryGetComponent(out Door_Col_LayerRoom doorCol);
        _door = doorCol;
        if (m_Enemy.m_CurEnemyFSM._enemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            if (Mathf.Abs(m_Enemy.transform.position.x - m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x) <= 0.1f)
            {
                _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);
                m_Enemy.MoveNextPoint();
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
        
        other.TryGetComponent(out Door_Col_LayerRoom doorCol);
        _door = doorCol;
        if (m_Enemy.m_CurEnemyFSM._enemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            if (Mathf.Abs(m_Enemy.transform.position.x - m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x) <= 0.1f)
            {
                m_Enemy.bMoveToUsedDoor = true;
                if (m_Enemy.bMoveToUsedDoor)
                {
                    _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);
                    m_Enemy.MoveNextPoint();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        other.TryGetComponent(out Door_Col_LayerRoom doorCol);
        _door = doorCol;
        if (Mathf.Abs(m_Enemy.transform.position.y - _player.transform.position.y) <= 0.5f && _door)
        {
            m_Enemy.bMoveToUsedDoor = false;
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