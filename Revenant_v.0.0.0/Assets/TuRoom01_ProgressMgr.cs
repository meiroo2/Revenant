using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuRoom01_ProgressMgr : ProgressMgr
{
    private bool m_isPushA = false;
    private bool m_isPushD = false;
    private float m_KeyPushTimer = 2f;
    public GameObject m_CenterDoor;
    public Animator m_Stair;
    public GameObject m_PhysicsStair;


    private void Start()
    {
        m_ProgressScriptUI = P_ProgressScriptUI.GetComponent<IUI>();

        m_ProgressCheck = new bool[10];
        InitProgressMgr();

        m_ProgressValue = -1;
        NextProgress();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            m_isPushA = true;
        else if (Input.GetKeyUp(KeyCode.A))
            m_isPushA = false;

        if (Input.GetKeyDown(KeyCode.D))
            m_isPushD = true;
        else if (Input.GetKeyUp(KeyCode.D))
            m_isPushD = false;

        if (Input.GetKeyDown(KeyCode.I))
        {
            m_ProgressScriptUI.ActivateUI(new IUIParam(1));
        }
    }
    private void FixedUpdate()
    {
        switch (m_ProgressValue)
        {
            case 0:
                if (m_isPushA)
                {
                    m_KeyPushTimer -= Time.deltaTime;
                    if (m_KeyPushTimer <= 0f)
                    {
                        m_KeyPushTimer = 2f;
                        m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(0));
                        NextProgress();
                    }
                }
                else
                    m_KeyPushTimer = 2f;
                break;
            case 1:
                if (m_isPushD)
                {
                    m_KeyPushTimer -= Time.deltaTime;
                    if (m_KeyPushTimer <= 0f)
                    {
                        m_KeyPushTimer = 2f;
                        m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(0));
                        NextProgress();
                    }
                }
                else
                    m_KeyPushTimer = 2f;
                break;
        }
    }
    public override void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
            case 0:
                m_ProgressScriptUI.ActivateUI(new IUIParam(1));
                m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(1));
                break;
            case 1:
                m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(1));
                break;
            case 2:
                break;
            case 3:
                m_ProgressUIArr[2].ActivateUI(new IUIParam(1));
                break;
            case 4:
                m_ProgressUIArr[2].ActivateUI(new IUIParam(0));
                m_CenterDoor.GetComponent<IDirect>().NextDirect();
                break;
            case 5:
                m_Stair.gameObject.SetActive(true);
                m_ProgressUIArr[3].ActivateUI(new IUIParam(1));
                break;
            case 6:
                m_ProgressScriptUI.ActivateUI(new IUIParam(1));

                m_ProgressUIArr[3].ActivateUI(new IUIParam(0));
                m_Stair.SetBool("isPush", true);
                m_ProgressUIArr[4].ActivateUI(new IUIParam(1));
                m_ProgressUIArr[5].ActivateUI(new IUIParam(1));
                m_ProgressUIArr[6].ActivateUI(new IUIParam(1));
                m_PhysicsStair.SetActive(true);
                break;
        }
    }
}