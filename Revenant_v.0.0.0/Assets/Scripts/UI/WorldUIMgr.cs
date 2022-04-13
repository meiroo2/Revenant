using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject[] P_WorldUIObjs;

    // Member Variables
    private IUI[] m_WorldIUIs;

    // Constructors
    private void Awake()
    {
        m_WorldIUIs = new IUI[P_WorldUIObjs.Length];
        for(int i = 0; i < P_WorldUIObjs.Length; i++)
        {
            m_WorldIUIs[i] = P_WorldUIObjs[i].GetComponent<IUI>();
        }
    }

    // Updates


    // Physics


    // Functions
    public int SetWorldUI(int _idx, IUIParam _param)
    {
        if (_idx >= 0 && _idx < m_WorldIUIs.Length)
        {
            m_WorldIUIs[_idx].ActivateUI(_param);
            return 1;
        }
        else
        {
            Debug.Log("WorldUIMgr���� Array ũ�� ���");
            return 0;
        }
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
