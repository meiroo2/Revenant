using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float p_BeforeBTEffectDelay = 0.5f;
    public float p_ThunderEffectDelay = 0.8f;

    public GameObject p_Marker;
    
    
    // Member Variables
    private RageGauge m_RageGauge;
    public bool m_IsGaugeFull { get; private set; } = false;

    private const int m_NumofMarker = 8;

    private SimpleEffectPuller m_SEPuller;
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

    public bool m_IsBulletTimeActivating { get; private set; } = false;

    private Coroutine m_SEThunderCoroutine;
    private Coroutine m_SEBeforeBTCoroutine;

    private BulletTime_AR m_BulletTime_AR;

    public GameObject m_ARScreenEffect { get; private set; }


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
        
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_Negotiator = m_Player.m_WeaponMgr.m_CurWeapon;
        
        var instance = InstanceMgr.GetInstance();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_InputMgr = instance.GetComponentInChildren<Player_InputMgr>();
        m_RageGauge = instance.m_MainCanvas.GetComponentInChildren<RageGauge>();
        m_SEPuller = instance.GetComponentInChildren<SimpleEffectPuller>();
        m_BulletTime_AR = instance.m_BulletTime_AR;
        
        m_MatChanger = GameMgr.GetInstance().p_MatChanger;
    }


    // Updates


    // Functions
    
    /// <summary>
    /// BulletTime 도중에 나오는 Thunder Effect Coroutine입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnThunderCoroutine()
    {
        while (true)
        {
            var effect = m_SEPuller.SpawnSimpleEffect(
                Random.Range(2, 5), m_Player.transform, true);

            yield return new WaitForSecondsRealtime(p_ThunderEffectDelay);
        }

        yield break;
    }

    /// <summary>
    /// BulletTime 키 입력을 받기 전까지 SE를 출력하는 Coroutine입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnBeforeBTPartCoroutine()
    {
        while (true)
        {
            var effect = m_SEPuller.SpawnSimpleEffect(
                Random.Range(0, 2), m_Player.transform, true, new Vector2(Random.Range(-0.1f, 0.1f), 0f));

            yield return new WaitForSecondsRealtime(p_BeforeBTEffectDelay);
        }
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
    /// BulletTimeMgr의 BulletTime 사용을 가능하게 변경합니다. 파티클도 출력합니다.
    /// </summary>
    public void SetCanUseBulletTime()
    {
        if (m_IsGaugeFull)
            return;
        
        m_IsGaugeFull = true;
        
        if (!ReferenceEquals(m_SEBeforeBTCoroutine, null))
        {
            StopCoroutine(m_SEBeforeBTCoroutine);
        } 
        m_SEBeforeBTCoroutine = StartCoroutine(SpawnBeforeBTPartCoroutine());
    }

    /// <summary>
    /// BulletTime 사용이 가능하게 바뀌었을 때, 입력 타이밍을 체크합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckBulletTimeStart()
    {
        StopCoroutine(m_SEBeforeBTCoroutine);
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
        //FireAll();
    }
    
    /// <summary>
    /// BulletTime을 시작합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_isStart">시작/중지 여부</param>
    public void ActivateBulletTime(bool _isStart)
    {
        if (_isStart)
        {
            m_RageGauge.TempStopRageGauge(true);

            m_SEThunderCoroutine = StartCoroutine(SpawnThunderCoroutine());
            m_IsBulletTimeActivating = true;
            
            m_MatChanger.ChangeMat(SpriteType.ENEMY, SpriteMatType.REDHOLO);
            m_MatChanger.ChangeMat(SpriteType.BACKGROUND, SpriteMatType.BnW);

            ChangeTimeScale(0f);
        }
        else
        {
            m_RageGauge.TempStopRageGauge(false);
            
            StopCoroutine(m_SEThunderCoroutine);
            
            m_IsBulletTimeActivating = false;
            
            m_MatChanger.ChangeMat(SpriteType.ENEMY, SpriteMatType.ORIGIN);
            m_MatChanger.ChangeMat(SpriteType.BACKGROUND, SpriteMatType.ORIGIN);

            ChangeTimeScale(1f);

            foreach (var element in m_MarkerList)
            {
                element.transform.parent = this.transform;
                element.SetActive(false);
            }
        }
    }
    
    
    /// <summary>
    /// 시간을 부드럽게 멈췄다가 다시 흐르게 합니다.
    /// </summary>
    /// <param name="_time">지정 시간</param>
    public void LerpingTimeScale(float _time)
    {
        StartCoroutine(CheckTimePassed(_time));
    }

    /// <summary>
    /// TimeScale을 수정합니다.
    /// </summary>
    /// <param name="_scale"></param>
    public void ChangeTimeScale(float _scale)
    {
        Time.timeScale = _scale;
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
    public void FireAll()
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
                m_SoundPlayer.PlayPlayerSoundOnce(1);
                m_BulletTimeParamList[i].m_FinalAction?.Invoke();
            }
            else
            {
                m_BulletTimeParamList[i].m_HotBox.HitHotBox(m_BulletTimeParamList[i].m_HotBoxParam);
                m_MarkerList[i].SetActive(false);
            
                m_SoundPlayer.PlayPlayerSoundOnce(1);
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
        m_IsGaugeFull = false;
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
        float timer = 0f;
        float speed = 1.15f;
        
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(7);
        while (true)
        {
            Time.timeScale /= speed;

            if (Time.timeScale <= 0.15f)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(0.02f);
        }
        
        while (true)
        {
            Time.timeScale *= speed;

            if (Time.timeScale >= 1f)
            {
                break;
            }
            
            yield return new WaitForSecondsRealtime(0.02f);
        }
        

        Time.timeScale = 1f;

        yield break;
    }
}