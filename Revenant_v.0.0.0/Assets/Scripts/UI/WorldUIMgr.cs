using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject[] P_WorldUIObjs;

    // Member Variables
    private WorldUI[] m_WorldIUIs;

    // Constructors
    private void Awake()
    {
        m_WorldIUIs = new WorldUI[P_WorldUIObjs.Length];
        for(int i = 0; i < P_WorldUIObjs.Length; i++)
        {
            m_WorldIUIs[i] = P_WorldUIObjs[i].GetComponent<WorldUI>();
        }
    }

    // Updates


    // Physics


    // Functions
    public WorldUI getWorldUI(int _idx)
    {
        if (_idx >= 0 && _idx < m_WorldIUIs.Length)
        {
            return m_WorldIUIs[_idx];
        }
        else
        {
            Debug.Log("WorldUIMgr���� Array ũ�� ���");
            return null;
        }
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
