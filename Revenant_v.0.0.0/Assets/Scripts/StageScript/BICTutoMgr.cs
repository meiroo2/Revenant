using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BICTutoMgr : MonoBehaviour
{
    // Visible Member Variables
    public Image p_ScreenUIImgObj;
    public Sprite[] p_HoloUISprites;
    public Door_LayerRoom p_Room00Door;
    //public Animator p_Room01Cover_First;
    public Turret_Room1 p_Turret01;
    
    
    // Member Variables
    private bool m_PlayerCanControl = true;
    private Player m_Player;
    private Coroutine m_CurCoroutine;
    private Player_InputMgr m_InputMgr;
    private int m_Phase = 0;
    
    
    // Constructors
    public void Start()
    {
        var instanceMgr = InstanceMgr.GetInstance();
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_InputMgr = instanceMgr.GetComponentInChildren<Player_InputMgr>();

        p_ScreenUIImgObj.sprite = p_HoloUISprites[0];
        m_CurCoroutine = StartCoroutine(SetControlLockForSec());
    }
    
    // Updates
    private void Update()
    {
        switch (m_Phase)
        {
            case 0:     // B위치 가기전 첫 홀로
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    m_Phase = 1;
                }
                break;
            
            case 1:     // B위치 대기
                break;
            
            case 2:     // holo_Stair 출력
                p_ScreenUIImgObj.enabled = true;
                p_ScreenUIImgObj.sprite = p_HoloUISprites[1];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 3;
                break;
            
            case 3:     // 코루틴 돌면서 입력판정
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    m_Phase = 4;
                }
                break;
            
            case 4:     // C위치 충돌 대기
                break;
            
            case 5:     // C위치 충돌함
                p_ScreenUIImgObj.enabled = true;
                p_ScreenUIImgObj.sprite = p_HoloUISprites[2];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 6;
                break;
            
            case 6:     // int_holo 출력
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    m_Phase = 7;
                }
                break;
            
            case 7:     // 지하 1층 Trigger 대기
                break;
            
            case 8:     // 지하 1층 B위치 충돌, holo_alert 출력
                p_ScreenUIImgObj.enabled = true;
                p_ScreenUIImgObj.sprite = p_HoloUISprites[3];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 9;
                break;
            
            case 9:     // holo_alert 클릭 대기
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    m_Phase = 10;
                }
                break;
            
            case 10:    // holo_hide 클릭 대기
                p_ScreenUIImgObj.sprite = p_HoloUISprites[4];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 11;
                break;
            
            case 11:    // holo_hide 클릭 받기, 엄폐물 나타남
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    //p_Room01Cover_First.SetTrigger("Appear");
                    m_Phase = 12;
                }
                break;
            
            case 12:    // 엄폐물 숨을 때까지 대기
                if (m_Player.m_CurPlayerFSMName == PlayerStateName.HIDDEN)
                {
                    p_Turret01.SpawnTurret();
                    m_Phase = 13;
                }
                break;
            
            case 13:    // 사격 중
                break;
            
            case 14:    // 엄폐 사격 끝 + holo_roll 출력
                p_ScreenUIImgObj.enabled = true;
                p_ScreenUIImgObj.sprite = p_HoloUISprites[5];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 15;
                break;
            
            case 15:    // holo_roll 클릭 대기 + 터렛 재출력
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    p_Turret01.ReSpawnTurret();
                    m_Phase = 16;
                }
                break;
            
            case 16:    // roll 터렛 사격중
                break;
            
            case 17:    // holo_reload 출력
                p_ScreenUIImgObj.enabled = true;
                p_ScreenUIImgObj.sprite = p_HoloUISprites[6];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 18;
                break;
            
            case 18:    // holo_reload 클릭 대기
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    m_Phase = 19;
                }
                break;
            
            case 19:    // holo_shoot 출력
                p_ScreenUIImgObj.sprite = p_HoloUISprites[7];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 20;
                break;
            
            case 20:    // holo_shoot 클릭 대기
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    m_Phase = 21;
                }
                break;
            
            case 21:    // holo_weak 출력
                p_ScreenUIImgObj.sprite = p_HoloUISprites[8];
                StopCoroutine(m_CurCoroutine);
                m_CurCoroutine = StartCoroutine(SetControlLockForSec());
                m_Phase = 22;
                break;
            
            case 22:    // holo_weak 클릭 대기
                if (m_PlayerCanControl == false)
                    return;
                if (Input.anyKey)
                {
                    p_ScreenUIImgObj.enabled = false;
                    m_Phase = 23;
                }
                break;
            
            case 23:    // 모든 행동 종료
                break;
        }
    }

    // Functions
    private IEnumerator SetControlLockForSec()
    {
        m_InputMgr.SetAllInputLock(true);
        m_PlayerCanControl = false;
        yield return new WaitForSeconds(1f);
        m_InputMgr.SetAllInputLock(false);
        m_PlayerCanControl = true;
    }

    public void NextPhase()
    {
        m_Phase++;
    }
}