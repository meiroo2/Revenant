using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TuRoom03_ProgressMgr : ProgressMgr
{
    /*
    
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
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
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
                m_FirstBulletTransform = m_EnemeyTutoMgr.GetFirstBulletPos();
                if (Vector2.Distance(m_Player.transform.position, m_FirstBulletTransform.position) < 0.3f)
                {
                    m_worldUIMgr.getWorldUI(2).ActivateIUI(new IUIParam(true));
                    m_worldUIMgr.getWorldUI(2).PosSetIUI(new IUIParam(m_Player.transform));
                    m_BlackBoxSpriteRenderer.color = new Color(0, 0, 0, 1);
                    Time.timeScale = 0f;
                    m_isBulletTime++;
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    NextProgress();
                    Time.timeScale = 1f;
                    m_Player.GetComponentInChildren<Player_HotBox>().m_isEnemys = true;
                }
                break;
        }

        if(m_ProgressValue == 3)
        {
            if (m_Player.m_curPlayerState == playerState.HIDDEN)
                NextProgress();
        }

        if(m_ProgressValue == 5)
        {
            if (m_Player.m_curPlayerState != playerState.HIDDEN)
                NextProgress();
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
                m_EnemeyTutoMgr.TurretToggle(true);
                // Turret Init -> Auto Sendmessage
                break;

            case 2:
                if (!m_isTimerOn)
                {
                    m_ScriptUIMgr.NextScript(0, true);
                    m_EnemeyTutoMgr.TurretFire_1();
                    Debug.Log("타다당");
                    m_isTimerOn = true;
                }
                break;

            case 3:
                // 엄폐물 등장 및 F키 UI
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(true));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 1));
                break;

            case 4:
                // 플레이어가 숨으면 호출함
                // 터렛 난사 + 끝나면 샌드메시지
                m_EnemeyTutoMgr.TurretFire_3();
                break;

            case 5:
                // Script("엄폐 해제해")
                m_ScriptUIMgr.NextScript(0, true);
                break;

            case 6:
                // 엄폐물 퇴장 애니
                m_worldUIMgr.getWorldUI(0).ActivateIUI(new IUIParam(false));
                m_worldUIMgr.getWorldUI(1).AniSetIUI(new IUIParam("isOn", 2));
                NextProgress();
                break;

            case 7:
                // Script("공겨그이 허점")
                m_ScriptUIMgr.NextScript(0, true);
                m_isTimerOn = true;
                NextProgress();
                break;

            case 8:
                // 터렛 사격
                m_isBulletTime = 1;
                m_FirstBulletTransform = m_EnemeyTutoMgr.GetFirstBulletPos();
                break;

            case 9:
                m_EnemeyTutoMgr.TurretFire_3();
                break;

            case 10:
                Time.timeScale = 1f;
                m_BlackBoxSpriteRenderer.color = new Color(0, 0, 0, 0);
                m_worldUIMgr.getWorldUI(1).ActivateIUI(new IUIParam(false));
                break;

            case 11:
                // 터렛 퇴장
                m_EnemeyTutoMgr.TurretExit();
                break;

            case 12:
                m_ScriptUIMgr.NextScript(0, true);
                Invoke("NextProgress", 6f);
                break;

            case 13:
                SceneManager.LoadScene("T_Warden");
                break;
        }
    }

    private void InitProgress()
    {

    }

    // 기타 분류하고 싶은 것이 있을 경우
    
    */
}