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
    private List<ISpriteMatChange> m_EnemyMatList = new List<ISpriteMatChange>();
    private List<ISpriteMatChange> m_BGMatList = new List<ISpriteMatChange>();
    

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

        m_EnemyMatList.Clear();
        m_EnemyMatList.TrimExcess();
        m_BGMatList.Clear();
        m_BGMatList.TrimExcess();
        
        // For Enemy
        var basicEnemyList = StaticMethods.FindAllObjects<BasicEnemy>();
        foreach (var VARIABLE in basicEnemyList)
        {
            if (VARIABLE.TryGetComponent(out ISpriteMatChange SMC))
            {
                m_EnemyMatList.Add(SMC);
            }
        }

        var bgList = GameObject.FindGameObjectsWithTag("BackGround");
        foreach (var VARIABLE in bgList)
        {
            if (VARIABLE.TryGetComponent(out ISpriteMatChange SMC))
            {
                m_BGMatList.Add(SMC);
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
                foreach (var VARIABLE in m_EnemyMatList)
                {
                    VARIABLE.ChangeMat(_spMatType);
                }
                break;
            
            case SpriteType.BACKGROUND:
                foreach (var VARIABLE in m_BGMatList)
                {
                    VARIABLE.ChangeMat(_spMatType);
                }
                break;
        }
    }
}