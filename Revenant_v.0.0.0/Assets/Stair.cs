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
                Debug.Log("Stair���� 1 ����");
                m_OnStairObjCount[i]++;
                if(m_OnStairObjCount[i] == 1)
                {
                    Debug.Log("Stair���� ������");
                    m_OnStairObjCount.RemoveAt(i);
                    m_OnStairObjNum.RemoveAt(i);
                    return 0;
                }
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            Debug.Log("Stair���� ó�� �߰�");
            m_OnStairObjNum.Add(_ObjID);
            m_OnStairObjCount.Add(0);
        }
        return 1;
    }
}