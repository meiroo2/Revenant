using System;
using System.Collections.Generic;
using UnityEngine;





public class SmartMelee_UseRange : MonoBehaviour
{
    public Dictionary<Collider2D, IUseableObj> m_CurObjDic { get; private set; } = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IUseableObj useableObj))
        {
            m_CurObjDic.Add(other, useableObj);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_CurObjDic.ContainsKey(other))
        {
            m_CurObjDic.Remove(other);
        }
    }

    public bool CanInteract()
    {
        return m_CurObjDic.Count > 0;
    }

    public IUseableObj GetUseableObjbyIDX(int _inputIdx)
    {
        int idx = 0;
        foreach (var VARIABLE in m_CurObjDic.Values)
        {
            if (idx == _inputIdx)
            {
                return VARIABLE;
            }
            else
            {
                idx++;
            }
        }

        return null;
    }
}