using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BulletTime 클래스에 사격 정보를 예약시 쓰는 파라미터용 클래스입니다.
/// </summary>
public class BulletTimeParam
{
    public IHotBox m_HotBox { get; private set; }
    public IHotBoxParam m_HotBoxParam { get; private set; }

    public BulletTimeParam(IHotBox _box, IHotBoxParam _param)
    {
        m_HotBox = _box;
        m_HotBoxParam = _param;
    }
}

public class BulletTimeMgr : MonoBehaviour
{
    // Visible Member Variables
    public float p_ShotDelayTime = 0.1f;
    public GameObject p_Marker;
    
    // Member Variables
    public bool m_CanUseBulletTime { get; private set; } = false;

    private BasicWeapon m_Negotiator;
    private Player_InputMgr m_InputMgr;
    
    private HitSFXMaker m_HitSFXMaker;
    private SoundPlayer m_SFXSoundMgr;

    private List<GameObject> m_MarkerList = new List<GameObject>();
    private int m_MarkerIdx = 0;

    private List<BulletTimeParam> m_BulletTimeParamList = new List<BulletTimeParam>();
    private Coroutine m_Coroutine;

    public bool m_BulletTimeActivating { get; private set; } = false;


    // Constructors

    private void Start()
    {
        // Awake시 마커용 오브젝트풀 생성
        for (int i = 0; i < 7; i++)
        {
            var element = Instantiate(p_Marker, this.transform, true);
            m_MarkerList.Add(element);
            element.SetActive(false);
        }
        
        var instance = InstanceMgr.GetInstance();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SFXSoundMgr = instance.GetComponentInChildren<SoundPlayer>();
        m_InputMgr = instance.GetComponentInChildren<Player_InputMgr>();
        m_Negotiator = instance.GetComponentInChildren<Player_Manager>().m_Player.m_WeaponMgr.m_CurWeapon;
    }


    // Updates


    // Functions

    
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
                break;
            }
            yield return null;
        }
        
        ActivateBulletTime(true);

        while (true)
        {
            if (m_Negotiator.m_LeftRounds == 0)
                break;
            
            yield return null;
        }
        
        ActivateBulletTime(false);
        FireAll();
    }
    
    /// <summary>
    /// BulletTime을 시작합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_isStart">시작/중지 여부</param>
    public void ActivateBulletTime(bool _isStart)
    {
        if (_isStart)
        {
            m_BulletTimeActivating = true;
            Time.timeScale = 0.05f;
        }
        else
        {
            m_BulletTimeActivating = false;
            Time.timeScale = 1f;

            foreach (var element in m_MarkerList)
            {
                element.transform.parent = this.transform;
                element.SetActive(false);
            }
            
            m_MarkerIdx = 0;
            
            m_BulletTimeParamList.Clear();
            m_BulletTimeParamList.TrimExcess();
        }
    }
    
    /// <summary>
    /// 오직 시간만을 잠시 수정합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_time">지정 시간</param>
    public void ActivateTimeScale(float _time)
    {
        Time.timeScale = 0f;
        StartCoroutine(CheckTimePassed(_time));
    }
    
    /// <summary>
    /// 파라미터로 들어온 사격 정보를 예약합니다. 최대 8발까지만 예약 가능
    /// </summary>
    /// <param name="_param">사격 정보</param>
    public void BookFire(BulletTimeParam _param)
    {
        if (m_MarkerIdx > 6)
            return;
        
        m_BulletTimeParamList.Add(_param);
        
        var marker = m_MarkerList[m_MarkerIdx];
        marker.SetActive(true);
        marker.transform.position = _param.m_HotBoxParam.m_contactPoint;
        marker.transform.parent = _param.m_HotBox.m_ParentObj.transform;

        m_MarkerIdx++;
    }

    /// <summary>
    /// 예약된 모든 사격 정보를 기반으로 발사합니다.
    /// </summary>
    public void FireAll()
    {
        if(!ReferenceEquals(m_Coroutine, null))
            StopCoroutine(m_Coroutine);
        
        m_Coroutine = StartCoroutine(FireCoroutine());
    }

    /// <summary>
    /// FireAll() 함수에서 사용하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireCoroutine()
    {
        int markerIdx = 0;
        foreach (var element in m_BulletTimeParamList)
        {
            Debug.Log("wow" + markerIdx);
            element.m_HotBox.HitHotBox(element.m_HotBoxParam);
            m_MarkerList[markerIdx].SetActive(false);
            markerIdx++;
            
            yield return new WaitForSecondsRealtime(p_ShotDelayTime);

        }
        
        yield break;
    }

    private IEnumerator CheckTimePassed(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1f;
    }
}