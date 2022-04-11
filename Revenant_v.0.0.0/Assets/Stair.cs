using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public List<int> m_OnStairObjNum = new List<int>();
    public List<int> m_OnStairObjCount = new List<int>();

    public int isOnStair(int _ObjID)
    {
        bool isFind = false;

        for (int i = 0; i < m_OnStairObjNum.Count; i++)
        {
            if(m_OnStairObjNum[i] == _ObjID)
            {
                m_OnStairObjCount[i]++;
                if(m_OnStairObjCount[i] == 2)
                {
                    m_OnStairObjCount.RemoveAt(i);
                    m_OnStairObjNum.RemoveAt(i);
                    return 1;
                }
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            m_OnStairObjNum.Add(_ObjID);
            m_OnStairObjCount.Add(0);
        }
        return 0;
    }
}