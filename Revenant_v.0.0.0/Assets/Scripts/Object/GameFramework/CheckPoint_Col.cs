using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint_Col : MonoBehaviour, IUseableObj
{
    private CheckPoint _checkPoint;

    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }

    private int GetEnemyNumFromEnemyList;
    private int GetEnemyNumFromSpawnerList;

    private BasicEnemy EnemyListToActivateFromSpawner_Cast_BasicEnemy;

    private void Awake()
    {
        m_ObjProperty = UseableObjList.CHECKPOINT;

        _checkPoint = GetComponent<CheckPoint>();
    }

    private void Start()
    {
        SetUpGetEnemyNum();
    }
    
    public int useObj(IUseableObjParam _param)
    {
        _checkPoint.ActivateBothOutline(true);
        int sum = GetEnemyNumFromEnemyList + GetEnemyNumFromSpawnerList;
        for (int i = 0; i < sum; i++)
        {
            RemoveEnemyListToActivate();
        }

        // 성공하면 1, 실패하면 0을 반환
        // 플레이어가 체크포인트 범위안에 들어있고, 등록된 적 리스트가 비워져 있다면 인터랙션 가능
        if (_checkPoint.bCanInteract && GetEnemyNumFromEnemyList == 0 && GetEnemyNumFromSpawnerList == 0)
        {
            // 체크포인트 활성화
            _checkPoint.ActivateCheckPoint();
            // 체크포인트 번호 저장
            DataHandleManager dataMgr = GameMgr.GetInstance().p_DataHandleMgr;
            dataMgr.CheckPointSectionNumber = _checkPoint.SectionNumber;

            // 저장된 체크포인트가 활성화되어 있으면
            if (CheckPoint.CheckPointsList[_checkPoint.SectionNumber - 1].bActivated)
            {
                // 저장된 체크포인트의 활성화 여부를 DataHandleManager에 저장
                dataMgr.IsCheckPointActivated = CheckPoint.CheckPointsList[_checkPoint.SectionNumber - 1].bActivated;
                // 플레이어가 스폰할 체크포인트의 위치를 DataHandleManager에 저장
                dataMgr.PlayerPositionVector = CheckPoint.GetActiveCheckPointPosition();
            }
            Debug.Log("체크포인트 상호작용 성공");
            return 1;
        }
        
        Debug.Log("체크포인트 상호작용 실패");
        return 0;
    }

    /** 리스트에 적이 있는지 판별 후 초기화하는 함수 */
    void SetUpGetEnemyNum()
    {
        // 배치된 적 리스트가 존재 할 때
        if (_checkPoint.EnemyListToActivate.Count != 0)
            GetEnemyNumFromEnemyList = _checkPoint.EnemyListToActivate.Count;
        else
            GetEnemyNumFromEnemyList = 0;
        
        // 스포너 리스트가 존재 할 때
        if (_checkPoint.EnemyListToActivateFromSpawner.Count != 0)
            GetEnemyNumFromSpawnerList = _checkPoint.EnemyListFromSpawner.Count;
        else
            GetEnemyNumFromSpawnerList = 0;

        foreach (var enemyListFromSpawner in _checkPoint.EnemyListFromSpawner)
        {
            EnemyListToActivateFromSpawner_Cast_BasicEnemy = enemyListFromSpawner.GetComponent<BasicEnemy>();
        }
    }

    /** 적을 리스트에서 지우는 함수 */
    void RemoveEnemyListToActivate()
    {
        if (_checkPoint == null && _checkPoint.EnemyListToActivate == null && _checkPoint.EnemyListToActivate[0].gameObject == null)
            return;
        
        // 체크포인트 활성화 조건 버튼이 켜져있고 리스트에 "맵에 배치된" 적이 있을 때
        // 적 리스트 제거
        if (_checkPoint.bEnemyListToActivate == true && _checkPoint.EnemyListToActivate.Count != 0)
        {
            for (int i = 0; i < GetEnemyNumFromEnemyList; i++)
            {
                // 적이 죽으면
                if (_checkPoint.EnemyListToActivate.Count == GetEnemyNumFromEnemyList && _checkPoint.EnemyListToActivate[i].m_IsDead == true)
                {
                    // 리스트에서 죽은 적을 제거
                    //Debug.Log(_checkPoint.EnemyListToActivate[i].gameObject.name);
                    _checkPoint.EnemyListToActivate.RemoveAt(i);
                }
            }
            // 리스트에 있는 적 숫자 갱신
            GetEnemyNumFromEnemyList = _checkPoint.EnemyListToActivate.Count;
        }
        // 체크포인트 활성화 조건 버튼이 꺼져있고 "맵에 배치된 적 리스트"가 없다면
        else if (_checkPoint.bEnemyListToActivate == false && _checkPoint.EnemyListToActivate.Count == 0)
        {
            // 리스트 있는 적 숫자를 0으로 갱신
            GetEnemyNumFromEnemyList = 0;
        }
        
        // 체크포인트 활성화 조건 버튼이 켜져있고 리스트에 "스포너"가 있을 때
        // 스포너 내 적 리스트 제거
        if (_checkPoint.bEnemyListToActivateFromSpawner == true && _checkPoint.EnemyListToActivateFromSpawner.Count != 0)
        {
            for (int i = 0; i < GetEnemyNumFromSpawnerList; i++)
            {
                // 적이 죽으면
                if (_checkPoint.EnemyListFromSpawner.Count == GetEnemyNumFromSpawnerList && EnemyListToActivateFromSpawner_Cast_BasicEnemy.m_IsDead)
                {
                    // 리스트에서 죽은 적을 제거
                    _checkPoint.EnemyListFromSpawner.RemoveAt(i);
                }
            }
            // 리스트에 있는 적 숫자 갱신
            GetEnemyNumFromSpawnerList = _checkPoint.EnemyListFromSpawner.Count;
        }
        // 체크포인트 활성화 조건 버튼이 꺼져있고 "스포너 리스트"가 없다면
        else if (_checkPoint.bEnemyListToActivateFromSpawner == false && _checkPoint.EnemyListToActivateFromSpawner.Count == 0)
        {
            // 리스트 있는 적 숫자를 0으로 갱신
            GetEnemyNumFromSpawnerList = 0;
        }
    }
}