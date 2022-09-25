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
    
    private List<GameObject> EnemyListFromSpawner;

    private int GetEnemyNumFromEnemyList;
    private int GetEnemyNumFromSpawnerList;

    private void Awake()
    {
        m_ObjProperty = UseableObjList.CHECKPOINT;

        _checkPoint = GetComponent<CheckPoint>();

        SetUpEnemyList();
        SetUpGetEnemyNum();
    }

    public void ActivateOutline(bool _isOn)
    {
        RemoveEnemyListToActivate();

        _checkPoint.ActivateBothOutline(_isOn);
    }

    public int useObj(IUseableObjParam _param)
    {
        // 성공하면 1, 실패하면 0을 반환
        // 플레이어가 체크포인트 범위안에 들어있고, 등록된 적 리스트가 비워져 있다면 인터랙션 가능
        if (_checkPoint.bCanInteract == true && GetEnemyNumFromEnemyList == 0 && GetEnemyNumFromSpawnerList == 0)
        {
            Debug.Log("Player Use The CheckPoint Box");
            _checkPoint.ActivateCheckPoint();
            return 1;
        }
        return 0;
    }

    void SetUpEnemyList()
    {
        // 스포너 적 리스트 초기화
        for (int i = 0; i < _checkPoint.EnemyListToActivateFromSpawner.Count; i++)
        {
            EnemyListFromSpawner = new List<GameObject>(_checkPoint.EnemyListToActivateFromSpawner[i].p_WillSpawnEnemys);
        }
    }

    void SetUpGetEnemyNum()
    {
        // 배치된 적 리스트가 존재 할 때
        if (_checkPoint.EnemyListToActivate.Count != 0)
            GetEnemyNumFromEnemyList = _checkPoint.EnemyListToActivate.Count;
        else
            GetEnemyNumFromEnemyList = 0;
        
        // 스포너 리스트가 존재 할 때
        if (_checkPoint.EnemyListToActivateFromSpawner.Count != 0)
            GetEnemyNumFromSpawnerList = EnemyListFromSpawner.Count;
        else
            GetEnemyNumFromSpawnerList = 0;
    }

    void RemoveEnemyListToActivate()
    {
        // 체크포인트 활성화 조건 버튼이 켜져있고 리스트에 "맵에 배치된" 적이 있을 때
        // 적 리스트 제거
        if (_checkPoint.bEnemyListToActivate == true && _checkPoint.EnemyListToActivate.Count != 0)
        {
            for (int i = 0; i < _checkPoint.EnemyListToActivate.Count; i++)
            {
                // 적이 죽으면
                if (_checkPoint.EnemyListToActivate[i].gameObject.activeSelf == false)
                {
                    // 리스트에서 죽은 적을 제거
                    _checkPoint.EnemyListToActivate.RemoveAt(i);
                    // 리스트에 있는 적 숫자 갱신
                    GetEnemyNumFromEnemyList = _checkPoint.EnemyListToActivate.Count;
                }
            }
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
            for (int i = 0; i < EnemyListFromSpawner.Count; i++)
            {
                // 적이 죽으면
                if (EnemyListFromSpawner[i].gameObject.activeSelf == false)
                {
                    // 리스트에서 죽은 적을 제거
                    EnemyListFromSpawner.RemoveAt(i);
                    // 리스트에 있는 적 숫자 갱신
                    GetEnemyNumFromSpawnerList = EnemyListFromSpawner.Count;
                }
            }
        }
        // 체크포인트 활성화 조건 버튼이 꺼져있고 "스포너 리스트"가 없다면
        else if (_checkPoint.bEnemyListToActivateFromSpawner == false && _checkPoint.EnemyListToActivateFromSpawner.Count == 0)
        {
            // 리스트 있는 적 숫자를 0으로 갱신
            GetEnemyNumFromSpawnerList = 0;
        }
    }
}