using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// BulletTime 클래스에 사격 정보를 예약시 쓰는 파라미터용 클래스입니다.
/// </summary>
public class BulletTimeParam
{
    public IHotBox m_HotBox { get; private set; }
    public IHotBoxParam m_HotBoxParam { get; private set; }
    public Vector2 m_MarkerPos { get; private set; }

    public Action m_FinalAction { get; private set; }

    public BulletTimeParam(IHotBox _box, IHotBoxParam _param, Vector2 _markerPos, Action _action)
    {
        m_HotBox = _box;
        m_HotBoxParam = _param;
        m_MarkerPos = _markerPos;
        m_FinalAction = _action;
    }
}

public class BulletTimeMgr : MonoBehaviour
{
    // Visible Member Variables
    public float p_BulletTimeLimit = 2f;
    public float p_ShotDelayTime = 0.1f;
    public GameObject p_Marker;
    
    // Member Variables
    private RageGauge_UI m_RageGaugeUI;
    public bool m_CanUseBulletTime { get; private set; } = false;

    private const int m_NumofMarker = 8;

    private Player m_Player;
    private BasicWeapon m_Negotiator;
    private Player_InputMgr m_InputMgr;
    
    private HitSFXMaker m_HitSFXMaker;
    private SoundPlayer m_SoundPlayer;
    private MatChanger m_MatChanger;

    private List<GameObject> m_MarkerList = new List<GameObject>();
    private int m_MarkerIdx = 0;

    [ShowInInspector, ReadOnly]
    private List<BulletTimeParam> m_BulletTimeParamList = new List<BulletTimeParam>();
    private Coroutine m_Coroutine;
    
    private List<Action> m_FinaleActionList = new List<Action>();

    public bool m_BulletTimeActivating { get; private set; } = false;


    // Constructors
    private void Awake()
    {
        m_MarkerIdx = 0;
    }

    private void Start()
    {
        // Awake시 마커용 오브젝트풀 생성
        for (int i = 0; i < m_NumofMarker - 1; i++)
        {
            var element = Instantiate(p_Marker, this.transform, true);
            m_MarkerList.Add(element);
            element.SetActive(false);
        }
        
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SoundPlayer = instance.GetComponentInChildren<SoundPlayer>();
        m_InputMgr = instance.GetComponentInChildren<Player_InputMgr>();
        m_MatChanger = instance.GetComponentInChildren<MatChanger>();
        m_Negotiator = instance.GetComponentInChildren<Player_Manager>().m_Player.m_WeaponMgr.m_CurWeapon;
        m_RageGaugeUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
    }


    // Updates


    // Functions

    private IEnumerator CheckBulletTimeLimit()
    {
        float Timer = 0f;

        while (true)
        {
            Timer += Time.unscaledDeltaTime;

            m_RageGaugeUI.GetTimePassed(1f - (Timer / p_BulletTimeLimit));
            
            if (Timer > p_BulletTimeLimit)
            {
                break;
            }
            yield return null;
        }
        
        if (m_BulletTimeActivating)
        {
            ActivateBulletTime(false);
            FireAll();
        }

        yield break;
    }
    
    /// <summary>
    /// 전탄 발사가 끝난 후 호출될 함수를 추가합니다.
    /// </summary>
    /// <param name="_action">원하는 함수</param>
    public void AddFinaleAction(Action _action)
    {
        m_FinaleActionList.Add(_action);
    }

    
    /// <summary>
    /// BulletTimeMgr의 BulletTime 사용을 가능하게 변경합니다.
    /// </summary>
    public void SetCanUseBulletTime()
    {
        if (m_CanUseBulletTime)
            return;
        
        m_CanUseBulletTime = true;
        StartCoroutine(CheckBulletTimeStart());
    }

    /// <summary>
    /// BulletTime 사용이 가능하게 바뀌었을 때, 입력 타이밍을 체크합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckBulletTimeStart()
    {
        while (true)
        {
            if (m_InputMgr.m_IsPushBulletTimeKey)
            {
                if (m_Negotiator.m_LeftRounds > 0 && (m_Player.m_CurPlayerFSMName == PlayerStateName.IDLE ||
                                                      m_Player.m_CurPlayerFSMName == PlayerStateName.WALK))
                {
                    break;
                }
                else
                {
                    m_RageGaugeUI.CanConsume(9999999f);
                }
            }
            yield return null;
        }
        
        Debug.Log("불릿타임 시작");
        ActivateBulletTime(true);

        while (true)
        {
            yield return null;


            if (m_Negotiator.m_LeftRounds == 0 || m_InputMgr.m_IsPushBulletTimeKey)
                break;
            
      
        }
        Debug.Log("불릿타임 끝");
        
        ActivateBulletTime(false);
        FireAll();
    }
    
