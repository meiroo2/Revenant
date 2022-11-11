using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class TimeSliceObj : MonoBehaviour
{
    // Visible Member Variables
    public TimeSliceMgr p_TimeSliceMgr;
    public TimeSliceBullet p_Bullet;
    public TimeSliceCircleCol p_CircleCol;
    
    public SpriteRenderer p_Renderer;
    public Collider2D p_FloorCol;
    public Collider2D p_BulletCol;

    [Title("CirclePoint")]
    public Transform p_CircleTransform;
    
    // Member Variables
    private float m_MoveSpeed = 1f;
    private float m_ColorSpeed = 1f;
    private float m_RemainTime = 1f;
    public bool m_IsFired = false;

    private Camera m_Cam;
    private CameraMgr m_CamMgr;
    private Vector2 m_CamCorrectionPos;

    [HideInInspector] public Player m_Player;
    private Color m_Color = Color.white;

    private Coroutine m_LifeCycleCoroutine = null;
    private Coroutine m_FollowCoroutine = null;
    private Coroutine m_ColorCoroutine = null;

    public Action m_OnHitAction = null;
    
    // Constructors
    private void Awake()
    {
        m_Cam = Camera.main;
        m_CamMgr = m_Cam.GetComponent<CameraMgr>();
        
        p_FloorCol.enabled = false;
        p_BulletCol.enabled = true;
        m_FollowCoroutine = null;
        m_Color = Color.white;
        m_Color.a = 0f;
        
        
        p_Renderer.color = m_Color;
    }

    /// <summary>
    /// TimeSliceObj를 준비시킵니다.
    /// </summary>
    /// <param name="_speed"></param>
    /// <param name="_colorSpeed"></param>
    /// <param name="_angle"></param>
    /// <param name="_remainTime"></param>
    public void ResetTimeSliceObj(float _speed, float _colorSpeed, float _angle, float _remainTime)
    {
        if (!ReferenceEquals(m_FollowCoroutine, null))
        {
            StopCoroutine(m_FollowCoroutine);
            m_FollowCoroutine = null;
        }

        if (!ReferenceEquals(m_ColorCoroutine, null))
        {
            StopCoroutine(m_ColorCoroutine);
            m_ColorCoroutine = null;
        }

        m_IsFired = false;
        transform.parent = null;
        m_MoveSpeed = _speed;
        m_ColorSpeed = _colorSpeed;
        m_RemainTime = _remainTime;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
        
        m_OnHitAction = null;
        
        p_FloorCol.enabled = false;
        p_BulletCol.enabled = true;
        m_Color = Color.white;
        m_Color.a = 0f;

        p_Renderer.color = m_Color;
    }
    
    
    // Functions

    /// <summary>
    /// CircleCol의 정보를 획득합니다.
    /// </summary>
    /// <param name="_infoIdx">0이면 끝까지 내려감, 1이면 맞음</param>
    public void GetCircleColInfo(int _infoIdx)
    {
        switch (_infoIdx)
        {
            case 0:
                break;
            
            case 1:
                m_OnHitAction?.Invoke();
                gameObject.SetActive(false);
                break;
        }
    }
    
    public void Activate()
    {
        if (m_IsFired)
            return;
        
        m_IsFired = true;
        p_Bullet.Fire();
        p_BulletCol.enabled = false;
        p_FloorCol.enabled = true;

        m_LifeCycleCoroutine = StartCoroutine(LifeCycle());
    }
    
    public void StartFollow()
    {
        transform.position = m_Cam.transform.position;
        m_FollowCoroutine = StartCoroutine(Following());
        m_ColorCoroutine = StartCoroutine(ColorChanging());
    }

    public void Stop()
    {
        if (!ReferenceEquals(m_FollowCoroutine, null))
        {
            StopCoroutine(m_FollowCoroutine);
            m_FollowCoroutine = null;
        }
        if (!ReferenceEquals(m_ColorCoroutine, null))
        {
            StopCoroutine(m_ColorCoroutine);
            m_ColorCoroutine = null;
        }
        m_Color = Color.yellow;
        m_Color.a = 1f;
        p_Renderer.color = m_Color;
    }
    
    private IEnumerator ColorChanging()
    {
        m_Color.a = 0f;
        p_Renderer.color = m_Color;
        while (true)
        {
            m_Color.a += Time.deltaTime * m_ColorSpeed;
            p_Renderer.color = m_Color;

            if (m_Color.a >= 1f)
            {
                m_Color.a = 1f;
                p_Renderer.color = m_Color;
                break;
            }

            yield return null;
        }
        
        Debug.Log("Color Fadein End");
        
        yield break;
    }
    
    private IEnumerator Following()
    {
        while (true)
        {
            m_CamCorrectionPos = m_Cam.transform.position;
            m_CamCorrectionPos.y -= m_CamMgr.p_YValue;
            transform.position = Vector2.Lerp(transform.position,
                m_CamCorrectionPos, Time.deltaTime * m_MoveSpeed);
            
            yield return null;
        }
        
        yield break;
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(m_RemainTime);
        transform.parent = p_TimeSliceMgr.transform;
        gameObject.SetActive(false);
    }
}