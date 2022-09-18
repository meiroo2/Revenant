using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CheckPoint_Col : MonoBehaviour, IUseableObj
{
    private CheckPoint _checkPoint;
    
    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }

    private int NumOfEnemies;

    private void Awake()
    {
        m_ObjProperty = UseableObjList.CHECKPOINT;
        
        _checkPoint = GetComponent<CheckPoint>();
        NumOfEnemies = _checkPoint.EnemyListToActivateCheckPoint.Count;
    }

    public void ActivateOutline(bool _isOn)
    {
        _checkPoint.ActivateBothOutline(_isOn);
    }
    
    // 오브젝트 사용. 성공하면 1, 실패하면 0을 반환
    public int useObj(IUseableObjParam _param)
    {
        for (int i = 0; i < _checkPoint.EnemyListToActivateCheckPoint.Count; i++)
        {
            if (_checkPoint.EnemyListToActivateCheckPoint.Contains(_checkPoint.EnemyListToActivateCheckPoint[i]))
            {
                if (_checkPoint.EnemyListToActivateCheckPoint[i].gameObject.activeSelf == false)
                {
                    Debug.Log(_checkPoint.EnemyListToActivateCheckPoint[i].gameObject);
                }
            }
            if (i == _checkPoint.EnemyListToActivateCheckPoint.Count - 1 && _checkPoint.EnemyListToActivateCheckPoint[i].activeSelf == false)
            {
                //Debug.Log(i);
                Debug.Log("마지막 적 죽음");
                
                // If player interacted Checkpoint, activate it
                if (_checkPoint.bCanInteract == true)
                {
                    Debug.Log("Player Use The CheckPoint Box");
                    _checkPoint.ActivateCheckPoint();
                    return 1;
                }
            }
        }
        return 0;
    }
}
