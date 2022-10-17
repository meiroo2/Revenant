using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public List<TutorialObject> tutorialObjects;

    public virtual void Initialize() { }

    public virtual bool CheckCondition()
    {
        // ����ó�� �߰��ϱ�
        return false;
    }
}
