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

    public GameObject m_ProgressUI;
    public GameObject P_ProgressScriptUI;

    public IUI m_ProgressScriptUI;
    public IUI[] m_ProgressUIArr;
    protected bool[] m_ProgressCheck;

    protected void InitProgressMgr()
    {
        m_ProgressUIArr = m_ProgressUI.GetComponentsInChildren<IUI>();
        for (int i = 0; i < m_ProgressUIArr.Length; i++)
        {
            m_ProgressUIArr[i].ActivateUI(new IUIParam(false));
        }
    }

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
            m_ProgressScriptUI.ActivateUI(new IUIParam(true));
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
                        m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(false));
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
                        m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(false));
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
                m_ProgressScriptUI.ActivateUI(new IUIParam(true));
                m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(true));
                break;
            case 1:
                m_ProgressUIArr[m_ProgressValue].ActivateUI(new IUIParam(true));
                break;
            case 2:
                break;
            case 3:
                m_ProgressUIArr[2].ActivateUI(new IUIParam(true));
                break;
            case 4:
                m_ProgressUIArr[2].ActivateUI(new IUIParam(false));
                m_CenterDoor.GetComponent<IDirect>().NextDirect();
                break;
            case 5:
                m_Stair.gameObject.SetActive(true);
                m_ProgressUIArr[3].ActivateUI(new IUIParam(true));
                break;
            case 6:
                m_ProgressScriptUI.ActivateUI(new IUIParam(true));

                m_ProgressUIArr[3].ActivateUI(new IUIParam(true));
                m_Stair.SetBool("isPush", true);
                m_ProgressUIArr[4].ActivateUI(new IUIParam(true));
                m_ProgressUIArr[5].ActivateUI(new IUIParam(true));
                m_ProgressUIArr[6].ActivateUI(new IUIParam(true));
                m_PhysicsStair.SetActive(true);
                break;
        }
    }
}