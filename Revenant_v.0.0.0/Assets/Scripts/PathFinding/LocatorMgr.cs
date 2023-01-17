using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class LocatorMgr : MonoBehaviour
{
    private List<LocatorCol> m_LocatorColList = new List<LocatorCol>();

    private void Awake()
    {
        var locatorArr = GameObject.FindObjectsOfType<LocatorCol>();
        foreach (var VARIABLE in locatorArr)
        {
            m_LocatorColList.Add(VARIABLE.GetComponent<LocatorCol>());
        }

        foreach (var VARIABLE in m_LocatorColList)
        {
            VARIABLE.m_LocatorNum = Convert.ToInt32(VARIABLE.gameObject.name.Split("_")[1]);
        }
    }
}
