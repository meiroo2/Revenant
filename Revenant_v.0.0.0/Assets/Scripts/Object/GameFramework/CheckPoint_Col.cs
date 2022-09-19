using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_Col : MonoBehaviour, IUseableObj
{
    private CheckPoint _checkPoint;

    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }

    private void Awake()
    {
        m_ObjProperty = UseableObjList.CHECKPOINT;
        
        _checkPoint = GetComponent<CheckPoint>();
    }

    public void ActivateOutline(bool _isOn)
    {
        if (_checkPoint.EnemyListToActivate.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < _checkPoint.EnemyListToActivate.Count; i++)
            {
                if (_checkPoint.EnemyListToActivate[i].activeSelf == false)
                {
                    _checkPoint.EnemyListToActivate.RemoveAt(i);
                }
            }
        }
        _checkPoint.ActivateBothOutline(_isOn);
    }
    
    public int useObj(IUseableObjParam _param)
    {
        // 성공하면 1, 실패하면 0을 반환
        // If player interacted Checkpoint, activate it
        if (_checkPoint.bCanInteract == true && _checkPoint.EnemyListToActivate.Count == 0)
        {
            Debug.Log("Player Use The CheckPoint Box");
            _checkPoint.ActivateCheckPoint();
            return 1;
        }
        return 0;
    }
}