    /// <summary>
    /// BulletTime을 시작합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_isStart">시작/중지 여부</param>
    private void ActivateBulletTime(bool _isStart)
    {
        if (_isStart)
        {
            m_Player.m_PlayerAniMgr.SetVisualParts(true, false, false, false);
            m_Player.m_PlayerAniMgr.p_FullBody.m_Animator.SetInteger("BulletTime", 1);
            m_RageGaugeUI.p_BulletTimeIndicator.enabled = false;
            m_RageGaugeUI.GaugeToBulletTime(true);
            m_RageGaugeUI.TempStopRageGauge(true);
            
            StartCoroutine(CheckBulletTimeLimit());
            m_BulletTimeActivating = true;
            m_MatChanger.ChangeMat();
            Time.timeScale = 0f;
        }
        else
        {
            m_RageGaugeUI.GaugeToBulletTime(false);
            m_RageGaugeUI.TempStopRageGauge(false);
            
            m_BulletTimeActivating = false;
            m_MatChanger.ResotreMat();
            Time.timeScale = 1f;

            foreach (var element in m_MarkerList)
            {
                element.transform.parent = this.transform;
                element.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// 오직 시간만을 잠시 수정합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_time">지정 시간</param>
    public void ModifyTimeScale(float _time)
    {
        m_MatChanger.ChangeMat();
        Time.timeScale = 0.3f;
        StartCoroutine(CheckTimePassed(_time));
    }
    
    /// <summary>
    /// 파라미터로 들어온 사격 정보를 예약합니다. 최대 8발까지만 예약 가능
    /// </summary>
    /// <param name="_param">사격 정보</param>
    public void BookFire(BulletTimeParam _param)
    {
        if (m_MarkerIdx > m_MarkerList.Count - 1)
            return;
        
        m_BulletTimeParamList.Add(_param);
        //Debug.Log("추가요" + m_BulletTimeParamList.Count);
        
        var marker = m_MarkerList[m_MarkerIdx];
        marker.SetActive(true);
        marker.transform.position = _param.m_MarkerPos;
        marker.transform.parent = _param.m_HotBox.m_ParentObj.transform;

        m_MarkerIdx++;
    }

    /// <summary>
    /// 빈 곳을 조준했을 때 사용하는 BookFire입니다.
    /// </summary>
    /// <param name="_markerPos"></param>
    /// <param name="_action"></param>
    public void BookFire(Vector2 _markerPos, Action _action)
    {
        if (m_MarkerIdx > m_MarkerList.Count - 1)
            return;
        
        m_BulletTimeParamList.Add(new BulletTimeParam(null, null, _markerPos, _action));
        
        var marker = m_MarkerList[m_MarkerIdx];
        marker.SetActive(true);
        marker.transform.position = _markerPos;

        m_MarkerIdx++;
    }

    /// <summary>
    /// 예약된 모든 사격 정보를 기반으로 발사합니다.
    /// </summary>
    private void FireAll()
    {
        StartCoroutine(FireCoroutine());
    }

    /// <summary>
    /// FireAll() 함수에서 사용하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireCoroutine()
    {
        for (int i = 0; i < m_BulletTimeParamList.Count; i++)
        {
            if (ReferenceEquals(m_BulletTimeParamList[i].m_HotBox, null))
            {
                m_MarkerList[i].SetActive(false);
                m_SoundPlayer.playGunFireSound(0, gameObject);
                m_BulletTimeParamList[i].m_FinalAction?.Invoke();
            }
            else
            {
                m_BulletTimeParamList[i].m_HotBox.HitHotBox(m_BulletTimeParamList[i].m_HotBoxParam);
                m_MarkerList[i].SetActive(false);
            
                m_SoundPlayer.playGunFireSound(0, gameObject);
                m_BulletTimeParamList[i].m_FinalAction?.Invoke();
            }
            yield return new WaitForSecondsRealtime(p_ShotDelayTime);
        }

        // FinaleAction
        foreach (var element in m_FinaleActionList)
        {
            element.Invoke();
        }

        // 초기화
        m_CanUseBulletTime = false;
        m_MarkerIdx = 0;
        m_FinaleActionList.Clear();
        m_FinaleActionList.TrimExcess();
        m_BulletTimeParamList.Clear();
        m_BulletTimeParamList.TrimExcess();
        
        yield break;
    }

    /// <summary>
    /// 지정된 UnscaledTime이 지난 후 Timescale을 1로 되돌립니다.
    /// </summary>
    /// <param name="_time">원하는 시간</param>
    /// <returns></returns>
    private IEnumerator CheckTimePassed(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1f;
        m_MatChanger.ResotreMat();

        yield break;
    }
}