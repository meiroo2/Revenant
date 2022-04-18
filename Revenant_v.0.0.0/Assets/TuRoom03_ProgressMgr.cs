using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TuRoom03_ProgressMgr : ProgressMgr
{
    // Visible Member Variables
    public WorldUIMgr m_worldUIMgr;
    public TutoRoom03EnemyMgr m_EnemeyTutoMgr;

    public SpriteRenderer m_BlackBoxSpriteRenderer;

    private Transform m_FirstBulletTransform;

    private Player m_Player;

    // Member Variables
    public ScriptUIMgr m_ScriptUIMgr;

    private float Timer = 3f;
    private bool m_isTimerOn = false;

    private int m_isBulletTime = 0;

    // Constructors
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        NextProgress();
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            NextProgress();

        if (m_isTimerOn)
        {
            Timer -= Time.deltaTime;
            if(Timer <= 0f)
            {
                NextProgress();
                m_isTimerOn = false;
                Timer = 3f;
            }
        }

        switch (m_isBulletTime)
        {
            case 1:
                if (Vector2.Distance(m_Player.transform.position, m_FirstBulletTransform.position) < 0.3f)
                {
                    m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(true));
                    m_worldUIMgr.getWorldUI(1).PosSetIUI(new IUIParam(m_Player.transform));
                    m_BlackBoxSpriteRenderer.color = new Color(0, 0, 0, 1);
                    Time.timeScale = 0f;
                    m_isBulletTime++;
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    NextProgress();
                }
                break;
        }
    }

    // Physics


    // Functions
    public override void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
            case 0:
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(true));
                break;

            case 1:
                m_ScriptUIMgr.NextScript(0, true);
                // Turret Init -> Auto Sendmessage
                break;

            case 2:
                if (!m_isTimerOn)
                {
                    m_ScriptUIMgr.NextScript(0, true);
                    // Turret Fire Once
                    Debug.Log("Ÿ�ٴ�");
                    m_isTimerOn = true;
                }
                break;

            case 3:
                // ���� ���� �� FŰ UI
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 1));
                break;

            case 4:
                // �÷��̾ ������ ȣ����
                // �ͷ� ���� + ������ ����޽���
                break;

            case 5:
                // Script("���� ������")
                m_ScriptUIMgr.NextScript(0, true);
                break;

            case 6:
                // ���� ���� �ִ�
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 2));
                break;

            case 7:
                // Script("���ܱ��� ����")
                m_ScriptUIMgr.NextScript(0, true);
                m_isTimerOn = true;
                NextProgress();
                break;

            case 8:
                if (!m_isTimerOn)
                {
                    // �ͷ� ���
                    // Ʈ������ �޾ƿ�
                    m_isBulletTime = 1;
                    NextProgress();
                }
                break;

            case 9:
                
                break;

            case 10:
                Time.timeScale = 1f;
                m_BlackBoxSpriteRenderer.color = new Color(0, 0, 0, 0);
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(false));
                break;

            case 11:
                // �ͷ� ����

                break;

            case 12:
                SceneManager.LoadScene("T_Warden");
                break;
        }
    }

    private void InitProgress()
    {

    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}