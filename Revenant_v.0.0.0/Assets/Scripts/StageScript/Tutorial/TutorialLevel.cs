using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    //public TutorialCondition p_Condition;
    protected bool m_isClear = false;
    public List<TutorialObject> tutorialObjects;

    public bool CheckCondition()
    {
        // 예외처리 추가하기
        return m_isClear;
    }

    public void SetClear()
    {
        Debug.Log("클리어");
        m_isClear = true;
    }
}
