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
        // ����ó�� �߰��ϱ�
        return m_isClear;
    }

    public void SetClear()
    {
        Debug.Log("Ŭ����");
        m_isClear = true;
    }
}
