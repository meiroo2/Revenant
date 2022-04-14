using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuRoom03_ProgressMgr : ProgressMgr
{
    // Visible Member Variables
    public WorldUIMgr m_worldUIMgr;

    public Transform m_TargetTransform;

    // Member Variables
    public ScriptUIMgr m_ScriptUIMgr;

    // Constructors
    private void Start()
    {
        NextProgress();
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            NextProgress();
    }

    // Physics


    // Functions
    public override void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
            case 0: 
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                break;
        }
    }

    private void InitProgress()
    {

    }

    // 기타 분류하고 싶은 것이 있을 경우
}