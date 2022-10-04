using System;
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
        _player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            return;

        Debug.Log("OnTriggerEnter m_Enemy.bUsedDoor - " + m_Enemy.bMoveToUsedDoor);
        
        _door = col.GetComponent<Door_Col_LayerRoom>();
        if (m_Enemy.m_CurEnemyFSM._EnemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);

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
        if (m_Enemy.m_CurEnemyFSM._EnemyState == Enemy_FSM.EnemyState.Chase && m_Enemy.bMoveToUsedDoor && _door)
        {
            m_Enemy.bMoveToUsedDoor = true;
            if(m_Enemy.bMoveToUsedDoor)
                _door.m_Door.MoveToOtherSide(m_Enemy.transform, false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;
        
        _door = other.GetComponent<Door_Col_LayerRoom>();
        if (_door)
        {
            m_Enemy.bMoveToUsedDoor = false;
            Debug.Log("Exit bMoveToUseDoor - " + m_Enemy.bMoveToUsedDoor);
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