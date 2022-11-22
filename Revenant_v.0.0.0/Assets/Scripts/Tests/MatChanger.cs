using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;



public class MatChanger : MonoBehaviour
{
    // Member Variables
    private Dictionary<GameObject, ISpriteMatChange> m_EnemyMatDic = 
        new Dictionary<GameObject, ISpriteMatChange>();

    private Dictionary<GameObject, ISpriteMatChange> m_BGMatDic =
        new Dictionary<GameObject, ISpriteMatChange>();


    // Constructor
    private void Awake()
    {
        Debug.Log("MatChanger Awake");
        InitMatChanger();
    }

    // Updates
  

    // Functions

    public void InitMatChanger()
    {
        Debug.Log("MatChanger OnSceneLoaded");

        m_EnemyMatDic.Clear();
        m_EnemyMatDic.TrimExcess();
        m_BGMatDic.Clear();
        m_BGMatDic.TrimExcess();
        
        // For Enemy
        var basicEnemyList = StaticMethods.FindAllObjects<BasicEnemy>();
        foreach (var VARIABLE in basicEnemyList)
        {
            if (VARIABLE.TryGetComponent(out ISpriteMatChange SMC))
            {
               m_EnemyMatDic.Add(VARIABLE.gameObject, SMC);
            }
        }

        var bgList = GameObject.FindGameObjectsWithTag("BackGround");
        foreach (var VARIABLE in bgList)
        {
            if (VARIABLE.TryGetComponent(out ISpriteMatChange SMC))
            {
               m_BGMatDic.Add(VARIABLE, SMC);
            }
        }
    }

    /// <summary>
    /// 멤버변수로 가지고 있는 ISpriteMatChange 스크립트를 기반으로 ChangeMat 함수를 호출합니다.
    /// SpriteType에 일치하는 모든 Material에 영향을 줍니다.
    /// </summary>
    /// <param name="_spType"></param>
    /// <param name="_spMatType"></param>
    [Button(ButtonSizes.Medium)]
    public void ChangeMat(SpriteType _spType, SpriteMatType _spMatType)
    {
        switch (_spType)
        {
            case SpriteType.ENEMY:
                foreach (var VARIABLE in m_EnemyMatDic)
                {
                    if(!VARIABLE.Key)
                        continue;
                    
                    if(!VARIABLE.Key.activeSelf)
                        continue;

                    VARIABLE.Value.ChangeMat(_spMatType);
                }
                break;
            
            case SpriteType.BACKGROUND:
                foreach (var VARIABLE in m_BGMatDic)
                {
                    if(!VARIABLE.Key)
                        continue;
                    
                    if(!VARIABLE.Key.activeSelf)
                        continue;
                    
                    VARIABLE.Value.ChangeMat(_spMatType);
                }
                break;
        }
    }
}