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
    private HitSFXMaker m_HitSFXMaker;
    private SoundPlayer m_SFXSoundMgr;

    private List<GameObject> m_MarkerList = new List<GameObject>();
    private int m_MarkerIdx = 0;

    private List<BulletTimeParam> m_BulletTimeParamList = new List<BulletTimeParam>();
    private Coroutine m_Coroutine;
    
    
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
    }


    // Updates


    // Functions
    
    /// <summary>
    /// BulletTime을 시작합니다. Timescale이 0이 됩니다.
    /// </summary>
    /// <param name="_isStart">시작/중지 여부</param>
    public void ActivateBulletTime(bool _isStart)
    {
        if (_isStart)
        {
            Time.timeScale = 0f;
        }
        else
        {
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
    /// 파라미터로 들어온 사격 정보를 예약합니다. 최대 8발까지만 예약 가능
    /// </summary>
    /// <param name="_param">사격 정보</param>
    public void BookFire(BulletTimeParam _param)
    {
        if (m_MarkerIdx > 7)
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
            m_HitSFXMaker.EnableNewObj(element.m_HotBox.m_HitBoxInfo == HitBoxPoint.HEAD ? 1 : 0,
                element.m_HotBoxParam.m_contactPoint);
            
            m_SFXSoundMgr.playAttackedSound(MatType.Normal, element.m_HotBoxParam.m_contactPoint);

            element.m_HotBox.HitHotBox(element.m_HotBoxParam);
            m_MarkerList[markerIdx].SetActive(false);
            markerIdx++;
            
            yield return new WaitForSecondsRealtime(p_ShotDelayTime);

        }
        
        yield break;
    }
}