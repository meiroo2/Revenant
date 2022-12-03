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
    
    public SpriteRenderer p_FrontCutRenderer;
    public Collider2D p_FloorCol;
    public Collider2D p_BulletCol;
    
    public Material p_FrontCutMaterial;

    [Title("CirclePoint")]
    public Transform p_CircleTransform;
    
    // Member Variables
    private Material m_UnlitMat;
    
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
    private Coroutine m_FadeInOnFireCoroutine = null;

    public Action m_OnHitAction = null;
    private readonly int ShadowTime = Shader.PropertyToID("_ShadowTime");

    // Constructors
    private void Awake()
    {
        m_UnlitMat = p_FrontCutRenderer.material;
        
        m_Cam = Camera.main;
        m_CamMgr = m_Cam.GetComponent<CameraMgr>();
        
        p_FloorCol.enabled = false;
        p_BulletCol.enabled = true;
        m_FollowCoroutine = null;
        m_Color = Color.red;
        m_Color.a = 0f;
        
        p_FrontCutRenderer.color = m_Color;
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
        m_Color = Color.red;
        m_Color.a = 0f;

        p_FrontCutRenderer.material = m_UnlitMat;
        p_FrontCutRenderer.color = m_Color;
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

        p_FrontCutRenderer.material = p_FrontCutMaterial;
        p_FrontCutRenderer.material.SetFloat(ShadowTime, 0f);
        p_FrontCutRenderer.color = Color.white;
        
        m_IsFired = true;
        p_Bullet.Fire();
        p_BulletCol.enabled = false;
        p_FloorCol.enabled = true;

        m_LifeCycleCoroutine = StartCoroutine(LifeCycle());

        // Cut하고 천천히 들어오는 알파값
        if (!ReferenceEquals(m_FadeInOnFireCoroutine, null))
        {
            StopCoroutine(m_FadeInOnFireCoroutine);
            m_FadeInOnFireCoroutine = null;
        }
        m_FadeInOnFireCoroutine = StartCoroutine(FadeInOnFireEnumerator());
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
        p_FrontCutRenderer.color = m_Color;
    }
    
    private IEnumerator ColorChanging()
    {
        m_Color.a = 0f;
        p_FrontCutRenderer.color = m_Color;
        while (true)
        {
            m_Color.a += Time.deltaTime * m_ColorSpeed;
            p_FrontCutRenderer.color = m_Color;

            if (m_Color.a >= 1f)
            {
                m_Color.a = 1f;
                p_FrontCutRenderer.color = m_Color;
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
            /*
            m_CamCorrectionPos = m_Cam.transform.position;
            m_CamCorrectionPos.y -= m_CamMgr.p_YValue;
            transform.position = Vector2.Lerp(transform.position,
                m_CamCorrectionPos, Time.deltaTime * m_MoveSpeed);
            */
            
            transform.position = Vector2.Lerp(transform.position,
                m_Player.GetPlayerCenterPos(), Time.deltaTime * m_MoveSpeed);
            
            yield return null;
        }
        
        yield break;
    }

    /// <summary>
    /// World에 생성되는 TimeSlice를 생성하되 Alpha값으로 천천히 들어오게 함.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInOnFireEnumerator()
    {
        Color whiteColor = Color.white;
        whiteColor.a = 0f;
        p_FrontCutRenderer.color = whiteColor;
        
        yield return new WaitForSecondsRealtime(1.3f);

        while (true)
        {
            p_FrontCutRenderer.color = whiteColor;
            whiteColor.a += Time.deltaTime;

            if (whiteColor.a >= 1f)
            {
                whiteColor.a = 1f;
                p_FrontCutRenderer.color = whiteColor;
                break;
            }
            yield return null;
        }
        
        yield break;
    }
    
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(m_RemainTime);

        float matVal = 0f;
        while (true)
        {
            p_FrontCutRenderer.material.SetFloat(ShadowTime, matVal);
            matVal += Time.deltaTime;
            
            if (matVal >= 1f)
            {
                matVal = 1f;
                p_FrontCutRenderer.material.SetFloat(ShadowTime, matVal);
                break;
            }
            yield return null;
        }
        
        transform.parent = p_TimeSliceMgr.transform;
        gameObject.SetActive(false);
    }
}