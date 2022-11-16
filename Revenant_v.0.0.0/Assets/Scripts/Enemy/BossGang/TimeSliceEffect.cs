﻿using System;
using System.Collections;
using UnityEngine;


public class TimeSliceEffect : MonoBehaviour
{
    private ScreenCaptureMgr m_ScreenCap;
    public CameraMgr m_CamMgr;
    public SpriteRenderer m_Renderer;
    public GameObject m_SpriteObj;
    public float m_THICCVAL;
    public float m_ColorSpeed = 1f;
    public float m_ScaleSpeed = 1f;

    public float m_XPos = 0f;
    public float m_ANgle = 48f;

    public SpriteRenderer m_SquareRenderer;
    
    
    private Vector2 m_OriginLocalScale;
    private Vector2 m_BigLocalScale;

    private Coroutine m_Coroutine = null;

    private void Awake()
    {
        Transform objTransform = m_SpriteObj.transform;
        m_OriginLocalScale = objTransform.localScale;
        m_BigLocalScale = m_OriginLocalScale;
        m_BigLocalScale.x *= m_THICCVAL;

        objTransform.localScale = new Vector2(0f, m_OriginLocalScale.y);

        Color squareColor = Color.white;
        squareColor.a = 0f;
        m_SquareRenderer.color = squareColor;
    }

    private void Start()
    {
        m_ScreenCap = InstanceMgr.GetInstance().GetComponentInChildren<ScreenCaptureMgr>();
    }

    public void Update()
    {
        Vector2 position = m_CamMgr.transform.position;
        transform.position = new Vector2(position.x,
          position.y -= m_CamMgr.p_YValue);
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_ScreenCap.Capture(transform.position.x + m_XPos, transform.position.y, m_ANgle);
            if (!ReferenceEquals(m_Coroutine, null))
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }

            m_Coroutine = StartCoroutine(WOWSANS());
        }
    }

    private IEnumerator WOWSANS()
    {
        Transform objTransform = m_SpriteObj.transform;
        objTransform.localScale = m_BigLocalScale;
        
        Vector2 lerpFinalScale = m_BigLocalScale;
        lerpFinalScale.x = 0f;

        float lerpVal = 0f;

        Color blueColor = Color.blue;
        Color whiteColor = Color.white;

        Color blackColor = Color.black;
        Color clearColor = Color.black;
        clearColor.a = 0f;

        m_SquareRenderer.color = blackColor;
        while (true)
        {
            if (lerpVal < 1f)
            {
                lerpVal += Time.unscaledDeltaTime * m_ScaleSpeed;
                objTransform.localScale = 
                    Vector2.Lerp(m_BigLocalScale, lerpFinalScale, lerpVal);
                m_Renderer.color = Vector4.Lerp(blueColor, whiteColor, lerpVal);
            }
            else
            {
                lerpVal = 1f;
                objTransform.localScale = 
                    Vector2.Lerp(m_BigLocalScale, lerpFinalScale, lerpVal);
                m_Renderer.color = Vector4.Lerp(blueColor, whiteColor, lerpVal);
                break;
            }
            yield return null;
        }

        lerpVal = 0f;
        yield return new WaitForSecondsRealtime(0.4f);
        while (true)
        {
            lerpVal += Time.unscaledDeltaTime * m_ScaleSpeed;
            m_SquareRenderer.color = Vector4.Lerp(blackColor, clearColor, lerpVal / 2f);
            yield return null;
        }
      
    }
}