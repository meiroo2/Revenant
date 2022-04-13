using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMgr : MonoBehaviour
{
    protected int m_ProgressValue = -1;
    public GameObject m_ProgressUI;
    public GameObject P_ProgressScriptUI;

    public IUI m_ProgressScriptUI;
    public IUI[] m_ProgressUIArr;

    protected bool[] m_ProgressCheck;

    protected void InitProgressMgr()
    {
        m_ProgressUIArr = m_ProgressUI.GetComponentsInChildren<IUI>();
        for(int i = 0; i < m_ProgressUIArr.Length; i++)
        {
            m_ProgressUIArr[i].ActivateUI(new IUIParam(0));
        }
    }

    public virtual void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
        }
    }
}